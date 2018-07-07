using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Unofficial.Owlet.Extensions;
using Unofficial.Owlet.Models;
using Unofficial.Owlet.TestConsole.Models;

namespace Unofficial.Owlet.TestConsole
{
    public class Startup
    {
        public static IConfiguration Configuration { get; set; }

        public static void Main(string[] args)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var launch = Environment.GetEnvironmentVariable("LAUNCH_PROFILE");

            if (string.IsNullOrWhiteSpace(env))
            {
                env = "Development";
            }

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env}.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            if (env == "Development")
            {
                builder.AddUserSecrets<Startup>();
            }

            Configuration = builder.Build();

            // Create a service collection and configure our depdencies
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            // Build the our IServiceProvider and set our static reference to it
            ServiceProviderFactory.ServiceProvider = serviceCollection.BuildServiceProvider();

            // Enter the application (Main starting point for app!)
            ServiceProviderFactory.ServiceProvider.GetService<Application>().Run().Wait();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            // Make configuration settings available
            services.AddSingleton<IConfiguration>(Configuration);

            var owletConfigurationSection = Configuration.GetSection("owlet");

            services.AddOptions();
            services.Configure<MyAppConfiguration>(owletConfigurationSection);

            services.AddOwletApi(options =>
            {
                var serviceProvider = services.BuildServiceProvider();
                var myAppConfig = serviceProvider.GetService<IOptions<MyAppConfiguration>>().Value;
                options.WithSettings(new OwletApiSettings
                {
                    Email = myAppConfig.Email,
                    Password = myAppConfig.Password,
                    KnownDeviceSerialNumbers = myAppConfig.KnownDeviceSerialNumbers
                });
            });

            // Add console application main class 
            services.AddTransient<Application>();
        }
    }

    public static class ServiceProviderFactory
    {
        public static IServiceProvider ServiceProvider { get; set; }
    }
}
