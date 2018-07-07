using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Unofficial.Owlet.Interfaces;
using Unofficial.Owlet.Models.Response;
using Unofficial.Owlet.TestConsole.Models;

namespace Unofficial.Owlet.TestConsole
{
    public class Application
    {
        private readonly MyAppConfiguration _myAppConfiguration;
        private readonly IOwletUserApi _owletUserApi;
        private readonly IOwletDeviceApi _owletDeviceApi;

        public Application(IOptions<MyAppConfiguration> owletConfigurationOptions, IOwletUserApi owletUserApi, IOwletDeviceApi owletDeviceApi)
        {
            this._myAppConfiguration = owletConfigurationOptions.Value;
            this._owletUserApi = owletUserApi;
            this._owletDeviceApi = owletDeviceApi;
        }

        public async Task Run()
        {
            // do not need to make explicit call to this._owletUserApi.LoginAsync(email, password) since I provided that information in 
            // Startup configuration.

            var accountDevices = await this._owletDeviceApi.GetDevicesAsync();
            var ourSock = accountDevices.First();
            if (ourSock == null)
            {
                Console.WriteLine("== NO SOCKS");
                return;
            }

            var deviceData = await this._owletDeviceApi.TryGetFreshDevicePropertiesAsync(ourSock.Device.DeviceSerialNumber);
            PrintDeviceDataToConsole(deviceData);

            Console.WriteLine("== DONE");
            Console.Read();
        }

        private void PrintDeviceDataToConsole(IEnumerable<OwletDevicePropertyResponse> deviceData)
        {
            foreach (var property in deviceData)
            {
                Console.WriteLine($"{property.Property.Name} : {property.Property.Value} ({property.Property.DataUpdatedAt?.ToString("MM/dd/yyyy HH:mm")})");
            }
        }

    }
}
