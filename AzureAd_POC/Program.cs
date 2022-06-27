using AzureAd_POC;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);
var temp = builder.Configuration.GetSection("AzureAd");
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAuthentication(authentication =>
{
    authentication.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    authentication.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

//builder.Services.AddSingleton<IAuthorizationHandler, HasADGroupHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, HasScopesHandler>();
builder.Services
    .AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerGenOptions>()
    .AddSwaggerGen();
    //.AddSwaggerGen(c => {

    //    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme()
    //    {
    //        Name = "oauth2",
    //        Type = SecuritySchemeType.OAuth2,
    //        Scheme = "Bearer",
    //        BearerFormat = "JWT",
    //        In = ParameterLocation.Header,
    //        Description = "JWT Authorization header using the Bearer scheme.",
    //        Flows = new OpenApiOAuthFlows
    //        {
    //            AuthorizationCode = new OpenApiOAuthFlow
    //            {
    //                AuthorizationUrl = new Uri("https://login.microsoftonline.com/41ff26dc-250f-4b13-8981-739be8610c21/oauth2/v2.0/authorize"),
    //                TokenUrl = new Uri("https://login.microsoftonline.com/41ff26dc-250f-4b13-8981-739be8610c21/oauth2/v2.0/token"),
    //                Scopes = new Dictionary<string, string>
    //                {
    //                    { "https://slb001.onmicrosoft.com/1ef3b1cd-e2ef-4251-94fa-1f2cac3e7485/.default" , "All Scopes" },
    //                },
    //            }
    //        }
    //    });

    //    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    //            {
    //                {
    //                      new OpenApiSecurityScheme
    //                      {
    //                          Reference = new OpenApiReference
    //                          {
    //                              Type = ReferenceType.SecurityScheme,
    //                              Id = "oauth2"
    //                          }
    //                      },
    //                     new string[] {}
    //                }
    //            });
    //});

//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("ADGroupValidation", policy => policy.Requirements.Add(new HasADGroupRequirement(new string[] { "590de890-06dd-42c3-a545-6e957e4f3ece" })));
//});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("workorderapi", policy => policy.Requirements.Add(new HasScopesRequirement("workorderapi")));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint($"/swagger/v1/swagger.json", "Version 1.0");
        c.DefaultModelExpandDepth(2);
        c.DefaultModelsExpandDepth(2);
        c.DefaultModelRendering(ModelRendering.Example);
        c.DisplayRequestDuration();
        c.DocExpansion(DocExpansion.List);
        c.EnableFilter();
        c.EnableValidator();
        c.ShowCommonExtensions();
        c.ShowExtensions();
        c.OAuthClientId("43fc790d-e1a0-47f4-9782-b124ddf5bbcb");
        c.OAuthScopeSeparator(" ");
        c.OAuthUsePkce();
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
