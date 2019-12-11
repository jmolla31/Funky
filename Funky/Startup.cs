using Funky.Filters;
using Funky.Filters.ActionFilters;
using Funky.Filters.Auth;
using Funky.Filters.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;


[assembly: FunctionsStartup(typeof(Funky.Startup))]
namespace Funky
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton(new AuthConfig
            {
                Authority = "...",
                Audience = "..."
            });

            builder.Services
                .AddFilterExecutor()
                .RegisterFilters(typeof(Startup))
                .MapFilter<Function1,FunctionFilter1>()
                .MapFilter<Function1,FunctionFilter2>()
                .MapFilter<FunctionFilter1>(nameof(Function1.JwtTest));
        }
    }
}
