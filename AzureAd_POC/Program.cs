using AzureAd_POC;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;
using Swashbuckle.AspNetCore.SwaggerGen;

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
    app.UseSwaggerUI(setup =>
    {
        setup.SwaggerEndpoint($"/swagger/v1/swagger.json", "Version 1.0");
        setup.OAuthClientId("43fc790d-e1a0-47f4-9782-b124ddf5bbcb");
        setup.OAuthAppName("workorderapi");
        setup.OAuthScopeSeparator(" ");
        setup.OAuthUsePkce();
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
