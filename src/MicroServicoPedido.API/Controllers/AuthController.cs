using MicroServicoPedido.Application.UseCases.Login.DoLogin;
using MicroServicoPedido.Comunication.Requests;
using MicroServicoPedido.Comunication.Responses;
using Microsoft.AspNetCore.Mvc;

namespace MicroServicoPedido.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IDoLoginUseCase _doLoginUseCase;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IDoLoginUseCase doLoginUseCase, ILogger<AuthController> logger)
        {
            _doLoginUseCase = doLoginUseCase;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] RequestLoginJson request)
        {
            if (string.IsNullOrWhiteSpace(request.UsuarioNome) || string.IsNullOrWhiteSpace(request.Senha))
            {
                return BadRequest(new ResponseErrorJson(new List<string> { "Usuário e senha são obrigatórios." }));
            }

            try
            {
                var response = await _doLoginUseCase.Execute(request);
                if (response != null)
                {
                    return Ok(response);
                }

                return Unauthorized(new ResponseErrorJson(new List<string> { "Credenciais inválidas." }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao realizar login.");
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorJson(new List<string> { "Erro inesperado. Tente novamente mais tarde." }));
            }
        }
    }
}
