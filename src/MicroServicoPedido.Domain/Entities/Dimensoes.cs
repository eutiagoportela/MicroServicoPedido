using System.Text.Json.Serialization;

namespace MicroServicoPedido.Domain.Entities;

public class Dimensoes
{
    public int Altura { get; init; }
    public int Largura { get; init; }
    public int Comprimento { get; init; }
}
