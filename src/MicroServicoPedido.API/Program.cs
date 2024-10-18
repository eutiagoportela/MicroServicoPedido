using MicroServicoPedido.Infrastructure.Security;
using MicroServicoPedido.Application;
using Microsoft.OpenApi.Models;
using Serilog;
using MicroServicoPedido.Application.AutoMapper;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

// Configura��o do Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

// Adicionar suporte a vari�veis de ambiente para produ��o via Docker
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddAutoMapper(typeof(AutoMapping));
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddControllers();
builder.Logging.AddConsole();

// Configura��o do Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config =>
{
    config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = @"Cabe�alho de autoriza��o JWT usando o esquema Bearer. Insira 'Bearer' [espa�o] e, em seguida, seu token no campo de texto abaixo. Exemplo: 'Bearer 12345abcdef'.",
        In = ParameterLocation.Header,
        Scheme = "Bearer",
        Type = SecuritySchemeType.ApiKey
    });

    config.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

var app = builder.Build();

// Verifica��o da configura��o do AutoMapper
var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapping>());
mapperConfig.AssertConfigurationIsValid(); // Valida a configura��o do AutoMapper

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
