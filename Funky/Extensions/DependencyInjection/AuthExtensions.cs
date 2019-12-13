using Funky.Filters.Auth;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Funky.Filters.Extensions.DependencyInjection
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
    }
}
