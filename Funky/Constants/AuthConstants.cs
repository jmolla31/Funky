using System;
using System.Collections.Generic;
using System.Text;

namespace Funky.Filters.Constants
{
    public static class AuthConstants
    {
        public const string InvalidToken = "InvalidToken";

        public const string IsAuthenticated = "IsAuthenticated";

        public const string AnonymousUser = "AnonymousUser";

        public const string Authorization = "Authorization";

        public const string ClientSecretAuthClaim = "azpacr";

        public const string AzpacrClientSecret = "1";
        
        public const string AzpacrCertificate = "2";

        public const string B2CIdp = "idp";
    }
}
