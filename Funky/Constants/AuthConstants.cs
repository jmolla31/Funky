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

        public const string BearerPrefix = "Bearer ";

        public const string ClientSecretAuthClaim = "azpacr";

        public const string AzpacrClientSecret = "1";
        
        public const string AzpacrCertificate = "2";

        /// <summary>
        /// Yeah, the validator "normalizes" certain claims...
        /// </summary>
        public const string B2CIdpClaim = "http://schemas.microsoft.com/identity/claims/identityprovider";

        /// <summary>
        /// Yeah, the validator "normalizes" certain claims...
        /// </summary>
        public const string ObjectIdentifierClaim = "http://schemas.microsoft.com/identity/claims/objectidentifier";

        public const string B2CCustomAttrPrefix = "extension_";

        public const string UserEmailsClaim = "emails";
    }
}
