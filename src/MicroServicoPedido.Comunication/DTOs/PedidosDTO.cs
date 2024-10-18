

using System.Text.Json.Serialization;

namespace MicroServicoPedido.Comunication.DTOs
{
    public class PedidosDTO
    {
        [JsonPropertyName("pedido_id")]
        public int PedidoId { get; set; }

        [JsonPropertyName("caixas")]
        public List<CaixaUsadaDTO>? CaixasUsadas { get; set; }
    }
}
