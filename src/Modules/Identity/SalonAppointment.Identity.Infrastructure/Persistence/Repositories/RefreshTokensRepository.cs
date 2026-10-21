namespace SalonAppointment.Identity.Infrastructure.Persistence.Repositories;

public class RefreshTokensRepository(IdentityDbContext dbContext) : BaseRepository<Guid, RefreshToken>(dbContext), IRefreshTokensRepository
{
    public async Task<RefreshToken?> GetByTokenHashAsync(string tokenHash, CancellationToken cancellationToken)
    {
        return await dbContext.RefreshTokens.FirstOrDefaultAsync(x => x.TokenHash == tokenHash, cancellationToken);
    }
}