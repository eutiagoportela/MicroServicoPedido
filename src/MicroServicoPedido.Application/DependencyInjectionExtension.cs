using MicroServicoPedido.Application.UseCases.Login.DoLogin;
using MicroServicoPedido.Application.UseCases.Pedido.CalcularCaixas;
using MicroServicoPedido.Application.UseCases.User.RegisterUser;
using MicroServicoPedido.Domain.Repositories;
using MicroServicoPedido.Infrastructure.Repositories;
using MicroServicoPedido.Security;
using Microsoft.Extensions.DependencyInjection;


namespace MicroServicoPedido.Application;
public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services)
    {
        AddUseCases(services);
    }

    private static void AddUseCases(IServiceCollection services)
    {

        services.AddScoped<ICalcularCaixasPedidoUseCase, CalcularCaixasPedidoUseCase>();
        services.AddScoped<IDoLoginUseCase, DoLoginUseCase>();
        services.AddScoped<EmbalagemService>();
        services.AddScoped<JwtTokenGenerator>();

        services.AddSingleton<IUsuarioRepository, UsuarioRepository>();//usuarios em memoria, seguindo o ciclo de vida da aplicação apenas para teste sem banco de dados.
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IRegisterUsuarioUseCase, RegisterUsuarioUseCase>();
    }
}
