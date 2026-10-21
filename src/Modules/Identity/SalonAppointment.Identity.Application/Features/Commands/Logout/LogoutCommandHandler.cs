namespace SalonAppointment.Identity.Application.Features.Commands.Logout;

public class LogoutCommandHandler(
    TimeProvider timeProvider,
    ISecurityService securityService,
    IRefreshTokensRepository refreshTokensRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<LogoutCommand, Unit>
{
    public async Task<Unit> Handle(LogoutCommand command, CancellationToken cancellationToken)
    {
        var now = timeProvider.GetUtcNow().UtcDateTime;

        if (string.IsNullOrWhiteSpace(command.RefreshToken)) return Unit.Value;

        var tokenHash = securityService.HashRefreshToken(command.RefreshToken);

        var existingRefreshToken = await refreshTokensRepository.GetByTokenHashAsync(tokenHash, cancellationToken);

        if (existingRefreshToken is null || existingRefreshToken.IsRevoked) return Unit.Value;

        existingRefreshToken.Revoke(now);

        await unitOfWork.CommitChangesAsync(cancellationToken);

        return Unit.Value;
    }
}