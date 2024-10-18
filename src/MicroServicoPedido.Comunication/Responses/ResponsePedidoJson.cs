
using MicroServicoPedido.Comunication.DTOs;
using System.Text.Json.Serialization;

namespace MicroServicoPedido.Comunication.Responses;

public class ResponsePedidoJson
{
    [JsonPropertyName("pedidos")]
    public List<PedidosDTO>? Pedidos { get; set; }
}
