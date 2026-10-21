namespace SalonAppointment.Identity.Application.Features.Commands.Register;

public class RegisterCommandHandler(
    TimeProvider timeProvider,
    IUsersRepository usersRepository,
    IRolesRepository rolesRepository,
    ISecurityService securityService,
    IRefreshTokensRepository refreshTokensRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<RegisterCommand, ErrorOr<AuthResponse>>
{
    public async Task<ErrorOr<AuthResponse>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        var now = timeProvider.GetUtcNow().UtcDateTime;

        var mobile = MobileNumber.Create(command.Mobile);

        var existingUser = await usersRepository.GetByMobileWithRolesAsync(mobile, cancellationToken);

        if (existingUser is not null) return AuthErrors.AlreadyRegistered;

        var userRole = await rolesRepository.GetByNameAsync(ApplicationRoles.User, cancellationToken);

        var newUser = User.Create(command.FullName, mobile, securityService.HashPassword(command.Password));
        newUser.AssignRole(userRole!.Id);

        await usersRepository.AddAsync(newUser, cancellationToken);

        var accessToken = securityService.GenerateAccessToken(
            newUser.Id,
            newUser.FullName,
            newUser.Mobile.Value,
            [userRole.Name]);

        var (refreshToken, hashedRefreshToken) = securityService.GenerateRefreshToken();

        var newRefreshToken = RefreshToken.Create(newUser.Id, hashedRefreshToken, now, securityService.RefreshTokenLifetime);

        await refreshTokensRepository.AddAsync(newRefreshToken, cancellationToken);

        await unitOfWork.CommitChangesAsync(cancellationToken);

        return new AuthResponse(accessToken, refreshToken, now.Add(securityService.AccessTokenLifetime), newRefreshToken.ExpiresAt);
    }
}