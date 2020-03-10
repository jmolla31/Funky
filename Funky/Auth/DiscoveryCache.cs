using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Funky.Auth
{
    public class DiscoveryCache : IDiscoveryCache
    {
        private const string DiscoverySuffix = ".well-known/openid-configuration";
        private readonly string DiscoveryUrl;

        private DateTime nextReload = DateTime.MinValue;

        private readonly IHttpClientFactory httpClientFactory;

        private OpenIdConnectConfiguration cachedResult;

        public TimeSpan CacheDuration { get; set; } = TimeSpan.FromHours(24);

        public DiscoveryCache(AuthConfig authConfig, IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;

            this.DiscoveryUrl = authConfig.Authority.EndsWith("/")
                ? authConfig.Authority + DiscoverySuffix
                : authConfig.Authority + "/" + DiscoverySuffix;
        }

        public async Task<OpenIdConnectConfiguration> GetAsync()
        {
            if (this.nextReload <= DateTime.UtcNow) await RefreshAsync();

            return cachedResult;
        }

        public async Task RefreshAsync()
        {
            this.cachedResult = await OpenIdConnectConfigurationRetriever.GetAsync(this.DiscoveryUrl, this.httpClientFactory.CreateClient(), new CancellationToken());

            this.nextReload = DateTime.UtcNow.Add(CacheDuration);
        }
    }
}