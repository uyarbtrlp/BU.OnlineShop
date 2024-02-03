// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;

namespace BU.OnlineShop.Identity
{
    public static class Config
    {
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
                    Scopes = { "catalogservice.read", "catalogservice.write" }
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
                new ApiScope("catalogservice.read"),
                new ApiScope("catalogservice.write"),
                new ApiScope("basketservice.fullaccess")
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientName = "Onlineshop Machine to Machine Client",
                    ClientId = "onlineshopm2m",
                    ClientSecrets ={new Secret("1q2w3e*".Sha256())},
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "onlineshop.fullaccess" }
                }
            };
    }
}