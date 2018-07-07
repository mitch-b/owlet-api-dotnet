using System;
using System.Collections.Generic;

namespace Unofficial.Owlet.Interfaces
{
    /// <summary>
    /// Allow configuring data to make Owlet API consumption easier for your use case.
    /// </summary>
    public interface IOwletApiSettings
    {
        /// <summary>
        /// Your Owlet Smart Sock account email
        /// </summary>
        string Email { get; set; }
        /// <summary>
        /// Your Owlet Smart Sock account password
        /// </summary>
        string Password { get; set; }
        /// <summary>
        /// Interval between retry requests for new Owlet Smart Sock data in <see cref="IOwletDeviceApi.TryGetFreshDevicePropertiesAsync"/>.
        /// </summary>
        TimeSpan? FreshDataRetryPeriod { get; set; }
        /// <summary>
        /// Prevent an additional API request to look this value up if you already know the propertyId for your Owlet Smart Sock's APP_ACTIVE property.
        /// </summary>
        int? AppActivePropertyId { get; set; }
        /// <summary>
        /// Prevent necessity of calling <see cref="IOwletDeviceApi.GetDevicesAsync"/> if you already know your Dsn (deviceSerialNumber) (or multiple).
        /// </summary>
        IEnumerable<string> KnownDeviceSerialNumbers { get; set; }
    }
}
