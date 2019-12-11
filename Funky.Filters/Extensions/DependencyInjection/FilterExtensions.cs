using Funky.Filters.ActionFilters;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Funky.Filters.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFilterExecutor(this IServiceCollection services)
        {
            services.AddSingleton(new FilterMapper());

            services.AddScoped(typeof(MainFilterExecutor), x => 
            {
                return new MainFilterExecutor
                (
                    x.GetServices<IActionFilter>(),
                    x.GetRequiredService<IHttpContextAccessor>(),
                    x.GetRequiredService<FilterMapper>()
                );
            });

            return services;
        }

        public static IServiceCollection RegisterFilter(this IServiceCollection services, Type filter)
        {
            services.AddScoped(typeof(IActionFilter), filter);

            return services;
        }

        public static IServiceCollection RegisterFilters(this IServiceCollection services, Type filterPointer)
        {
            var mainExecutor = services.Where(x => x.ServiceType == typeof(MainFilterExecutor)).FirstOrDefault();

            if (mainExecutor == null) throw new ArgumentNullException(nameof(MainFilterExecutor));

            var interfaceType = typeof(IActionFilter);

            var servicesAssembly = filterPointer.Assembly;

            var locatedServices = servicesAssembly.ExportedTypes.Where(t => interfaceType.IsAssignableFrom(t) && !t.IsAbstract);

            foreach (var service in locatedServices)
            {
                services.AddSingleton(typeof(IActionFilter), service);
            }

            return services;
        }

        public static IServiceCollection MapFilter<TClass,TFilter>(this IServiceCollection services)
        {
            var mapper = services.FirstOrDefault(x => x.ServiceType == typeof(FilterMapper));

            if (mapper == null) throw new ArgumentNullException(nameof(MainFilterExecutor));

            (mapper.ImplementationInstance as FilterMapper).MapFilter<TClass,TFilter>();

            return services;
        }

        public static IServiceCollection MapFilter<TFilter>(this IServiceCollection services, string actionName) 
        {
            var mapper = services.FirstOrDefault(x => x.ServiceType == typeof(FilterMapper));

            if (mapper == null) throw new ArgumentNullException(nameof(MainFilterExecutor));

            (mapper.ImplementationInstance as FilterMapper).MapFilter<TFilter>(actionName);

            return services;
        }
    }
}
