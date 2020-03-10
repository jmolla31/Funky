using Funky.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Net.Http;

namespace Funky.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds all Jwt validation services (AuthConfig, DiscoveryCache, and JwtValidatorService) and support services if not present to the service collection
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IServiceCollection AddJwtValidator(this IServiceCollection services, AuthConfig config)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));

            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddSingleton(config);

            if (!services.Any(x => x.ServiceType == typeof(IHttpClientFactory))) services.AddHttpClient();
            if (!services.Any(x => x.ServiceType == typeof(IHttpContextAccessor))) services.AddHttpContextAccessor();

            services.AddSingleton<IDiscoveryCache, DiscoveryCache>();

            services.AddSingleton<IJwtValidatorService, JwtValidatorService>();

            return services;
        }

        /// <summary>
        /// Adds the AuthConfig instance into the services collection as a singleton.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IServiceCollection AddAuthConfig(this IServiceCollection services, AuthConfig config)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));

            if (!services.Any(x => x.ServiceType == typeof(AuthConfig))) services.AddSingleton(config);

            return services;
        }
    }
}
