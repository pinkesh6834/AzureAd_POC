using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;

public class ConfigureSwaggerGenOptions : IConfigureOptions<SwaggerGenOptions>
{
    public void Configure(SwaggerGenOptions c)
    {
        c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme()
        {
            In = ParameterLocation.Header,
            Name = "Authorization",
            Type = SecuritySchemeType.OAuth2,
            Description = "OAuth2 authentication",
            Flows = new OpenApiOAuthFlows
            {
                AuthorizationCode = new OpenApiOAuthFlow
                {
                    AuthorizationUrl = new Uri("https://login.microsoftonline.com/41ff26dc-250f-4b13-8981-739be8610c21/oauth2/v2.0/authorize"),
                    TokenUrl = new Uri("https://login.microsoftonline.com/41ff26dc-250f-4b13-8981-739be8610c21/oauth2/v2.0/token"),
                    Scopes = new Dictionary<string, string>
                    {
                        { "https://slb001.onmicrosoft.com/1ef3b1cd-e2ef-4251-94fa-1f2cac3e7485/.default" , "All Scopes" },
                    },
                }
            }
        });

        c.OperationFilter<SecurityRequirementsOperationFilter>();
    }
}