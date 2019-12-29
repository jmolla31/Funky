using IdentityModel.Client;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Threading.Tasks;

namespace Funky.Auth.B2C
{
    public interface IB2CDiscoveryCache
    {
        TimeSpan CacheDuration { get; set; }

        Task<OpenIdConnectConfiguration> GetAsync();
        Task RefreshAsync();
    }
}