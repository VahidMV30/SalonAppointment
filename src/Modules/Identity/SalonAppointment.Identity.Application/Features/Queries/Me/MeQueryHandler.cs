namespace SalonAppointment.Identity.Application.Features.Queries.Me;

public class MeQueryHandler(
    ISecurityService securityService,
    IUsersRepository usersRepository,
    IRolesRepository rolesRepository) : IQueryHandler<MeQuery, ErrorOr<UserResponse>>
{
    public async Task<ErrorOr<UserResponse>> Handle(MeQuery query, CancellationToken cancellationToken)
    {
        if (securityService.GetUserId is not { } userId) return AuthErrors.Unauthorized;

        var user = await usersRepository.GetByIdWithRolesAsync(userId, cancellationToken);

        if (user is null) return AuthErrors.Unauthorized;

        var roleIds = user.UserRoles.Select(x => x.RoleId).ToList();
        var roles = await rolesRepository.GetNamesByIdsAsync(roleIds, cancellationToken);

        return new UserResponse(user.Id, user.FullName, user.Mobile.Value, user.IsActive, roles);
    }
}