using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unofficial.Owlet.Models;
using Unofficial.Owlet.Models.Response;

namespace Unofficial.Owlet.Interfaces
{
    /// <summary>
    /// Provides access to device-level information of your Owlet Smart Sock.
    /// <para>
    /// Available endpoints include device information, vitals, and logging your own datapoints.
    /// </para>
    /// </summary>
    public interface IOwletDeviceApi
    {
        /// <summary>
        /// Returns available devices associated with your Owlet user account.
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        Task<IEnumerable<OwletDeviceResponse>> GetDevicesAsync(string accessToken = null);
        /// <summary>
        /// Returns a specific device property. (HEART_RATE, OXYGEN_LEVEL, MOVEMENT, BATT_LEVEL, BASE_STATION_ON, etc)
        /// </summary>
        /// <param name="deviceSerialNumber">Your smart sock DSN (access via <see cref="IOwletDeviceApi.GetDevicesAsync(string)"/>)</param>
        /// <param name="propertyName">A valid device property (HEART_RATE, OXYGEN_LEVEL, MOVEMENT, BATT_LEVEL, BASE_STATION_ON, etc)</param>
        /// <param name="accessToken">If provided, will use accessToken for authentication. Otherwise, will try to log you in based on provided <see cref="IOwletApiSettings.Email"/> and <see cref="IOwletApiSettings.Password"/>.</param>
        /// <returns></returns>
        Task<OwletProperty> GetPropertyByNameAsync(string deviceSerialNumber, string propertyName, string accessToken = null);
        /// <summary>
        /// Latest available Owlet Smart Sock properties.
        /// </summary>
        /// <param name="deviceSerialNumber">Your smart sock DSN (access via <see cref="IOwletDeviceApi.GetDevicesAsync(string)"/>)</param>
        /// <param name="accessToken">If provided, will use accessToken for authentication. Otherwise, will try to log you in based on provided <see cref="IOwletApiSettings.Email"/> and <see cref="IOwletApiSettings.Password"/>.</param>
        /// <returns></returns>
        Task<IEnumerable<OwletDevicePropertyResponse>> GetDevicePropertiesAsync(string deviceSerialNumber, string accessToken = null);
        /// <summary>
        /// Will poll the Owlet Smart Sock properties until new values appear.
        /// <para>
        /// This is achieved by marking the property APP_ACTIVE truthy, 
        /// then allowing time for the Owlet API to register the latest datapoints from your local devices.
        /// </para>
        /// </summary>
        /// <param name="deviceSerialNumber">Your smart sock Dsn (deviceSerialNumber) (access via <see cref="IOwletDeviceApi.GetDevicesAsync(string)"/>)</param>
        /// <param name="retryPeriod">If provided, will keep retrying over this interval until new data is found. (Max retry attempts: 5)</param>
        /// <param name="accessToken">If provided, will use accessToken for authentication. Otherwise, will try to log you in based on provided <see cref="IOwletApiSettings.Email"/> and <see cref="IOwletApiSettings.Password"/>.</param>
        /// <returns></returns>
        Task<IEnumerable<OwletDevicePropertyResponse>> TryGetFreshDevicePropertiesAsync(string deviceSerialNumber, TimeSpan? retryPeriod = null, string accessToken = null);
        /// <summary>
        /// Register a new datapoint for your Owlet Smart Sock
        /// 
        /// <para>
        /// WARNING: This is potentially not a reversible action. Use with care.
        /// </para>
        /// </summary>
        /// <param name="propertyId">Property ID is related to a Property Name, yet unique for each Smart Sock. Find your appropriate propertyId via <see cref="IOwletDeviceApi.GetPropertyByNameAsync(string, string, string)"/></param>
        /// <param name="value"></param>
        /// <param name="accessToken">If provided, will use accessToken for authentication. Otherwise, will try to log you in based on provided <see cref="IOwletApiSettings.Email"/> and <see cref="IOwletApiSettings.Password"/>.</param>
        /// <returns></returns>
        Task UpdatePropertyAsync(int propertyId, object value, string accessToken = null);
        /// <summary>
        /// Will look up your specific Owlet Smart Sock's APP_ACTIVE property and register it as a truthy value. This property must be truthy for data to update in the Owlet API.
        /// <para>
        /// INFO: It appears this value will reset to falsy after about 30 seconds.
        /// </para>
        /// <para>
        /// WARNING: This method will write a 1 value to this property. Be sure you are using a boolean-valued property.
        /// </para>
        /// </summary>
        /// <param name="propertyId">
        /// <para>Provide a 0 to have this value determined for you automatically.</para>
        /// If known, will use this propertyId instead of looking it up for you based on your Dsn (deviceSerialNumber).
        /// Find your appropriate propertyId via <see cref="IOwletDeviceApi.GetPropertyByNameAsync(string, string, string)"/>
        /// </param>
        /// <param name="deviceSerialNumber">Your smart sock Dsn (deviceSerialNumber) (access via <see cref="IOwletDeviceApi.GetDevicesAsync(string)"/>)</param>
        /// <param name="accessToken">If provided, will use accessToken for authentication. Otherwise, will try to log you in based on provided <see cref="IOwletApiSettings.Email"/> and <see cref="IOwletApiSettings.Password"/>.</param>
        /// <returns></returns>
        Task MarkAppActive(int propertyId = 0, string deviceSerialNumber = null, string accessToken = null);
    }
}
