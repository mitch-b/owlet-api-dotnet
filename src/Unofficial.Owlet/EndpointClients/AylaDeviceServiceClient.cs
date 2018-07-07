using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Unofficial.Owlet.Models.Response;
using Unofficial.Owlet.Models.Request;

namespace Unofficial.Owlet.EndpointClients
{
    /// <summary>
    /// See: https://developer.aylanetworks.com/apibrowser/swaggers/DeviceService
    /// </summary>
    public class AylaDeviceServiceClient : AylaServiceClient
    {
        private const string DeviceApiBaseaddress = "https://ads-field.aylanetworks.com";
        public AylaDeviceServiceClient(HttpClient httpClient)
            : base(httpClient)
        {
            httpClient.BaseAddress = new Uri(DeviceApiBaseaddress);
        }

        public async Task<IEnumerable<OwletDeviceResponse>> GetDevicesAsync(string accessToken)
        {
            CheckArgument(accessToken, nameof(accessToken));

            var headers = new Dictionary<string, string>()
                .AddAuthorizationHeader(accessToken);

            var response = await this.SendRequest(HttpMethod.Get, "/apiv1/devices.json", null, headers);

            response.EnsureSuccessStatusCode();

            return await GetFromResponse<IEnumerable<OwletDeviceResponse>>(response);
        }

        public async Task<OwletDevicePropertyResponse> GetPropertyByNameAsync(string deviceSerialNumber, string propertyName,
            string accessToken)
        {
            CheckArgument(deviceSerialNumber, nameof(deviceSerialNumber));
            CheckArgument(propertyName, nameof(propertyName));
            CheckArgument(accessToken, nameof(accessToken));

            var headers = new Dictionary<string, string>()
                .AddAuthorizationHeader(accessToken);

            var response = await this.SendRequest(HttpMethod.Get, $"/apiv1/dsns/{deviceSerialNumber}/properties/{propertyName}.json", null, headers);

            response.EnsureSuccessStatusCode();

            return await GetFromResponse<OwletDevicePropertyResponse>(response);
        }

        public async Task<IEnumerable<OwletDevicePropertyResponse>> GetDevicePropertiesAsync(string deviceSerialNumber, string accessToken)
        {
            CheckArgument(deviceSerialNumber, nameof(deviceSerialNumber));
            CheckArgument(accessToken, nameof(accessToken));

            var headers = new Dictionary<string, string>()
                .AddAuthorizationHeader(accessToken);

            var response = await this.SendRequest(HttpMethod.Get, $"/apiv1/dsns/{deviceSerialNumber}/properties.json", null, headers);

            response.EnsureSuccessStatusCode();

            return await GetFromResponse<IEnumerable<OwletDevicePropertyResponse>>(response);
        }

        public async Task<OwletSignInResponse> UpdatePropertyAsync(int propertyId, object value, string accessToken)
        {
            CheckArgument(accessToken, nameof(accessToken));

            var headers = new Dictionary<string, string>()
                .AddAuthorizationHeader(accessToken);

            var data = this.ToJsonPayload(new OwletDatapointRequest(value));

            var response = await this.SendRequest(HttpMethod.Post, $"/apiv1/properties/{propertyId}/datapoints.json", data, headers);

            response.EnsureSuccessStatusCode();

            return await GetFromResponse<OwletSignInResponse>(response);
        }
    }
}
