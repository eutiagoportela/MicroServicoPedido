using MicroServicoPedido.Comunication.Requests;
using MicroServicoPedido.Comunication.Responses;
using MicroServicoPedido.Application.Configuration;
using MicroServicoPedido.Domain.Entities;
using MicroServicoPedido.Comunication.DTOs;
using AutoMapper;

namespace MicroServicoPedido.Application.UseCases.Pedido.CalcularCaixas
{
    public class CalcularCaixasPedidoUseCase : ICalcularCaixasPedidoUseCase
    {
        private readonly EmbalagemService _embalagemService;
        private readonly IMapper _mapper;

        public CalcularCaixasPedidoUseCase(EmbalagemService embalagemService, IMapper mapper)
        {
            _embalagemService = embalagemService;
            _mapper = mapper;
        }

        public async Task<ResponsePedidoJson> Execute(RequestPedidoJson request)
        {
            var response = new ResponsePedidoJson { Pedidos = new List<PedidosDTO>() };

            var pedidosMapper = _mapper.Map<Pedidos>(request); // Mapeando RequestPedidoJson diretamente para Pedidos

            foreach (var pedido in pedidosMapper.ListaPedidos)
            {
                var pedidoResponse = new PedidosDTO { PedidoId = pedido.PedidoId, CaixasUsadas = new List<CaixaUsadaDTO>() };
                var produtosRestantes = pedido.Produtos.ToList();//pega os produtos do pedido que veio do DTO e agora esta no dominio

                // Usar caixas disponíveis como uma lista
                var caixasDisponiveis = CaixasDisponiveis.Caixas.OrderBy(c => c.Altura * c.Largura * c.Comprimento).ToList();

                while (produtosRestantes.Any())
                {
                    CaixaUsadaDTO? melhorCaixa = null;
                    List<string> produtosNaMelhorCaixa = new List<string>();
                    List<Produto> produtosQueNaoCabem = new List<Produto>();

                    foreach (var (index, (altura, largura, comprimento)) in caixasDisponiveis.Select((c, i) => (i, c)))
                    {
                        var caixa = new Caixa { Altura = altura, Largura = largura, Comprimento = comprimento };
                        var (produtosNaCaixa, produtosQueNaoCabemTemp) = _embalagemService.EmbalarProdutos(produtosRestantes, caixa);

                        if (produtosNaCaixa.Count > 0)
                        {
                            if (melhorCaixa == null || produtosNaCaixa.Count > produtosNaMelhorCaixa.Count)
                            {
                                melhorCaixa = new CaixaUsadaDTO
                                {
                                    CaixaId = $"Caixa {index + 1}", // Usa o índice da caixa disponível
                                    Produtos = produtosNaCaixa
                                };

                                produtosNaMelhorCaixa = produtosNaCaixa;
                                produtosQueNaoCabem = produtosQueNaoCabemTemp;
                            }
                        }
                    }

                    if (melhorCaixa != null)
                    {
                        pedidoResponse.CaixasUsadas.Add(melhorCaixa);
                        produtosRestantes = produtosQueNaoCabem;
                    }
                    else
                    {
                        pedidoResponse.CaixasUsadas.Add(new CaixaUsadaDTO
                        {
                            CaixaId = "Sem Caixa",
                            Produtos = produtosRestantes.Select(p => p.ProdutoId).ToList(),
                            Observacao = "Produto não cabe em nenhuma caixa disponível."
                        });
                        break; // Interrompe se não houver mais caixas disponíveis
                    }
                }

                response.Pedidos.Add(pedidoResponse);
            }

            return response;
        }
    }
}
