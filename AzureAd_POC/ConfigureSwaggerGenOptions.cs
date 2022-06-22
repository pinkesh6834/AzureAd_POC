﻿using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class ConfigureSwaggerGenOptions : IConfigureOptions<SwaggerGenOptions>
{
    public void Configure(SwaggerGenOptions options)
    {
        options.AddSecurityDefinition("OAuth2", new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.OAuth2,

            Flows = new OpenApiOAuthFlows
            {
                AuthorizationCode = new OpenApiOAuthFlow
                {
                    AuthorizationUrl = new Uri("https://login.microsoftonline.com/41ff26dc-250f-4b13-8981-739be8610c21/oauth2/v2.0/authorize"),
                    TokenUrl = new Uri("https://login.microsoftonline.com/41ff26dc-250f-4b13-8981-739be8610c21/oauth2/v2.0/token"),
                    Scopes = new Dictionary<string, string>
                    {
                        { "https://slb001.onmicrosoft.com/1ef3b1cd-e2ef-4251-94fa-1f2cac3e7485/.default" , "All Scopes" }
                    },
                }
            }
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement() {
    {
        new OpenApiSecurityScheme {
            Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                        Id = "oauth2"
                },
                Scheme = "oauth2",
                Name = "oauth2",
                In = ParameterLocation.Header
        },
        new List < string > ()
    }
});
    }
}