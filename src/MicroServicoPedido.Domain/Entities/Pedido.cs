using System.Text.Json.Serialization;

namespace MicroServicoPedido.Domain.Entities;

public class Pedido
{
    public int PedidoId { get; init; }

    public List<Produto> Produtos { get; init; } = new List<Produto>();
}
