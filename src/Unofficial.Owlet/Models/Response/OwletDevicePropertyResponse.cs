using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Unofficial.Owlet.Models.Response
{
    public class OwletDevicePropertyResponse
    {
        [JsonProperty("property")]
        public OwletProperty Property { get; set; }
    }

    public static class OwletDevicePropertyResponseExtensions
    {
        private static string AppActivePropertyName = "APP_ACTIVE";

        public static OwletProperty GetAppActiveProperty(this IEnumerable<OwletDevicePropertyResponse> deviceProperties)
        {
            if (deviceProperties == null)
            {
                throw new ArgumentNullException(nameof(deviceProperties));
            }
            return deviceProperties.FirstOrDefault(p => p.Property.Name == AppActivePropertyName)?.Property;
        }
    }
}
