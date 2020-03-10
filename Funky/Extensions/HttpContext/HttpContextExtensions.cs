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
        /// ONLY FOR AZURE AD, Returns if the token has been generated using a ClientId and ClientSecret.
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public static bool AuthorizedByClientSecret(this HttpContext ctx) => ctx.User.Claims.Any(x => x.Type == AuthConstants.ClientSecretAuthClaim && x.Value == AuthConstants.AzpacrClientSecret);

        /// <summary>
        /// ONLY FOR AZURE AD, Returns if the token has been generated using a Certificate.
        /// </summary>
        /// <param name="ctx">Http context</param>
        /// <returns></returns>
        public static bool AuthorizedByCertificate(this HttpContext ctx) => ctx.User.Claims.Any(x => x.Type == AuthConstants.ClientSecretAuthClaim && x.Value == AuthConstants.AzpacrCertificate);

        /// <summary>
        /// ONLY FOR AZURE B2C, returns the identity provider that has authorized the current user (ex: google, github, twitter)
        /// </summary>
        /// <param name="ctx">Http context</param>
        /// <returns></returns>
        public static string GetB2CTokenIdp(this HttpContext ctx) => ctx.User.Claims.FirstOrDefault(x => x.Type == AuthConstants.B2CIdpClaim).Value;

        /// <summary>
        /// Gets the current user unique "sub" or "nameidentifier" claim value
        /// </summary>
        /// <param name="ctx">Http context</param>
        /// <returns></returns>
        public static string GetUserIdentifier(this HttpContext ctx) => ctx.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;

        /// <summary>
        /// Gets the current user unique "oid" or "objectidentifier" claim value
        /// </summary>
        /// <param name="ctx">Http context</param>
        /// <returns></returns>
        public static string GetUserObjectIdentifier(this HttpContext ctx) => ctx.User.Claims.FirstOrDefault(x => x.Type == AuthConstants.ObjectIdentifierClaim).Value;

        /// <summary>
        /// ONLY FOR AZURE B2C, returns all the custom attributes (claims) present in the current token.
        /// </summary>
        /// <param name="ctx">Http context</param>
        /// <returns></returns>
        public static IEnumerable<Claim> GetB2CCustomAttributes(this HttpContext ctx) => ctx.User.FindAll(x => x.Type.StartsWith(AuthConstants.B2CCustomAttrPrefix));

        /// <summary>
        /// Gets the user email value (only retrieves the first value if multiple email claims)
        /// </summary>
        /// <param name="ctx">Http context</param>
        /// <returns></returns>
        public static string GetUserEmail(this HttpContext ctx) => ctx.User.FindFirst(AuthConstants.UserEmailsClaim).Value;

        /// <summary>
        /// Gets all the user email claim values
        /// </summary>
        /// <param name="ctx">Http context</param>
        /// <returns></returns>
        public static IEnumerable<string> GetUserEmailList(this HttpContext ctx) => ctx.User.FindAll(AuthConstants.UserEmailsClaim).Select(x => x.Value);

    }
}
