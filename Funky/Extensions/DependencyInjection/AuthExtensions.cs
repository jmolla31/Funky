using Funky.Auth;
using Funky.Auth.B2C;
using IdentityModel.Client;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Funky.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers the provided AuthConfig as a singleton inside the service collecion
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IServiceCollection AddAuthConfig(this IServiceCollection services, AuthConfig config)
        {
            services.AddSingleton(config);

            return services;
        }

        public static IServiceCollection AddDiscoveryCache(this IServiceCollection services, AuthConfig config = null)
        {
            var authority = (services.AddSingleton(config).FirstOrDefault(x => x.ServiceType == typeof(AuthConfig)).ImplementationInstance as AuthConfig).Authority
                ?? (services.FirstOrDefault(x => x.ServiceType == typeof(AuthConfig)).ImplementationInstance as AuthConfig).Authority;

            if (!services.Any(x => x.ServiceType == typeof(IHttpClientFactory))) services.AddHttpClient();

            services.AddSingleton<IDiscoveryCache>(r =>
            {
                var factory = r.GetRequiredService<IHttpClientFactory>();
                return new DiscoveryCache(authority, () => factory.CreateClient());
            });

            return services;
        }

        public static IServiceCollection AddB2CDiscoveryCache(this IServiceCollection services, AuthConfig config = null)
        {
            var authority = (services.AddSingleton(config).FirstOrDefault(x => x.ServiceType == typeof(AuthConfig)).ImplementationInstance as AuthConfig).Authority
                ?? (services.FirstOrDefault(x => x.ServiceType == typeof(AuthConfig)).ImplementationInstance as AuthConfig).Authority;

            if (!services.Any(x => x.ServiceType == typeof(IHttpClientFactory))) services.AddHttpClient();

            services.AddSingleton<IB2CDiscoveryCache, B2CDiscoveryCache>();

            return services;
        }
    }
}
