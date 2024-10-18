using MicroServicoPedido.Comunication.Requests;
using MicroServicoPedido.Comunication.Responses;


namespace MicroServicoPedido.Application.UseCases.Login.DoLogin;

public interface IDoLoginUseCase
{
    Task<ResponseLoginJson> Execute(RequestLoginJson login);
}
