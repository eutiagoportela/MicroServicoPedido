using MicroServicoPedido.Comunication.Requests;
using MicroServicoPedido.Comunication.Responses;
using MicroServicoPedido.Domain.Repositories;
using MicroServicoPedido.Security;

namespace MicroServicoPedido.Application.UseCases.Login.DoLogin
{
    public class DoLoginUseCase : IDoLoginUseCase
    {
        private readonly JwtTokenGenerator _jwtTokenGenerator;
        private readonly IUsuarioRepository _userRepository; // Repositório de usuários em tempo de execução

        public DoLoginUseCase(JwtTokenGenerator jwtTokenGenerator, IUsuarioRepository userRepository)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _userRepository = userRepository;
        }

        public async Task<ResponseLoginJson> Execute(RequestLoginJson login)
        {

            var usuario = await _userRepository.GetByUsuarioNomeAsync(login.UsuarioNome);
            if (usuario == null || !VerifyPassword(login.Senha, usuario.SenhaHash))
            {
                return new ResponseLoginJson
                {
                    Token = "Usuário ou senha inválidos."
                };
            }

            var token = _jwtTokenGenerator.GenerateToken(login.UsuarioNome);
            return new ResponseLoginJson
            {
                Token = token
            };
        }

        private bool VerifyPassword(string enteredPassword, string storedPasswordHash)
        {
            return BCrypt.Net.BCrypt.Verify(enteredPassword, storedPasswordHash);
        }
    }
}
