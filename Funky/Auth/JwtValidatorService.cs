using Funky.Filters.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;


namespace Funky.Auth
{
    public class JwtValidatorService : IJwtValidatorService
    {
        private readonly ILogger<JwtValidatorService> logger;

        private readonly IDiscoveryCache b2CDiscoveryCache;
        private readonly HttpContext currentContext;
        private readonly JwtSecurityTokenHandler handler;
        private readonly TokenValidationParameters tokenValidationParameters;

        private readonly int BearerPrefixLenght = AuthConstants.BearerPrefix.Length;

        public JwtValidatorService(AuthConfig authConfig, IDiscoveryCache b2CDiscoveryCache, ILogger<JwtValidatorService> logger, IHttpContextAccessor httpContextAccessor)
        {
            this.b2CDiscoveryCache = b2CDiscoveryCache;
            this.logger = logger;
            this.currentContext = httpContextAccessor.HttpContext;
            this.handler = new JwtSecurityTokenHandler();

            this.tokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = authConfig.Authority,
                ValidAudiences = new[] { authConfig.Audience },
                RequireSignedTokens = true
            };
        }

        /// <summary>
        /// Tries to find and validate a Jwt bearer token in the current request context
        /// </summary>
        /// <returns></returns>
        public async Task ValidateJwt()
        {
            this.logger.LogInformation($"{currentContext.TraceIdentifier} Validating Jwt Bearer token for request");

            var jwtHeader = currentContext.Request.Headers.FirstOrDefault(x => x.Key == AuthConstants.Authorization);

            if (jwtHeader.Key == null)
            {
                // The Functions host adds a identity even when set to Anonymous authentication so this is still the
                // source of truth to check if a user has been authenticated using a valid JWT.
                currentContext.Items.Add(AuthConstants.AnonymousUser, AuthConstants.AnonymousUser);
                return;
            }

            var disco = await this.b2CDiscoveryCache.GetAsync();

            this.logger.LogInformation($"{currentContext.TraceIdentifier} Downloaded discovery document");

            this.tokenValidationParameters.IssuerSigningKeys = disco.SigningKeys;

            var tokenString = jwtHeader.Value.ToString().Substring(this.BearerPrefixLenght).Trim();

            try
            {
                var claims = this.handler.ValidateToken(tokenString, this.tokenValidationParameters, out var validatedToken);

                this.logger.LogInformation($"{currentContext.TraceIdentifier} Token validated, user authenticated");

                currentContext.User = claims;

                // The Functions host adds a identity even when set to Anonymous authentication so this is still the
                // source of truth to check if a user has been authenticated using a valid JWT.
                currentContext.Items.Add(AuthConstants.IsAuthenticated, AuthConstants.IsAuthenticated);
            }
            catch (Exception e)
            {
                logger.LogInformation($"{currentContext.TraceIdentifier} Invalid token, reason: {e.Message}");

                // The Functions host adds a identity even when set to Anonymous authentication so this is still the
                // source of truth to check if a user has been authenticated using a valid JWT.
                currentContext.Items.Add(AuthConstants.InvalidToken, AuthConstants.InvalidToken);
            }
        }
    }
}
