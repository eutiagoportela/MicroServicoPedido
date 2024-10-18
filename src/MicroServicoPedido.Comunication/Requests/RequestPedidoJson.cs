using MicroServicoPedido.Comunication.DTOs;
using System.Text.Json.Serialization;



namespace MicroServicoPedido.Comunication.Requests;

public class RequestPedidoJson
{
    [JsonPropertyName("pedidos")]
    public List<PedidoDTO> Pedidos { get; init; } = new List<PedidoDTO>();
}
