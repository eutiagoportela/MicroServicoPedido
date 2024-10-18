using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MicroServicoPedido.Comunication.DTOs
{
    public  class DimensoesDTO
    {
        [JsonPropertyName("altura")]
        public int Altura { get; init; }

        [JsonPropertyName("largura")]
        public int Largura { get; init; }

        [JsonPropertyName("comprimento")]
        public int Comprimento { get; init; }
    }
}
