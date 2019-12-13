using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Funky.Filters.ActionFilters
{
    public interface IActionFilter
    {
        /// <summary>
        /// This method gets called by the MainFilterExecutor when the filter is invoked to execute.
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <returns></returns>
        Task<bool> ExecuteFilter(IHttpContextAccessor httpContextAccessor);
    }
}
