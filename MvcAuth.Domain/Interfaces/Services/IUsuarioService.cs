using MvcAuth.Domain.Models;

namespace MvcAuth.Domain.Interfaces.Services;
public interface IUsuarioService
{
    Task<Usuario> Autenticar(string email, string senha);
    Task Atualizar(Usuario usuario);
    Task Cadastrar(Usuario usuario);
    Task Editar(Usuario usuario);
    Task Deletar(Guid Id);
    Task<List<Usuario>> ObterLista();
    Task<Usuario?> ObterPorId(Guid Id);
    Task Confirmar(Guid usuarioId, int codigoConfirmacao);
    Task AlterarSenha(Guid usuarioId, string senhaAntiga, string novaSenha);
    Task<bool> IsConfirmado (Guid usuarioId);
    Task NovoCodigoConfirmacao(Guid usuarioId);
}
