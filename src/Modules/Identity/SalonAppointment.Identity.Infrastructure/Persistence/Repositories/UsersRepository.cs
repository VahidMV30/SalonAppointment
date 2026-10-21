namespace SalonAppointment.Identity.Infrastructure.Persistence.Repositories;

public class UsersRepository(IdentityDbContext dbContext) : BaseRepository<Guid, User>(dbContext), IUsersRepository
{
    public async Task<User?> GetByMobileWithRolesAsync(MobileNumber mobile, CancellationToken cancellationToken)
    {
        return await dbContext.Users.Include(x => x.UserRoles).FirstOrDefaultAsync(x => x.Mobile == mobile, cancellationToken);
    }

    public async Task<User?> GetByIdWithRolesAsync(Guid id, CancellationToken cancellationToken)
    {
        return await dbContext.Users.AsNoTracking().Include(x => x.UserRoles).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<UserResponse>> GetAllUsersAsync(CancellationToken cancellationToken)
    {
        var users = await dbContext.Users.AsNoTracking().Select(x => new
        {
            x.Id,
            x.FullName,
            Mobile = x.Mobile.Value,
            x.IsActive,
            RoleIds = x.UserRoles.Select(y => y.RoleId).ToList()
        }).ToListAsync(cancellationToken);

        var roles = await dbContext.Roles.AsNoTracking().ToDictionaryAsync(x => x.Id, x => x.Name, cancellationToken);

        return users.Select(x => new UserResponse(
            x.Id,
            x.FullName,
            x.Mobile,
            x.IsActive,
            [.. x.RoleIds.Select(id => roles[id])]));
    }
}