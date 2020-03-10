using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Threading.Tasks;

namespace Funky.Auth
{
    public interface IDiscoveryCache
    {
        TimeSpan CacheDuration { get; set; }

        Task<OpenIdConnectConfiguration> GetAsync();
        Task RefreshAsync();
    }
}