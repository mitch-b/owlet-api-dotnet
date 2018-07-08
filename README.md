# Owlet Smart Sock API for .NET

**Unofficial** .NET Standard 2.0 Library. Run on Linux, Mac, Windows.

[![NuGet](https://img.shields.io/nuget/vpre/Unofficial.Owlet.svg)](https://www.nuget.org/packages/Unofficial.Owlet)

![Build status](https://mitch.visualstudio.com/_apis/public/build/definitions/97a642db-3f4a-4c58-a7a7-6155f74394c2/2/badge)
![nuget.org Release](https://mitch.vsrm.visualstudio.com/_apis/public/Release/badge/97a642db-3f4a-4c58-a7a7-6155f74394c2/1/5)

## Notice

This code is in no way affiliated with, authorized, maintained, sponsored or endorsed by Owlet Baby Care or any of its affiliates or subsidiaries. This is an independent and unofficial API. **Use at your own risk.**

## Fair Warning

> ### :warning: Some method calls possible by utilizing this API **could cause instability with your Owlet Smart Sock**. Particularly any usage of the following methods:
>
> - `IOwletDeviceApi.UpdatePropertyAsync`
> - `IOwletDeviceApi.MarkAppActive`
> - `IOwletDeviceApi.TryGetFreshDevicePropertiesAsync`
>
> Which exist so that the Owlet Base Station and Smart Sock can advertise new datapoints to the AylaNetworks IoT platform that this API wrapper can query - which requires your Owlet device to broadcast `APP_ACTIVE = 1`. This framework will handle managing your `APP_ACTIVE` property. You will assume the risk if this changes.
>
> I say "could cause instability" because even though I have not experienced any adverse side-effects, there is no documented usage guidelines provided by Owlet Baby Care, so I do not know exactly how they internally use the Ayla Networks IoT platform to aggregate data for their own Connected Care reporting application. That all being said, I have used this code to my own success.
>
> If you are not comfortable with this to pull near-real-time data, you can utilize the `IOwletDeviceApi.GetDevicePropertiesAsync` method which **will not update any device properties on your behalf**. It will instead pull values from the last time you had the Owlet app open on your mobile device.

## Example Data

Device Properties for specific Owlet Smart Sock:

```json
[
  {
    "Name": "BATT_LEVEL",
    "Value": 93,
    "DataUpdatedAt": "2018-01-01T12:34:56Z"
    // ...
  },
  {
    "Name": "HEART_RATE",
    "Value": 125,
    "DataUpdatedAt": "2018-01-01T12:34:56Z"
    // ...
  },
  {
    "Name": "OXYGEN_LEVEL",
    "Value": 99,
    "DataUpdatedAt": "2018-01-01T12:34:56Z"
    // ...
  }
  // ...
]
```

## Using this Owlet API Wrapper

Install NuGet package

```bash
dotnet add package Unofficial.Owlet
```

Using .NET Core Dependency Injection

```cs
// ...
using Unofficial.Owlet.Extensions;

// ...

private void ConfigureServices(IServiceCollection services)
{
    // ...

    services.AddOwletApi();

    // ...
}
```

Then, you can import the specific API services you would like to consume into your class:

```cs
namespace My.App
{
    public class MyClass
    {
        private readonly IOwletUserApi _owletUserApi;
        private readonly IOwletDeviceApi _owletDeviceApi;
        public MyClass(IOwletUserApi owletUserApi, IOwletDeviceApi owletDeviceApi)
        {
            this._owletUserApi = owletUserApi;
            this._owletDeviceApi = owletDeviceApi;
        }

        public async Task Run()
        {
            var email = ""; // from IOptions<YourConfigClass> or input
            var password = ""; // from IOptions<YourConfigClass> or input
            await Login(email, password);

            var mySock = await GetDeviceSerialNumber(); // get first sock in your account
            var mySockProperties = await GetDeviceProperties(mySock.Device.DeviceSerialNumber);

            foreach (var property in mySockProperties)
            {
                Console.WriteLine($"{property.Property.Name} : {property.Property.Value} ({property.Property.DataUpdatedAt?.ToString("MM/dd/yyyy HH:mm")})");
            }
        }

        public async Task<OwletSignInResponse> Login(string email, string password)
        {
            return await this._owletUserApi.LoginAsync(email, password);
        }

        public async Task<string> GetDeviceSerialNumber()
        {
            return await this._owletDeviceApi.GetDevicesAsync()
                .First()?.Device?.DeviceSerialNumber;
        }

        public async Task<IEnumerable<OwletDevicePropertyResponse>> GetDeviceProperties(string deviceSerialNumber)
        {
            return await this._owletDeviceApi.TryGetFreshDevicePropertiesAsync(deviceSerialNumber);
        }
    }
}
```

## Setting Up Dependency Injection with User Settings

You may want to configure your account email/password in your startup method, then forget about it later. Or you may want to always use your current sock's device serial number (if you know it - or find it using this library), so that you can reduce an additional API call to speed up retrieval of your Owlet Smart Sock data.

```cs
using Unofficial.Owlet.Extensions;
using Unofficial.Owlet.Models;

// ...

private void ConfigureServices(IServiceCollection services)
{
    services.AddOwletApi(options =>
    {
        options.WithSettings(new OwletApiSettings
        {
            Email = "abc@abc.com",
            Password = "mypassword",
            KnownDeviceSerialNumbers = new [] { "A...." }
        });
    });

    // ...
}
```

Then, if no `accessToken` is provided to some of the API methods, it will check for this configuration value and log you in as needed automatically. No more calls to `LoginAsync(email, password)`!

```cs
namespace My.App
{
    public class MyClass
    {
        // ...

        public async Task Run()
        {
            // no need to make explicit LoginAsync() call!
            // this Owlet API library will try for you!

            var mySock = await GetDeviceSerialNumber(); // get first sock in your account
            var mySockProperties = await GetDeviceProperties(mySock.Device.DeviceSerialNumber);

            foreach (var property in mySockProperties)
            {
                Console.WriteLine($"{property.Property.Name} : {property.Property.Value} ({property.Property.DataUpdatedAt?.ToString("MM/dd/yyyy HH:mm")})");
            }
        }

        // ...
    }
}
```

## Managing User Secrets

This is not required to consume this library, but a note nonetheless.

If you use the `IOptions<T>` pattern to read configuration from `appsettings.json`, you may want to consider storing your Email/Password in a `secrets.json` file stored on your client machine outside of this (or your own) code repository - which can accidentally leak to the public!

```
# recommend .NET Core 2.1+ SDK

dotnet tool install -g dotnet-user-secrets

cd $ProjectRoot
cd src/Unofficial.Owlet.TestConsole/
dotnet-user-secrets list
dotnet-user-secrets set owlet:email abc@abc.com
dotnet-user-secrets set owlet:password mypassword
```

## Credits

Many thanks go out to Owlet Baby Care for giving us piece of mind.

Secondly, thanks to

- @puco
- @bobcat0070
- @arosequist

for helping me get started working with Ayla Networks IoT platform to read Owlet Smart Sock data!

## How Does It Work?

I touched on this briefly above, but it's worth calling out again.

Other collaborators have identified that it's easy enough to request device properties from the Ayla Networks IoT platform, but the data ends up getting stale after `~30 seconds`, or after the Owlet app has closed.

As it turns out, there's a property returned named `APP_ACTIVE` that reads as `1` when the mobile application is in-use, and `0` when not.

If we want to fake that the app is always open so that we can collect data over a time period, we need to continually write a new `APP_ACTIVE` datapoint with value of `1` such that the Owlet base station will broadcast the other assorted sock datapoints. Of course, there is a time delay between the initial mark of `APP_ACTIVE` to when the base station is able to create new datapoint records. So, this library takes a swing at `2 seconds` ... even though realistically, that's likely not sufficient for most experiences (2 seconds between notifying Owlet API that it's active, having data requested from the base station, and the base station replying with datapoints).

I mentioned the risks above earlier, too. You can see that this library is making the assumption that there is a property named `APP_ACTIVE` and that its value needs to be `1` in order for data to update. If this property ever gets renamed, or the value it needs to be is ever updated, or an alternate mechanism is introduced, this library will need to be updated for that.

### Data Flow

1.  Login (`/users/sign_in`)
1.  Get Devices for Account (`/apiv1/devices`)
1.  Get Device Properties (`/apiv1/dsns/{deviceSerialNumber}/properties`)
    - Are properties stale?
      1.  Create new datapoint marking app as "active" (`/apiv1/properties/{propertyId}/datapoints`)
      1.  Wait a few seconds for data to propagate
      1.  Query for Device Properties again (repeat)

---

&copy; 2018 Mitchell Barry
