namespace SalonAppointment.Identity.Application.Interfaces.Repositories;

public interface IRolesRepository : IBaseRepository<Guid, Role>
{
    Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken);
    Task<List<string>> GetNamesByIdsAsync(IEnumerable<Guid> roleIds, CancellationToken cancellationToken);
}