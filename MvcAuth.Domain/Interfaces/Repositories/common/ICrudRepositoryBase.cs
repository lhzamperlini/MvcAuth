using MvcAuth.Domain.Models.common;

namespace MvcAuth.Domain.Interfaces.Repositories.common;
public interface ICrudRepositoryBase<T> : IRepositoryBase where T : EntityBase
{
    IQueryable<T> Query();
    Task Salvar();
    Task Deletar(Guid Id);
    Task Cadastrar(T entity);
    Task Atualizar(T entity);
    Task<List<T>> ObterLista();
    Task<T?> ObterPorId(Guid id);
}
