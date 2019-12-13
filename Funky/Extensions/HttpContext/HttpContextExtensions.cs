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
        /// <summary>
        /// Returns if the provided token couldn't be validated by JwtValidator (malformed, expired, wrong signature...)
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public static bool IsJwtInvalid(this HttpContext ctx) => ctx.Items.ContainsKey(AuthConstants.InvalidToken);

        /// <summary>
        /// Returns the current user identity inside HttpContext
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public static IIdentity GetIdentity(this HttpContext ctx) => ctx.User.Identity;

        /// <summary>
        /// Returns if JwtValidator has determined that the user is anonymous (no auth header found in the request)
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public static bool IsUserAnonymous(this HttpContext ctx) => ctx.Items.ContainsKey(AuthConstants.AnonymousUser);

        /// <summary>
        /// ONLY FOR AZURE AD TOKENS, Returns if the token has been generated using a ClientId and ClientSecret.
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public static bool AuthorizedByClientSecret(this HttpContext ctx) => ctx.User.Claims.Any(x => x.Type == AuthConstants.ClientSecretAuthClaim && x.Value == AuthConstants.AzpacrClientSecret);

        /// <summary>
        /// ONLY FOR AZURE AD TOKENS, Returns if the token has been generated using a Certificate.
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public static bool AuthorizedByCertificate(this HttpContext ctx) => ctx.User.Claims.Any(x => x.Type == AuthConstants.ClientSecretAuthClaim && x.Value == AuthConstants.AzpacrCertificate);
    }
}
    