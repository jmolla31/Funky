using Funky.Filters.Auth;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Funky.Filters.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAuthConfig(this IServiceCollection services, AuthConfig config)
        {
            services.AddSingleton(config);

            return services;
        }
    }
}
