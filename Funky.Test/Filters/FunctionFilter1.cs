using Funky.Filters.ActionFilters;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Funky
{
    public class FunctionFilter1 : IActionFilter
    {
        public async Task<bool> ExecuteFilter(IHttpContextAccessor httpContextAccessor)
        {
            return false;
        }
    }

    public class FunctionFilter2 : IActionFilter
    {
        public async  Task<bool> ExecuteFilter(IHttpContextAccessor httpContextAccessor)
        {
            return true;
        }
    }
}
