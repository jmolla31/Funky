using Funky.Filters.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Azure.WebJobs.Host;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Funky.Filters.ActionFilters
{
    public class MainFilterExecutor
    {
        private readonly IEnumerable<IActionFilter> filters;
        private readonly IHttpContextAccessor httpContextAccesor;
        private readonly FilterMapper mapper;

        public MainFilterExecutor(IEnumerable<IActionFilter> filters, IHttpContextAccessor httpContextAccesor, FilterMapper mapper)
        {
            this.filters = filters;
            this.httpContextAccesor = httpContextAccesor;
            this.mapper = mapper;
        }

        /// <summary>
        /// Retrieves all the filter result values in the current HttpContext
        /// </summary>
        /// <returns></returns>
        public IEnumerable<KeyValuePair<string,bool>> GetFilterResults()
        {
            var results = this.httpContextAccesor.HttpContext.Items
                .Where(x => x.Key.ToString().StartsWith(FilterConstants.ExecutionResult))
                .Select( x => new KeyValuePair<string,bool>(x.Key.ToString(), bool.Parse(x.Value.ToString())));

            return results;
        }

        /// <summary>
        /// Execute all avialiable filters
        /// </summary>
        /// <returns></returns>
        public async Task<bool> ExecuteAll()
        {
            var result = true;

            foreach (var filter in this.filters)
            {   
                var executionResult = await filter.ExecuteFilter(httpContextAccesor);

                httpContextAccesor.HttpContext.Items.Add($"{FilterConstants.ExecutionResult}{filter.GetType().Name}", executionResult);

                if (executionResult == false) result = false;
            }

            return result;
        }

        /// <summary>
        /// Execute all avialiable filters mapped to the callerName, defaults to the caller method name
        /// </summary>
        /// <param name="callerName"></param>
        /// <returns></returns>
        public async Task<bool> ExecuteMapped([CallerMemberName] string callerName = "")
        {
            if (!this.filters.Any()) return false;

            var mappedTypes = this.mapper.GetFilters(callerName);

            var result = true;

            foreach (var @type in mappedTypes)
            {
                var executionResult = await this.filters.FirstOrDefault(x => x.GetType() == @type).ExecuteFilter(httpContextAccesor);

                httpContextAccesor.HttpContext.Items.Add($"{FilterConstants.ExecutionResult}{@type.Name}", executionResult);

                if (executionResult == false) result = false;
            }

            return result;
        }
    }
}
