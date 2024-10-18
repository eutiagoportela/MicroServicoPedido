using MicroServicoPedido.Comunication.Requests;
using MicroServicoPedido.Comunication.Responses;

namespace MicroServicoPedido.Application.UseCases.User.RegisterUser
{
    public interface IRegisterUsuarioUseCase
    {
        Task<ResponseRegisterUsuarioJson> Execute(RequestRegisterUsuarioJson request);
    }
}
