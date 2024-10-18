using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MicroServicoPedido.Security
{
    public class JwtTokenGenerator
    {
        private readonly IConfiguration _configuration;

        public JwtTokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(string username)
        {
            // Verifica se o ambiente é de produção
            var isProduction = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production";

            // Obtém as configurações conforme o ambiente
            var key = isProduction
                ? Environment.GetEnvironmentVariable("JWT_SECRET")
                : _configuration["JwtSettings:Secret"];

            var issuer = isProduction
                ? Environment.GetEnvironmentVariable("JWT_ISSUER")
                : _configuration["JwtSettings:Issuer"];

            var audience = isProduction
                ? Environment.GetEnvironmentVariable("JWT_AUDIENCE")
                : _configuration["JwtSettings:Audience"];

            // Verifica se a chave é nula ou vazia
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key), "A chave secreta não pode ser nula ou vazia.");
            }

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username)
            };

            // Obtendo o tempo de expiração
            double expirationInMinutes;
            if (isProduction)
            {
                // Em produção, obter de variável de ambiente
                var expirationValue = Environment.GetEnvironmentVariable("JWT_EXPIRATION_IN_MINUTES");
                if (!double.TryParse(expirationValue, out expirationInMinutes))
                {
                    expirationInMinutes = 60; // Valor padrão se a variável não estiver definida
                }
            }
            else
            {
                // Em desenvolvimento, obter do appsettings
                expirationInMinutes = Convert.ToDouble(_configuration["JwtSettings:ExpirationInMinutes"]);
            }

            Console.WriteLine($"Expiration Time in Minutes: {expirationInMinutes}"); // Log do tempo de expiração

            var tokenOptions = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(expirationInMinutes), // Usa o valor apropriado
                signingCredentials: signinCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return tokenString;
        }
    }
}
