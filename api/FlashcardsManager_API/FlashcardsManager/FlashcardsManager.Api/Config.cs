using System.Collections.Generic;
using System.Security.Claims;
using IdentityModel;
using IdentityServer4.Models;

namespace FlashcardsManager.Api
{
    public class Config
    {
        public static string HOST_URL = "https://flashcards-manager.azurewebsites.net";

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("flashcards")
                {
                    ApiSecrets =
                    {
                        new Secret("flashcardsSecret".Sha256())
                    },
                    Scopes =
                    {
                        new Scope
                        {
                            Name = "flashcardsScope",
                            DisplayName = "Scope for the flashcardsScope ApiResource",
                            UserClaims = new[] { JwtClaimTypes.Role, ClaimTypes.Role },

                        },
                    }

                }
            };
        }

        // clients want to access resources (aka scopes)
        public static IEnumerable<Client> GetClients()
        {
            // client credentials client
            return new List<Client>
            {
                new Client
                {
                    ClientName = "ApiClient",
                    ClientId = "apiClient",
                    ClientSecrets = { new Secret("flashcardsSecret") },
                    RequireClientSecret = false,
                    AccessTokenType = AccessTokenType.Jwt,
                    //AccessTokenLifetime = 600, // 10 minutes, default 60 minutes
                    AllowedGrantTypes = GrantTypes.Implicit,
                    RequireConsent = false,
                    AllowAccessTokensViaBrowser = false,
                    RedirectUris = new List<string>
                    {
                         HOST_URL

                    },
                    PostLogoutRedirectUris = new List<string>
                    {
                         HOST_URL
                    },
                    AllowedCorsOrigins = new List<string>
                    {
                         HOST_URL
                    },
                    AllowedScopes = new List<string>
                    {
                        "openid",
                        "flashcardsScope",
                    }
                },
                 new Client
                {
                    ClientName = "wpf",
                    ClientId = "wpf",
                    ClientSecrets = { new Secret("flashcardsSecret".Sha256()) },
                    AccessTokenType = AccessTokenType.Jwt,
                    //AccessTokenLifetime = 600, // 10 minutes, default 60 minutes
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    RequireConsent = false,
                    AllowAccessTokensViaBrowser = false,
                    RedirectUris = new List<string>
                    {
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
                         HOST_URL
                    },
                    AllowedCorsOrigins = new List<string>
                    {
                         HOST_URL
                    },
                    AllowedScopes = new List<string>
                    {
                        "openid",
                        "flashcardsScope",
                    }
                }
            };
        }
    }
}
