using MvcAuth.Domain.Models;

namespace MvcAuth.Domain.Interfaces.Services;
public interface IUsuarioService
{
    Task<Usuario> Autenticar(string email, string senha);
    Task Cadastrar(Usuario usuario);
    Task Editar(Usuario usuario);
    Task Deletar(Guid Id);
    Task<List<Usuario>> ObterLista();
    Task<Usuario?> ObterPorId(Guid Id);
}
