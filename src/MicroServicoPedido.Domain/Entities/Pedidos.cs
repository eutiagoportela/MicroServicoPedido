using System.Text.Json.Serialization;

namespace MicroServicoPedido.Domain.Entities;

public class Pedidos
{
    public List<Pedido> ListaPedidos { get; init; } = new List<Pedido>();
}
