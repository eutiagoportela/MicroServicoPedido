using MicroServicoPedido.Domain.Entities;

namespace MicroServicoPedido.Domain.Repositories
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> GetByUsuarioNomeAsync(string usuarionome);
        Task AddAsync(Usuario usuario);
    }
}
