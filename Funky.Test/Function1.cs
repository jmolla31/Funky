using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Funky.Filters;
using Funky.Filters.ActionFilters;
using Funky.Filters.Extensions.HttpCtx;
using System.Diagnostics;
using Funky.Auth.B2C;

namespace Funky
{
    public class Function1
    {
        private readonly HttpContext httpContext;
        private readonly MainFilterExecutor mainFilterExecutor;

        public Function1(IHttpContextAccessor httpContextAccessor, MainFilterExecutor mainFilterExecutor)
        {
            this.httpContext = httpContextAccessor.HttpContext;
            this.mainFilterExecutor = mainFilterExecutor;
        }


        [FunctionName(nameof(JwtTest))]
        [JwtValidatorService]
        public async Task<IActionResult> JwtTest(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {

            this.httpContext.AuthorizedByClientSecret();
            /*
            var watch = Stopwatch.StartNew();
            await mainFilterExecutor.ExecuteMapped();

            await mainFilterExecutor.ExecuteAll();

            var results = mainFilterExecutor.GetFilterResults();

            watch.Stop();

            var ticks = watch.ElapsedTicks;
            */

            if (this.httpContext.IsUserAnonymous()) return new UnauthorizedResult();

            if (this.httpContext.IsJwtInvalid()) return new BadRequestResult();


            return new OkResult();
        }
    }
}
