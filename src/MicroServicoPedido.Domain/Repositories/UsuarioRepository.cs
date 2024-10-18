using MicroServicoPedido.Domain.Entities;
using MicroServicoPedido.Domain.Repositories;

namespace MicroServicoPedido.Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly List<Usuario> _usuarios = new();

        public Task AddAsync(Usuario usuario)
        {
            _usuarios.Add(usuario); // Adiciona o usuário à lista em memoria
            return Task.CompletedTask;
        }

        public Task<Usuario?> GetByUsuarioNomeAsync(string usuarionome)
        {
            var usuario = _usuarios.FirstOrDefault(u => u.NomeUsuario == usuarionome);
            return Task.FromResult<Usuario?>(usuario);
        }

    }
}
