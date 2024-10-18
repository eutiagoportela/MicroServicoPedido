using MicroServicoPedido.Application.UseCases.User.RegisterUser;
using MicroServicoPedido.Comunication.Requests;
using MicroServicoPedido.Comunication.Responses;
using Microsoft.AspNetCore.Mvc;

namespace MicroServicoPedido.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly ILogger<UsuariosController> _logger;

        public UsuariosController(ILogger<UsuariosController> logger)
        {
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(
            [FromServices] IRegisterUsuarioUseCase useCase,
            [FromBody] RequestRegisterUsuarioJson request)
        {
            // Validação de campos vazios
            if (string.IsNullOrWhiteSpace(request.NomeUsuario) || string.IsNullOrWhiteSpace(request.Senha))
            {
                return BadRequest(new ResponseErrorJson(new List<string> { "Usuário ou senha inválidos." }));
            }

            try
            {
                var response = await useCase.Execute(request);

                if (response.Message == "Usuário já existe")
                {
                    return BadRequest(new ResponseErrorJson(new List<string> { response.Message }));
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao registrar usuário.");
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorJson(new List<string> { "Erro inesperado. Tente novamente mais tarde." }));
            }
        }
    }
}
