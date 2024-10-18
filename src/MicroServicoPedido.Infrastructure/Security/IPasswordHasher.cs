namespace MicroServicoPedido.Security
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);
    }
}
