using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unofficial.Owlet.EndpointClients;
using Unofficial.Owlet.Interfaces;
using Unofficial.Owlet.Models;
using Unofficial.Owlet.Models.Response;

namespace Unofficial.Owlet.Services
{
    public class OwletDeviceApi : OwletApi, IOwletDeviceApi
    {
        private readonly AylaDeviceServiceClient _aylaDeviceServiceClient;
        public OwletDeviceApi(AylaDeviceServiceClient aylaDeviceServiceClient, AylaUserServiceClient aylaUserServiceClient, IOwletApiSettings owletApiSettings, OwletUserSession userSession)
            : base(owletApiSettings, userSession, aylaUserServiceClient)
        {
            this._aylaDeviceServiceClient = aylaDeviceServiceClient;
        }

        public async Task<IEnumerable<OwletDeviceResponse>> GetDevicesAsync(string accessToken = null)
        {
            accessToken = await GetAccessToken(accessToken);
            return await this._aylaDeviceServiceClient.GetDevicesAsync(accessToken);
        }

        public async Task<IEnumerable<OwletDevicePropertyResponse>> GetDevicePropertiesAsync(string deviceSerialNumber, string accessToken = null)
        {
            accessToken = await GetAccessToken(accessToken);
            return await this._aylaDeviceServiceClient.GetDevicePropertiesAsync(deviceSerialNumber, accessToken);
        }

        public async Task<IEnumerable<OwletDevicePropertyResponse>> TryGetFreshDevicePropertiesAsync(string deviceSerialNumber, TimeSpan? retryPeriod = null, string accessToken = null)
        {
            accessToken = await GetAccessToken(accessToken);
            var startTime = DateTimeOffset.UtcNow;
            var initialProperties = Enumerable.Empty<OwletDevicePropertyResponse>();

            // TODO: probably add these as const or config values
            var considerFreshSinceSeconds = TimeSpan.FromSeconds(10);
            var waitTimeAfterMarkAppActive = TimeSpan.FromSeconds(2);
            var retryAttempts = 5;

            if (!retryPeriod.HasValue && this._owletApiSettings.FreshDataRetryPeriod.HasValue)
            {
                retryPeriod = this._owletApiSettings.FreshDataRetryPeriod.Value;
            }
            else
            {
                retryPeriod = TimeSpan.FromSeconds(3);
            }

            await this.MarkAppActive(0, deviceSerialNumber, accessToken);

            // await initial duration
            // this gives owlet sock & base station *some* time to 
            // advertise out to Ayla Networks IoT platform with device details
            await Task.Delay(waitTimeAfterMarkAppActive);

            while (retryAttempts > 0)
            {
                var properties = await this.GetDevicePropertiesAsync(deviceSerialNumber, accessToken);
                if (properties.Any())
                {
                    if (!initialProperties.Any())
                    {
                        initialProperties = properties.ToList();
                    }

                    // APP_ACTIVE is truthy
                    if (properties.GetAppActiveProperty()?.GetValue<int>() == 1)
                    {
                        var latestProperty = properties.OrderByDescending(p => p.Property.DataUpdatedAt).FirstOrDefault();

                        // check if : latestProperty.DataUpdatedAt > (start time - freshSinceSeconds)
                        if (latestProperty?.Property?.DataUpdatedAt != null && latestProperty.Property.DataUpdatedAt >
                            startTime.AddSeconds(-1 * considerFreshSinceSeconds.Seconds))
                        {
                            return properties;
                        }
                    }
                    else
                    {
                        Console.WriteLine("* App not reading Active. Retrying...");
                    }
                }
                await Task.Delay(retryPeriod.Value);
                retryAttempts--;
            }

            return initialProperties;
        }

        public async Task UpdatePropertyAsync(int propertyId, object value, string accessToken = null)
        {
            accessToken = await GetAccessToken(accessToken);
            await this._aylaDeviceServiceClient.UpdatePropertyAsync(propertyId, value, accessToken);
        }

        public async Task<int> TryGetAppActivePropertyId(string deviceSerialNumber = null, string accessToken = null)
        {
            var appActivePropertyId = 0;
            if (this._owletApiSettings.AppActivePropertyId.HasValue)
            {
                appActivePropertyId = this._owletApiSettings.AppActivePropertyId.Value;
            }
            else
            {
                accessToken = await GetAccessToken(accessToken);
                var appActiveProperty = await this.GetPropertyByNameAsync(deviceSerialNumber, "APP_ACTIVE", accessToken);
                if (appActiveProperty == null)
                {
                    return 0;
                }

                appActivePropertyId = appActiveProperty.Key;
                if (appActivePropertyId != 0)
                {
                    // persist for current session
                    this._owletApiSettings.AppActivePropertyId = appActivePropertyId;
                }
            }
            return appActivePropertyId;
        }

        public async Task MarkAppActive(int propertyId = 0, string deviceSerialNumber = null, string accessToken = null)
        {
            accessToken = await GetAccessToken(accessToken);
            if (propertyId == 0)
            {
                propertyId = await this.TryGetAppActivePropertyId(deviceSerialNumber, accessToken);
            }
            await this._aylaDeviceServiceClient.UpdatePropertyAsync(propertyId, 1, accessToken);
        }

        public async Task<OwletProperty> GetPropertyByNameAsync(string deviceSerialNumber, string propertyName,
            string accessToken = null)
        {
            accessToken = await GetAccessToken(accessToken);
            var deviceResponse = await this._aylaDeviceServiceClient.GetPropertyByNameAsync(deviceSerialNumber, propertyName, accessToken);
            return deviceResponse.Property;
        }
    }
}
