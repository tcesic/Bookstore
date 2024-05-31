using IdentityServer4.Models;
using IdentityServer4;
using IdentityServer4.Validation;
using static System.Net.WebRequestMethods;

namespace BookStore
{
    public static class IdentityConfig
    {
        //Better than appsettings.json, but for production some of Secrets Manager Services like AWS Secret Manager or Azure Key Vault,..
        public const string SecretKey = "vPH5TqC8bGfyQh2LM7D5t1F6yE8nC6QrJ6zW/7J2G5U=,";

        public const string RedirectUri = "https://localhost:7037/swagger/oauth2-redirect.html";
        public const string PostLogoutRedirectUri = "https://localhost:7037/swagger/index.html";
        public const string Url = "https://localhost:7037";
        public const string AutorizationUrl = "https://localhost:7037/connect/authorize";
        public const string TokenConnectUrl = "https://localhost:7037/connect/token";
        public const string ImplicitClient = "implicit_client_id";
        public const string ClientCredentialsClient = "book_store_client_id";

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
        {
           // Client for implicit flow
            new Client
            {
                ClientId = ImplicitClient,
                AllowedGrantTypes = GrantTypes.Implicit,
                RequireClientSecret = false,
                RequireConsent = false,
                AllowAccessTokensViaBrowser = true,
                RedirectUris = { RedirectUri },
                PostLogoutRedirectUris = { PostLogoutRedirectUri},
                AllowedScopes = { "api1" }
            },
            
            // Client for client credentials flow
            new Client
            {
                ClientId = ClientCredentialsClient,
                ClientName = "Book Store Credentials Client",
                ClientSecrets = { new Secret(SecretKey.Sha256()) },
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = { "api2" }
            }
        };
        }

        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
            {
                new ApiScope("api1", "My API"),
                new ApiScope("api2", "My Second API"),
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
        {
             new ApiResource("api1", "My API")
            {
                Scopes = { "api1" }
            },
            new ApiResource("api2", "My Second API")
            {
                Scopes = { "api2" }
            }
        };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };
        }
    }

}
