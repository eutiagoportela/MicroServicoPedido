using MicroServicoPedido.Application.UseCases.Pedido.CalcularCaixas;
using MicroServicoPedido.Comunication.DTOs;
using MicroServicoPedido.Comunication.Requests;
using MicroServicoPedido.Comunication.Responses;
using MicroServicoPedido.Controllers;
using MicroServicoPedido.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

public class PedidosControllerTests
{
    private readonly PedidosController _controller;
    private readonly Mock<ICalcularCaixasPedidoUseCase> _mockUseCase;
    private readonly Mock<ILogger<PedidosController>> _mockLogger;

    public PedidosControllerTests()
    {
        _mockUseCase = new Mock<ICalcularCaixasPedidoUseCase>();
        _mockLogger = new Mock<ILogger<PedidosController>>();
        _controller = new PedidosController(_mockLogger.Object);
    }

    [Fact]
    public async Task Calcular_EmptyPedidos_ReturnsBadRequest()
    {
        var request = new RequestPedidoJson { Pedidos = new List<PedidoDTO>() };

        var result = await _controller.Calcular(_mockUseCase.Object, request);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var responseError = Assert.IsType<ResponseErrorJson>(badRequestResult.Value);
        Assert.Equal("Dados inválidos.", responseError.ErrorMessages.First());
    }

    [Fact]
    public async Task Calcular_ProductDoesNotFitInAnyBox_ReturnsExpectedMessage()
    {
        var request = new RequestPedidoJson
        {
            Pedidos = new List<PedidoDTO>
            {
                new PedidoDTO
                {
                    PedidoId = 3,
                    Produtos = new List<ProdutoDTO>
                    {
                        new ProdutoDTO
                        {
                            ProdutoId = "Televisão 50\"",
                            Dimensoes = new DimensoesDTO { Altura = 60, Largura = 120, Comprimento = 10 }
                        }
                    }
                }
            }
        };

        var expectedResponse = new ResponsePedidoJson
        {
            Pedidos = new List<PedidosDTO>
            {
                new PedidosDTO
                {
                    PedidoId = 3,
                    CaixasUsadas = new List<CaixaUsadaDTO>
                    {
                        new CaixaUsadaDTO
                        {
                            CaixaId = "Sem Caixa",
                            Produtos = new List<string> { "Televisão 50\"" },
                            Observacao = "Produto não cabe em nenhuma caixa disponível."
                        }
                    }
                }
            }
        };

        _mockUseCase.Setup(useCase => useCase.Execute(request)).ReturnsAsync(expectedResponse);

        var result = await _controller.Calcular(_mockUseCase.Object, request);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualResponse = Assert.IsAssignableFrom<ResponsePedidoJson>(okResult.Value);
        Assert.Equal(expectedResponse.Pedidos.First().CaixasUsadas.First().Observacao, actualResponse.Pedidos.First().CaixasUsadas.First().Observacao);
    }

    [Fact]
    public async Task Calcular_PedidoWithMultipleProducts_ReturnsCorrectBoxes3()
    {
        var request = new RequestPedidoJson
        {
            Pedidos = new List<PedidoDTO>
            {
                new PedidoDTO
                {
                    PedidoId = 4,
                    Produtos = new List<ProdutoDTO>
                    {
                        new ProdutoDTO { ProdutoId = "Mouse Gamer", Dimensoes = new DimensoesDTO { Altura = 5, Largura = 8, Comprimento = 12 } },
                        new ProdutoDTO { ProdutoId = "Teclado Mecânico", Dimensoes = new DimensoesDTO { Altura = 4, Largura = 45, Comprimento = 15 } }
                    }
                }
            }
        };

        var expectedResponse = new ResponsePedidoJson
        {
            Pedidos = new List<PedidosDTO>
            {
                new PedidosDTO
                {
                    PedidoId = 4,
                    CaixasUsadas = new List<CaixaUsadaDTO>
                    {
                        new CaixaUsadaDTO
                        {
                            CaixaId = "Caixa Grande",
                            Produtos = new List<string> { "Mouse Gamer", "Teclado Mecânico" }
                        }
                    }
                }
            }
        };

        _mockUseCase.Setup(useCase => useCase.Execute(request)).ReturnsAsync(expectedResponse);

        var result = await _controller.Calcular(_mockUseCase.Object, request);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualResponse = Assert.IsAssignableFrom<ResponsePedidoJson>(okResult.Value);
        Assert.Equal(expectedResponse.Pedidos.First().PedidoId, actualResponse.Pedidos.First().PedidoId);
        Assert.Equal(expectedResponse.Pedidos.First().CaixasUsadas.First().CaixaId, actualResponse.Pedidos.First().CaixasUsadas.First().CaixaId);
        Assert.Equal(expectedResponse.Pedidos.First().CaixasUsadas.First().Produtos.Count, actualResponse.Pedidos.First().CaixasUsadas.First().Produtos.Count);
    }

    [Fact]
    public async Task Calcular_MultiplePedidos_ReturnsAllExpectedBoxes2()
    {
        var request = new RequestPedidoJson
        {
            Pedidos = new List<PedidoDTO>
            {
                new PedidoDTO
                {
                    PedidoId = 1,
                    Produtos = new List<ProdutoDTO>
                    {
                        new ProdutoDTO
                        {
                            ProdutoId = "Produto A",
                            Dimensoes = new DimensoesDTO { Altura = 50, Largura = 40, Comprimento = 30 }
                        }
                    }
                },
                new PedidoDTO
                {
                    PedidoId = 2,
                    Produtos = new List<ProdutoDTO>
                    {
                        new ProdutoDTO
                        {
                            ProdutoId = "Produto B",
                            Dimensoes = new DimensoesDTO { Altura = 20, Largura = 30, Comprimento = 15 }
                        }
                    }
                },
                new PedidoDTO
                {
                    PedidoId = 3,
                    Produtos = new List<ProdutoDTO>
                    {
                        new ProdutoDTO
                        {
                            ProdutoId = "Produto C",
                            Dimensoes = new DimensoesDTO { Altura = 60, Largura = 50, Comprimento = 10 }
                        }
                    }
                }
            }
        };

        var expectedResponse = new ResponsePedidoJson
        {
            Pedidos = new List<PedidosDTO>
            {
                new PedidosDTO { PedidoId = 1, CaixasUsadas = new List<CaixaUsadaDTO> { new CaixaUsadaDTO { CaixaId = "Caixa 1", Produtos = new List<string> { "Produto A" } } } },
                new PedidosDTO { PedidoId = 2, CaixasUsadas = new List<CaixaUsadaDTO> { new CaixaUsadaDTO { CaixaId = "Caixa 2", Produtos = new List<string> { "Produto B" } } } },
                new PedidosDTO { PedidoId = 3, CaixasUsadas = new List<CaixaUsadaDTO> { new CaixaUsadaDTO { CaixaId = "Caixa 3", Produtos = new List<string> { "Produto C" } } } }
            }
        };

        _mockUseCase.Setup(useCase => useCase.Execute(request)).ReturnsAsync(expectedResponse);

        var result = await _controller.Calcular(_mockUseCase.Object, request);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualResponse = Assert.IsAssignableFrom<ResponsePedidoJson>(okResult.Value);
        Assert.Equal(expectedResponse.Pedidos.Count, actualResponse.Pedidos.Count);
    }

    [Fact]
    public async Task Calcular_PedidoWithMultipleProducts_ReturnsCorrectBoxes2()
    {
        var request = new RequestPedidoJson
        {
            Pedidos = new List<PedidoDTO>
            {
                new PedidoDTO
                {
                    PedidoId = 4,
                    Produtos = new List<ProdutoDTO>
                    {
                        new ProdutoDTO { ProdutoId = "Produto D", Dimensoes = new DimensoesDTO { Altura = 10, Largura = 20, Comprimento = 30 } },
                        new ProdutoDTO { ProdutoId = "Produto E", Dimensoes = new DimensoesDTO { Altura = 15, Largura = 25, Comprimento = 35 } }
                    }
                }
            }
        };

        var expectedResponse = new ResponsePedidoJson
        {
            Pedidos = new List<PedidosDTO>
            {
                new PedidosDTO
                {
                    PedidoId = 4,
                    CaixasUsadas = new List<CaixaUsadaDTO>
                    {
                        new CaixaUsadaDTO
                        {
                            CaixaId = "Caixa Grande",
                            Produtos = new List<string> { "Produto D", "Produto E" }
                        }
                    }
                }
            }
        };

        _mockUseCase.Setup(useCase => useCase.Execute(request)).ReturnsAsync(expectedResponse);

        var result = await _controller.Calcular(_mockUseCase.Object, request);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualResponse = Assert.IsAssignableFrom<ResponsePedidoJson>(okResult.Value);
        Assert.Equal(expectedResponse.Pedidos.First().PedidoId, actualResponse.Pedidos.First().PedidoId);
        Assert.Equal(expectedResponse.Pedidos.First().CaixasUsadas.First().CaixaId, actualResponse.Pedidos.First().CaixasUsadas.First().CaixaId);
    }
}
