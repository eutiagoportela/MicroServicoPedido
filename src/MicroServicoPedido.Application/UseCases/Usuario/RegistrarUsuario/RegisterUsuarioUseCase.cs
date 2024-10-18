using MicroServicoPedido.Comunication.Requests;
using MicroServicoPedido.Comunication.Responses;
using MicroServicoPedido.Domain.Entities;
using MicroServicoPedido.Domain.Repositories;
using MicroServicoPedido.Security;



namespace MicroServicoPedido.Application.UseCases.User.RegisterUser
{
    public class RegisterUsuarioUseCase : IRegisterUsuarioUseCase
    {
        private readonly IUsuarioRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public RegisterUsuarioUseCase(IUsuarioRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<ResponseRegisterUsuarioJson> Execute(RequestRegisterUsuarioJson request)
        {
            // Verifica se o usuário já existe
            var existingUser = await _userRepository.GetByUsuarioNomeAsync(request.NomeUsuario);
            if (existingUser != null)
            {
                return new ResponseRegisterUsuarioJson { Message = " Usuário já existe" };
            }

            // Cria o novo usuário e faz o hash da senha
            var hashedPassword = _passwordHasher.HashPassword(request.Senha);
            var newUser = new Usuario { NomeUsuario = request.NomeUsuario, SenhaHash = hashedPassword };

            // Salva no repositório (pode ser em memória, banco de dados, etc.)
            await _userRepository.AddAsync(newUser);

            // Retorna a resposta com sucesso
            return new ResponseRegisterUsuarioJson { Message = "Usuário registrado com sucesso" };
        }
    }
}
