namespace SalonAppointment.Identity.Application.Features.Commands.Refresh;

public class RefreshCommandHandler(
    TimeProvider timeProvider,
    ISecurityService securityService,
    IRefreshTokensRepository refreshTokensRepository,
    IUsersRepository usersRepository,
    IRolesRepository rolesRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<RefreshCommand, ErrorOr<AuthResponse>>
{
    public async Task<ErrorOr<AuthResponse>> Handle(RefreshCommand command, CancellationToken cancellationToken)
    {
        var now = timeProvider.GetUtcNow().UtcDateTime;

        if (string.IsNullOrWhiteSpace(command.RefreshToken)) return AuthErrors.InvalidRefreshToken;

        var tokenHash = securityService.HashRefreshToken(command.RefreshToken);

        var existingRefreshToken = await refreshTokensRepository.GetByTokenHashAsync(tokenHash, cancellationToken);

        if (existingRefreshToken is null || !existingRefreshToken.IsActive(now)) return AuthErrors.InvalidRefreshToken;

        var user = await usersRepository.GetByIdWithRolesAsync(existingRefreshToken.UserId, cancellationToken);

        if (user is null || !user.IsActive) return AuthErrors.InvalidRefreshToken;

        var roleIds = user.UserRoles.Select(x => x.RoleId).ToList();
        var roles = await rolesRepository.GetNamesByIdsAsync(roleIds, cancellationToken);

        existingRefreshToken.Revoke(now);

        var accessToken = securityService.GenerateAccessToken(user.Id, user.FullName, user.Mobile.Value, roles);
        var (refreshToken, hashedRefreshToken) = securityService.GenerateRefreshToken();

        var newRefreshToken = RefreshToken.Create(user.Id, hashedRefreshToken, now, securityService.RefreshTokenLifetime);

        await refreshTokensRepository.AddAsync(newRefreshToken, cancellationToken);

        await unitOfWork.CommitChangesAsync(cancellationToken);

        return new AuthResponse(accessToken, refreshToken, now.Add(securityService.AccessTokenLifetime), newRefreshToken.ExpiresAt);
    }
}