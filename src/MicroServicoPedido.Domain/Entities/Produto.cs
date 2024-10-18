using System.Text.Json.Serialization;

namespace MicroServicoPedido.Domain.Entities;

public class Produto
{
    public string ProdutoId { get; init; } = string.Empty;

    public Dimensoes Dimensoes { get; init; } = new Dimensoes();
}
