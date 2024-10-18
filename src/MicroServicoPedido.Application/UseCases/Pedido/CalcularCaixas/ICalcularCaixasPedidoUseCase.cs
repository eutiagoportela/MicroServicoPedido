using MicroServicoPedido.Comunication.Requests;
using MicroServicoPedido.Comunication.Responses;


namespace MicroServicoPedido.Application.UseCases.Pedido.CalcularCaixas;

public interface ICalcularCaixasPedidoUseCase
{
    Task<ResponsePedidoJson> Execute(RequestPedidoJson request);
}
