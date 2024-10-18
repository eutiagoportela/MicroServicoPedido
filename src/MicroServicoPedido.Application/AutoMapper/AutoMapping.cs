using AutoMapper;
using MicroServicoPedido.Comunication.DTOs;
using MicroServicoPedido.Comunication.Requests;
using MicroServicoPedido.Domain.Entities;

namespace MicroServicoPedido.Application.AutoMapper
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<PedidoDTO, Pedido>(); // Mapeia PedidoDTO para Pedido
            CreateMap<ProdutoDTO, Produto>(); // Mapeia ProdutoDTO para Produto
            CreateMap<DimensoesDTO, Dimensoes>(); // Mapeia DimensoesDTO para Dimensoes

            CreateMap<RequestPedidoJson, Pedidos>() // Mapeia RequestPedidoJson para Pedidos
                .ForMember(dest => dest.ListaPedidos, opt => opt.MapFrom(src => src.Pedidos));
        }
    }


}
