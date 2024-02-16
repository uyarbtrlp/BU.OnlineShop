// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace BU.OnlineShop.Identity
{
    public static class Config
    {
        public static IConfiguration Configuration { get; private set; }

        public static void SetConfiguration(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource("test",new List<string>()
                {
                    "test"
                })
            };
        public static IEnumerable<ApiResource> ApiResources =>
            new ApiResource[]
            {
                new ApiResource("catalogservice", "Catalog Service APIs")
                {
                    Scopes = { "catalogservice.fullaccess" },
                    UserClaims = new[]
                    {
                        "email",
                        "email_verified",
                        "name",
                        "test"
                    }

                },
                new ApiResource("basketservice", "Basket Service APIs")
                {
                    Scopes = { "basketservice.fullaccess" }
                }
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("catalogservice.fullaccess"),
                new ApiScope("basketservice.fullaccess")
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientName = "OnlineShop Swagger Client",
                    ClientId = "OnlineShop_Swagger",
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris = {
                        $"{Configuration["IdentityServer:Clients:OnlineShopSwagger:BaseUrl"]}/swagger/oauth2-redirect.html",
                        $"{Configuration["IdentityServer:Resources:CatalogService:BaseUrl"]}/swagger/oauth2-redirect.html",
                        $"{Configuration["IdentityServer:Resources:BasketService:BaseUrl"]}/swagger/oauth2-redirect.html",
                    },
                    AllowedCorsOrigins = {
                        Configuration["IdentityServer:Clients:OnlineShopSwagger:BaseUrl"],
                        Configuration["IdentityServer:Resources:CatalogService:BaseUrl"],
                        Configuration["IdentityServer:Resources:BasketService:BaseUrl"],
                    },
                    AllowedScopes = { "openid", "profile", "email", "catalogservice.fullaccess", "basketservice.fullaccess"},
                    RequireConsent = false,
                    RequireClientSecret = false
                },
                new Client
                {
                    ClientId = "BasketServiceTokenExchangeClient",
                    ClientName = "Basket Service Token Exchange Client",
                    AllowedGrantTypes = new[] { "urn:ietf:params:oauth:grant-type:token-exchange" },
                    ClientSecrets = { new Secret("1q2w3e*".Sha256()) },
                    AllowedScopes = {
                         "openid", "profile", "catalogservice.fullaccess" }
                },
            };
    }
}