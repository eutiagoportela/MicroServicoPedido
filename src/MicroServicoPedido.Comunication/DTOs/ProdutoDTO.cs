
using System.Text.Json.Serialization;

namespace MicroServicoPedido.Comunication.DTOs;

public class ProdutoDTO
{
    [JsonPropertyName("produto_id")]
    public string ProdutoId { get; init; } = string.Empty;

    [JsonPropertyName("dimensoes")]
    public DimensoesDTO Dimensoes { get; init; } = new DimensoesDTO();
}
