using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Common.Constants
{
    public class Configuration
    {
        public const string KEY = "Pr377Y0n143In$163*U6LI0nt430UI$1032020";
        public const int TIMEOUT = 30;
        public class Tokens
        {
            public const string NAME = "Authentication API";
            public const string ISSUER = "Auth.Security.Bearer";
            public const string AUDIENCE = "Auth.Security.Bearer";

            public const int EXPIRES_IN_MINUTES = 30;

            public class Refresh
            {
                public const int EXPIRES_IN_DAYS = 14;
            }

            public class Policy
            {
                public const string NAME = "Profile";
            }

            public class Claims
            {
                public const string NAME = "ProfileId";
                public const string TYPE = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
            }
        }

    }
}
