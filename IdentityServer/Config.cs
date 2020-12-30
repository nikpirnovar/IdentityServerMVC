// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;

using System.Collections.Generic;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId()
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("mvc.pravica1"),
                new ApiScope("mvc.pravica2")
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                 new Client
                 {
                     ClientId = "mvc",
                     ClientName = "Client za spletno stran (mvc)",
                     AllowedGrantTypes = GrantTypes.Code,
                     ClientSecrets = new List<Secret> {new Secret("secret".Sha256())},
                     AllowedScopes = new List<string> {"mvc.pravica1", "openid"},
                     RedirectUris= new List<string>()
                     {
                         "https://localhost:5000/oauth/callback"
                     },
                     AlwaysIncludeUserClaimsInIdToken = true
                 }
            };
    }
}