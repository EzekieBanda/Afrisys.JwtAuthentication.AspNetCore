using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Afrisys.JwtAuthKit;

public static class JwtAuthKitExtension
{
    public static IServiceCollection AddJwtAuthKit(
        this IServiceCollection services,
        IConfiguration config)
    {
        var auth = config.GetSection("Auth").Get<AuthOptions>();

        if (auth == null || string.IsNullOrWhiteSpace(auth.Authority))
            throw new Exception("Auth:Authority is required.");

        if (string.IsNullOrWhiteSpace(auth.Audience))
            throw new Exception("Auth:Audience is required.");

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = auth.Authority;

                options.RequireHttpsMetadata = !auth.Authority.StartsWith("http://");

                options.RefreshOnIssuerKeyNotFound = true;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = auth.Authority,

                    ValidateAudience = true,
                    ValidAudience = auth.Audience,

                    ValidateLifetime = true
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = ctx =>
                    {
                        Console.WriteLine($"❌ Token validation failed: {ctx.Exception.Message}");
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = ctx =>
                    {
                        Console.WriteLine("✅ Token validated successfully");
                        return Task.CompletedTask;
                    }
                };
            });

        services.AddAuthorizationBuilder()
            .SetFallbackPolicy(new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build());

        return services;
    }
}