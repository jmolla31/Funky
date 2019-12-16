using Funky.Filters.Constants;
using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace Funky.Filters.Auth
{

    public sealed class JwtValidator : FunctionInvocationFilterAttribute
    {
        public override async Task OnExecutingAsync(FunctionExecutingContext executingContext, CancellationToken cancellationToken)
        {
            if (!(executingContext.Arguments.FirstOrDefault().Value is DefaultHttpRequest @request)) return;

            var currentContext = request.HttpContext;

            if (!(currentContext.RequestServices.GetService(typeof(AuthConfig)) is AuthConfig authConfig)) 
                throw new ArgumentNullException("Couldn't load AuthConfig class form dependency injection.");

            if (!(currentContext.RequestServices.GetService(typeof(DiscoveryCache)) is DiscoveryCache discoveryCache))
                throw new ArgumentNullException("Couldn't load DiscoveryCache class form dependency injection.");

            var jwtHeader = currentContext.Request.Headers.FirstOrDefault(x => x.Key == AuthConstants.Authorization);

            if (jwtHeader.Key == null)
            {
                currentContext.Items.Add(AuthConstants.AnonymousUser, AuthConstants.AnonymousUser);
                return;
            }

            var validationParams = await this.GetTokenValidationParameters(authConfig, discoveryCache);

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            var tokenString = jwtHeader.Value.ToString().Substring("Bearer ".Length).Trim();

            try
            {
                var claims = handler.ValidateToken(tokenString, validationParams, out var validatedToken);

                currentContext.User = claims;
                currentContext.Items.Add(AuthConstants.IsAuthenticated, AuthConstants.IsAuthenticated);
            }
            catch (Exception)
            {
                currentContext.Items.Add(AuthConstants.InvalidToken, AuthConstants.InvalidToken);
            }
        }

        private async Task<TokenValidationParameters> GetTokenValidationParameters(AuthConfig authConfig, DiscoveryCache discoveryCache)
        {
            var disco = await discoveryCache.GetAsync();

            var keys = new List<SecurityKey>();
            foreach (var webKey in disco.KeySet.Keys)
            {
                var e = Base64Url.Decode(webKey.E);
                var n = Base64Url.Decode(webKey.N);

                var key = new RsaSecurityKey(new RSAParameters { Exponent = e, Modulus = n })
                {
                    KeyId = webKey.Kid
                };

                keys.Add(key);
            }

            TokenValidationParameters validationParameters =
            new TokenValidationParameters
            {
                ValidIssuer = authConfig.Authority,
                ValidAudiences = new[] { authConfig.Audience },
                IssuerSigningKeys = keys,
                RequireSignedTokens = true
                // TODO:  Scopes
            };

            return validationParameters;
        }
    }
}
