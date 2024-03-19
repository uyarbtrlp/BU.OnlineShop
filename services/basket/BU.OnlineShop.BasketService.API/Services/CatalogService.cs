using BU.OnlineShop.BasketService.API.Dtos.CatalogService;
using BU.OnlineShop.Shared.Exceptions;
using BU.OnlineShop.Shared.Extensions;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;

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
                Address = _configuration["AuthServer:Authority"],
                Policy =
                {
                    RequireHttps = Convert.ToBoolean(_configuration["AuthServer:RequireHttpsMetadata"]),
                }
            });
            if (discoveryDocumentResponse.IsError)
            {
                throw new Exception(discoveryDocumentResponse.Error);
            }

            var currentToken = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");

            var customParams = new Dictionary<string, string>
            {
                { "subject_token_type","urn:ietf:params:oauth:token-type:access_token"},
                { "subject_token",currentToken},
                { "scope","openid profile catalogservice.fullaccess"}
            };

            var tokenResponse = await _httpClient.RequestTokenAsync(new TokenRequest()
            {
                Address = discoveryDocumentResponse.TokenEndpoint,
                GrantType = "urn:ietf:params:oauth:grant-type:token-exchange",
                Parameters = new Parameters(customParams),
                ClientId = "BasketServiceTokenExchangeClient",
                ClientSecret = "1q2w3e*"
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
            return await response.ReadContentAs<ProductDto>();
        }
    }
}
