namespace MvcAuth.Domain.Interfaces.Repositories.common;
public interface ICrudRepositoryBase<T> where T : class
{
    IQueryable<T> Query();
    Task Salvar();
    Task Deletar(Guid Id);
    Task Cadastrar(T entity);
    Task Atualizar(T entity);
    Task<List<T>> ObterLista();
    Task<T?> ObterPorId(Guid id);
}
