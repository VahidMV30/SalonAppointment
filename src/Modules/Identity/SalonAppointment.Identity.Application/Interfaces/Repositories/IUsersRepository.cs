namespace SalonAppointment.Identity.Application.Interfaces.Repositories;

public interface IUsersRepository : IBaseRepository<Guid, User>
{
    Task<User?> GetByMobileWithRolesAsync(MobileNumber mobile, CancellationToken cancellationToken);
    Task<User?> GetByIdWithRolesAsync(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<UserResponse>> GetAllUsersAsync(CancellationToken cancellationToken);
}