using Funky.Filters.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Azure.WebJobs.Host;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Funky.Filters.Extensions.HttpCtx
{
    public static class HttpContextExtensions
    {
        public static bool IsJwtInvalid(this HttpContext ctx) => ctx.Items.ContainsKey(AuthConstants.InvalidToken);

        public static IIdentity GetIdentity(this HttpContext ctx) => ctx.User.Identity;

        public static bool IsUserAnonymous(this HttpContext ctx) => ctx.Items.ContainsKey(AuthConstants.AnonymousUser);

        public static bool AuthorizedByClientSecret(this HttpContext ctx) => ctx.User.Claims.Any(x => x.Type == AuthConstants.ClientSecretAuthClaim && x.Value == AuthConstants.AzpacrClientSecret);

        public static bool AuthorizedByCertificate(this HttpContext ctx) => ctx.User.Claims.Any(x => x.Type == AuthConstants.ClientSecretAuthClaim && x.Value == AuthConstants.AzpacrCertificate);
    }
}
    