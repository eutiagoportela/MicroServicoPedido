using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace MicroServicoPedido.Infrastructure.Security
{
    public static class Authentication
    {
        public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var key = configuration["JwtSettings:Secret"] ?? Environment.GetEnvironmentVariable("JWT_SECRET");
            var issuer = configuration["JwtSettings:Issuer"] ?? Environment.GetEnvironmentVariable("JWT_ISSUER");
            var audience = configuration["JwtSettings:Audience"] ?? Environment.GetEnvironmentVariable("JWT_AUDIENCE");

            // Adicionando logs para verificar os valores
            Console.WriteLine($"Key: {key}");
            Console.WriteLine($"Issuer: {issuer}");
            Console.WriteLine($"Audience: {audience}");

            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("JwtSettings:Secret", "A chave secreta do JWT não está definida.");
            }

            if (string.IsNullOrEmpty(issuer))
            {
                throw new ArgumentNullException("JwtSettings:Issuer", "O issuer do JWT não está definido.");
            }

            if (string.IsNullOrEmpty(audience))
            {
                throw new ArgumentNullException("JwtSettings:Audience", "A audience do JWT não está definida.");
            }

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                };
            });
        }
    }
}
