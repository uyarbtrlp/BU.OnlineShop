using BU.OnlineShop.BasketService.API.Dtos.CatalogService;
using BU.OnlineShop.Shared.Exceptions;
using BU.OnlineShop.Shared.Extensions;
using IdentityModel.Client;
using Keycloak.AuthServices.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Net;

namespace BU.OnlineShop.BasketService.API.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly HttpClient _httpClient;
        private string _accessToken;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CatalogService(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        private async Task<string> GetToken()
        {
            if (!string.IsNullOrEmpty(_accessToken))
            {
                return _accessToken;
            }

            var discoveryDocumentResponse = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _configuration["BasketServiceTokenExchange:Authority"],
                Policy =
                {
                    RequireHttps = Convert.ToBoolean(_configuration["BasketServiceTokenExchange:RequireHttpsMetadata"]),
                    ValidateIssuerName = false, //TODO: Keycloak does not have configurable issuer name so I am not able to resolve it from container name. When it is done, this line should be removed to make it more secure or I can use IAuthorityValidationStrategy!!!
                    ValidateEndpoints = false
                }
            });

            if (discoveryDocumentResponse.IsError)
            {
                throw new Exception(discoveryDocumentResponse.Error);
            }

            var currentToken = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");

            var customParams = new Dictionary<string, string>
            {
                { "requested_token_type","urn:ietf:params:oauth:token-type:access_token"},
                { "subject_token",currentToken},
                { "audience","BasketServiceTokenExchangeClient"},
                { "scope","CatalogService" }
            };

            var tokenResponse = await _httpClient.RequestTokenAsync(new TokenRequest()
            {
                Address = discoveryDocumentResponse.TokenEndpoint,
                GrantType = "urn:ietf:params:oauth:grant-type:token-exchange",
                Parameters = new Parameters(customParams),
                ClientId = _configuration["Keycloak:Resource"],
                ClientSecret = _configuration["Keycloak:Credentials:Secret"],
            });

            if (tokenResponse.IsError)
            {
                throw new Exception(tokenResponse.Error);
            }

            _accessToken = tokenResponse.AccessToken;

            return _accessToken;


        }

        public async Task<ProductDto> GetAsync(Guid id)
        {
            _httpClient.SetBearerToken(await GetToken());
            var response = await _httpClient.GetAsync($"/api/catalog-service/products/{id}");

            if (!response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.NotFound)
            {
                // If requested product is not found, carry exception message. Otherwise continue with global exception handling.
                var errorContent = await response.Content.ReadFromJsonAsync<ExceptionBase>();
                throw errorContent;
            }

            return await response.ReadContentAs<ProductDto>();
        }
    }
}
