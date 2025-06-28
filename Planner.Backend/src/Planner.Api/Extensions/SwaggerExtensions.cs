using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using NSwag;

namespace Planner.Api.Extensions;

internal static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerGenWithAuth(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.SwaggerDocument(o =>
        {
            o.EnableJWTBearerAuth = false;
            o.DocumentSettings = s =>
            {
                s.DocumentName = "Planner";
                s.Title = "Planner API";
                s.Version = "v1.0";
                s.AddAuth(JwtBearerDefaults.AuthenticationScheme, new()
                {
                    Name = "Bearer",
                    Scheme = "Bearer",
                    In = OpenApiSecurityApiKeyLocation.Header,
                    Type = OpenApiSecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = configuration["Keycloack:AuthorizationUrl"]!,
                            Scopes = new Dictionary<string, string>
                            {
                                { "openid", "openid" },
                                { "profile", "profile" }
                            }
                        }
                    }
                });
            };
        });
        return services;
    }
}