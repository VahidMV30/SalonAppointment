namespace SalonAppointment.Identity.Application.Interfaces.Repositories;

public interface IRefreshTokensRepository : IBaseRepository<Guid, RefreshToken>
{
    Task<RefreshToken?> GetByTokenHashAsync(string tokenHash, CancellationToken cancellationToken);
}