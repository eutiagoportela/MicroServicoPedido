using System.Text.Json.Serialization;

namespace MicroServicoPedido.Domain.Entities
{
    public class CaixaUsada
    {
        public string? CaixaId { get; set; }

        public List<string>? Produtos { get; set; }

        public string? Observacao { get; set; }
    }

}
