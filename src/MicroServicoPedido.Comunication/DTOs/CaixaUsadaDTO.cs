
using System.Text.Json.Serialization;


namespace MicroServicoPedido.Comunication.DTOs;

public  class CaixaUsadaDTO
{
    [JsonPropertyName("caixa_id")]
    public string? CaixaId { get; set; }

    [JsonPropertyName("produtos")]
    public List<string>? Produtos { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Observacao { get; set; }
}
