namespace SalonAppointment.Shared.Infrastructure.Persistence.Repositories;

public class BaseRepository<TKey, T>(DbContext dbContext) : IBaseRepository<TKey, T> where T : class
{
    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await dbContext.Set<T>().AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<T?> GetByIdAsync(TKey id, CancellationToken cancellationToken)
    {
        return await dbContext.Set<T>().FindAsync([id], cancellationToken);
    }

    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
    {
        return await dbContext.Set<T>().AnyAsync(predicate, cancellationToken);
    }

    public async Task AddAsync(T entity, CancellationToken cancellationToken)
    {
        await dbContext.AddAsync(entity, cancellationToken);
    }
}