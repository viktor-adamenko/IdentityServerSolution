using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MvcClient.Services
{
    public class TokenService : ITokenService
    {
        private readonly IOptions<IdentityServerSettings> _settings;
        private readonly IHttpContextAccessor _contextAccessor;
        private DiscoveryDocumentResponse _discoveryDocument;

        public TokenService(IOptions<IdentityServerSettings> settings, IHttpContextAccessor contextAccessor)
        {
            _settings = settings;
            _contextAccessor = contextAccessor;
        }

        /// <summary>
        /// Метод для получения access token'a по client_id\secret которые прописаны в конф. файле в секции "ApiIdentityServerSettings"
        /// </summary>
        public async Task<string> GetToken(string scope = null)
        {
            var accessToken = await _contextAccessor.HttpContext.GetTokenAsync("access_token");

            if (string.IsNullOrEmpty(accessToken))
            {
                GetDiscoveryDocument();

                using (var client = new HttpClient())
                {
                    var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
                    {
                        Address = _discoveryDocument.TokenEndpoint,
                        ClientId = _settings.Value.ClientName,
                        ClientSecret = _settings.Value.ClientPassword,
                        Scope = scope
                    });

                    if (tokenResponse.IsError)
                    {
                        throw new Exception("Unable to get token", tokenResponse.Exception);
                    }

                    return tokenResponse.AccessToken;
                }
            }

            return accessToken;

        }

        private void GetDiscoveryDocument()
        {
            using (var httpClient = new HttpClient())
            {
                _discoveryDocument = httpClient.GetDiscoveryDocumentAsync(_settings.Value.DiscoveryUrl).Result;
                if (_discoveryDocument.IsError)
                {
                    throw new Exception("Unable to get discovery document", _discoveryDocument.Exception);
                }
            }
        }
    }
}
