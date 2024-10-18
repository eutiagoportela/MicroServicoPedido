using System.Text.Json.Serialization;

namespace MicroServicoPedido.Comunication.DTOs
{
    public class PedidoDTO
    {
        [JsonPropertyName("pedido_id")]
        public int PedidoId { get; init; }

        [JsonPropertyName("produtos")]
        public List<ProdutoDTO> Produtos { get; init; } = new List<ProdutoDTO>();

    }
}
