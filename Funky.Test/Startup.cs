using Funky.Auth;
using Funky.Extensions.DependencyInjection;
using Funky.Filters;
using Funky.Filters.ActionFilters;
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
            builder.Services.AddB2CDiscoveryCache(new AuthConfig
            {
                Authority = "https://wallaridedev.b2clogin.com/75488c85-e9ad-4aa4-9051-8ac245355c69/v2.0/",
                Audience = "d6d23883-698f-4906-aa9b-03f8a8bd7403",
                DiscoveryUrl = "https://wallaridedev.b2clogin.com/75488c85-e9ad-4aa4-9051-8ac245355c69/v2.0/.well-known/openid-configuration?p=B2C_1_login2"
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
