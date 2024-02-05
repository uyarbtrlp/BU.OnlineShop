﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
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
            };
        public static IEnumerable<ApiResource> ApiResources =>
            new ApiResource[]
            {
                new ApiResource("catalogservice", "Catalog Service APIs")
                {
                    Scopes = { "catalogservice.fullaccess" }
                },
                new ApiResource("basketservice", "Basket Service APIs")
                {
                    Scopes = { "basketservice.fullaccess" }
                },
                    new ApiResource("onlineshopwebgateway", "OnlineShop Web Gateway")
                {
                    Scopes = { "onlineshopwebgateway.fullaccess" }
                }
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("catalogservice.fullaccess"),
                new ApiScope("basketservice.fullaccess"),
                new ApiScope("onlineshopwebgateway.fullaccess")
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
                    },
                    AllowedCorsOrigins = {
                        Configuration["IdentityServer:Clients:OnlineShopSwagger:BaseUrl"],
                        Configuration["IdentityServer:Resources:CatalogService:BaseUrl"],
                    },
                    AllowedScopes = { "catalogservice.fullaccess", "onlineshopwebgateway.fullaccess" },
                    RequireConsent = false,
                    RequireClientSecret = false
                }
            };
    }
}