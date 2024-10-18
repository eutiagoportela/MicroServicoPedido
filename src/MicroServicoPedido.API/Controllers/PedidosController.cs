using MicroServicoPedido.Application.UseCases.Pedido.CalcularCaixas;
using MicroServicoPedido.Comunication.Requests;
using MicroServicoPedido.Comunication.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MicroServicoPedido.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        private readonly ILogger<PedidosController> _logger;

        public PedidosController(ILogger<PedidosController> logger)
        {
            _logger = logger;
        }

        [HttpPost("calcular")]
        public async Task<IActionResult> Calcular(ICalcularCaixasPedidoUseCase useCase, [FromBody] RequestPedidoJson request)
        {
            if (request.Pedidos == null || !request.Pedidos.Any())
            {
                return BadRequest(new ResponseErrorJson("Dados inválidos."));
            }

            try
            {
                var resultado = await useCase.Execute(request);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao calcular caixas.");
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorJson("Erro inesperado. Tente novamente mais tarde."));
            }
        }
    }
}
