using Microsoft.EntityFrameworkCore;
using MvcAuth.Domain.Interfaces.Repositories.common;
using MvcAuth.Repository.Data;

namespace MvcAuth.Repository.Repositories.common;
public class CrudRepositoryBase<T> : ICrudRepositoryBase<T> where T : class
{
    private readonly DbSet<T> _dbSet;
    private readonly MainDbContext _context;

    public CrudRepositoryBase(MainDbContext context)
    {
        _dbSet = context.Set<T>();
        _context = context;
    }

    public async Task Atualizar(T entity)
    {
        _dbSet.Entry(entity).State = EntityState.Modified;
        await Salvar();
    }

    public async Task Cadastrar(T entity)
    {
        _dbSet.Add(entity);
        await Salvar();
    }

    public async Task Deletar(Guid Id)
    {
        var entity = await(ObterPorId(Id));
        if (entity == null)
            throw new Exception("Objeto não encontrado");

        _dbSet.Entry(entity).State = EntityState.Deleted;
        await Salvar();
    }

    public async Task<List<T>> ObterLista() => await _dbSet.ToListAsync();

    public async Task<T?> ObterPorId(Guid id) => await _dbSet.FindAsync(id);

    public IQueryable<T> Query() => _dbSet;

    public async Task Salvar()
    {
        await _context.SaveChangesAsync();
    }
}
