namespace SalonAppointment.Identity.Application.Features.Commands.Login;

public class LoginCommandHandler(
    TimeProvider timeProvider,
    IUsersRepository usersRepository,
    ISecurityService securityService,
    IRolesRepository rolesRepository,
    IRefreshTokensRepository refreshTokensRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<LoginCommand, ErrorOr<AuthResponse>>
{
    public async Task<ErrorOr<AuthResponse>> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var now = timeProvider.GetUtcNow().UtcDateTime;

        var mobile = MobileNumber.Create(command.Mobile);

        var user = await usersRepository.GetByMobileWithRolesAsync(mobile, cancellationToken);

        if (user is null || !securityService.VerifyPassword(user.PasswordHash, command.Password)) return AuthErrors.InvalidCredentials;

        if (!user.IsActive) return AuthErrors.AccountDeactivated;

        var roleIds = user.UserRoles.Select(x => x.RoleId).ToList();
        var roles = await rolesRepository.GetNamesByIdsAsync(roleIds, cancellationToken);

        var accessToken = securityService.GenerateAccessToken(user.Id, user.FullName, user.Mobile.Value, roles);

        var (refreshToken, hashedRefreshToken) = securityService.GenerateRefreshToken();

        var newRefreshToken = RefreshToken.Create(user.Id, hashedRefreshToken, now, securityService.RefreshTokenLifetime);

        await refreshTokensRepository.AddAsync(newRefreshToken, cancellationToken);

        await unitOfWork.CommitChangesAsync(cancellationToken);

        return new AuthResponse(accessToken, refreshToken, now.Add(securityService.AccessTokenLifetime), newRefreshToken.ExpiresAt);
    }
}