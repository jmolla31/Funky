using IdentityModel.Client;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Funky.Auth.B2C
{
    public class B2CDiscoveryCache : IB2CDiscoveryCache
    {
        private DateTime nextReload = DateTime.MinValue;

        private readonly IHttpClientFactory httpClientFactory;
        private readonly AuthConfig authConfig;

        private OpenIdConnectConfiguration cachedResult;

        public TimeSpan CacheDuration { get; set; } = TimeSpan.FromHours(24);

        public B2CDiscoveryCache(AuthConfig authConfig, IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
            this.authConfig = authConfig;
        }

        public async Task<OpenIdConnectConfiguration> GetAsync()
        {
            if (this.nextReload <= DateTime.UtcNow) await RefreshAsync();

            return cachedResult;
        }

        public async Task RefreshAsync()
        {
            this.cachedResult = await OpenIdConnectConfigurationRetriever.GetAsync(this.authConfig.DiscoveryUrl, this.httpClientFactory.CreateClient(), new CancellationToken());

            this.nextReload = DateTime.UtcNow.Add(CacheDuration);
        }
    }
}