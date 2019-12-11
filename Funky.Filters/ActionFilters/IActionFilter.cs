using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Funky.Filters.ActionFilters
{
    public interface IActionFilter
    {
        Task<bool> ExecuteFilter(IHttpContextAccessor httpContextAccessor);
    }
}
