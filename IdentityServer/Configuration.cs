using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace IdentityServer
{
    public static class Configuration
    {

        public static List<TestUser> Users
        {
            get
            {
                var address = new
                {
                    street_address = "One Hacker Way",
                    locality = "Heidelberg",
                    postal_code = 69118,
                    country = "Germany"
                };

                return new List<TestUser>
                {
                  new TestUser
                  {
                    SubjectId = "818727",
                    Username = "alice",
                    Password = "alice",
                    Claims =
                    {
                      new Claim(JwtClaimTypes.Name, "Alice Smith"),
                      new Claim(JwtClaimTypes.GivenName, "Alice"),
                      new Claim(JwtClaimTypes.FamilyName, "Smith"),
                      new Claim(JwtClaimTypes.Email, "AliceSmith@email.com"),
                      new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                      new Claim(JwtClaimTypes.Role, "admin"),
                      new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                      new Claim(JwtClaimTypes.Address, JsonSerializer.Serialize(address),
                        IdentityServerConstants.ClaimValueTypes.Json)
                    }
                  },
                  new TestUser
                  {
                    SubjectId = "88421113",
                    Username = "bob",
                    Password = "bob",
                    Claims =
                    {
                      new Claim(JwtClaimTypes.Name, "Bob Smith"),
                      new Claim(JwtClaimTypes.GivenName, "Bob"),
                      new Claim(JwtClaimTypes.FamilyName, "Smith"),
                      new Claim(JwtClaimTypes.Email, "BobSmith@email.com"),
                      new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                      new Claim(JwtClaimTypes.Role, "user"),
                      new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
                      new Claim(JwtClaimTypes.Address, JsonSerializer.Serialize(address),
                        IdentityServerConstants.ClaimValueTypes.Json)
                    }
                  }
                };
            }
        }

        public static IEnumerable<IdentityResource> GetIdentityResources() =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource
                {
                    Name = "role",
                    UserClaims = { "role" }
                }
            };

        public static IEnumerable<ApiScope> GetApiScopes() =>
            new List<ApiScope> {
                new ApiScope("identityserver")
            };

        public static IEnumerable<ApiResource> GetApiResources() =>
            new List<ApiResource>
            {
                new ApiResource("PharmacyApi")
                {
                    Scopes = { "identityserver" },
                    UserClaims = { "role" }
                },

            };

        public static IEnumerable<Client> GetClients() =>
            new List<Client>
            {
                //new Client
                //{
                //    ClientId = "client_id",
                //    ClientSecrets = { new Secret("client_secret".ToSha256()) },

                //    AllowedGrantTypes = GrantTypes.ClientCredentials,
                //    AllowedScopes = { "ApiOne" }
                //},

                new Client
                {
                    ClientId = "client_id_mvc",
                    ClientSecrets = { new Secret("client_secret_mvc".ToSha256()) },

                    RedirectUris = { "https://localhost:44392/signin-oidc" },
                    PostLogoutRedirectUris = { "https://localhost:44392/Home" },

                    AllowedGrantTypes = GrantTypes.Code,
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "identityserver",
                        "role"
                    },
                    RequireConsent = false,
                    AlwaysIncludeUserClaimsInIdToken = true
                },

                new Client
                {
                    ClientId = "client_id_pharmacy_api",
                    ClientSecrets = { new Secret("client_id_pharmacy_api_secret".ToSha256()) },

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = {
                        "identityserver"
                    }
                }
            };
    }
}
