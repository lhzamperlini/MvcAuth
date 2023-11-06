using MvcAuth.Domain.Interfaces.Repositories.common;
using MvcAuth.Domain.Models;

namespace MvcAuth.Domain.Interfaces.Repositories;
public interface IUsuarioRepository : ICrudRepositoryBase<Usuario>
{
    Task<Usuario?> ObterPorEmail(string email);
    Task<bool> VerificarExistente(string email);
}
