namespace SalonAppointment.Identity.Infrastructure.Persistence.Repositories;

public class RolesRepository(IdentityDbContext dbContext) : BaseRepository<Guid, Role>(dbContext), IRolesRepository
{
    public async Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        return await dbContext.Roles.AsNoTracking().FirstOrDefaultAsync(x => x.Name == name, cancellationToken);
    }

    public async Task<List<string>> GetNamesByIdsAsync(IEnumerable<Guid> roleIds, CancellationToken cancellationToken)
    {
        var ids = roleIds.ToList();

        return await dbContext.Roles.Where(x => ids.Contains(x.Id)).Select(x => x.Name).ToListAsync(cancellationToken);
    }
}