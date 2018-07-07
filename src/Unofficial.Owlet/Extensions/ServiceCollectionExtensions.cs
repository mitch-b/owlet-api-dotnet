using System;
using Microsoft.Extensions.DependencyInjection;
using Unofficial.Owlet.EndpointClients;
using Unofficial.Owlet.Interfaces;
using Unofficial.Owlet.Models;
using Unofficial.Owlet.Services;
using Polly;

namespace Unofficial.Owlet.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Configure Owlet API services
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddOwletApi(this IServiceCollection services)
        {
            return ConfigureOwletApiServiceDefaults(services);
        }

        /// <summary>
        /// Configure Owlet API services with specified options
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IServiceCollection AddOwletApi(this IServiceCollection services,
            Action<IOwletApiBuilder> options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            ConfigureOwletApiServiceDefaults(services);

            options(new OwletApiBuilder(services));

            return services;
        }

        private static IServiceCollection ConfigureOwletApiServiceDefaults(IServiceCollection services)
        {
            services.AddOptions();

            // NOTE: singleton, only single-account login supported
            services.AddSingleton<OwletUserSession>();

            // Only need one instance of settings...
            services.AddSingleton<IOwletApiSettings, OwletApiSettings>();

            services.AddHttpClient<AylaUserServiceClient>()
                .AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(new[]
                {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(5),
                    TimeSpan.FromSeconds(10)
                }));
            services.AddHttpClient<AylaDeviceServiceClient>()
                .AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(new[]
                {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(5),
                    TimeSpan.FromSeconds(10)
                }));
            services.AddScoped<IOwletUserApi, OwletUserApi>();
            services.AddScoped<IOwletDeviceApi, OwletDeviceApi>();

            return services;
        }
    }
}
