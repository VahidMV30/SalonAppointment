namespace SalonAppointment.Identity.Application.Features.Queries.GetAllUsers;

public class GetAllUsersQueryHandler(IUsersRepository usersRepository) : IQueryHandler<GetAllUsersQuery, IEnumerable<UserResponse>>
{
    public async Task<IEnumerable<UserResponse>> Handle(GetAllUsersQuery query, CancellationToken cancellationToken)
    {
        var users = await usersRepository.GetAllUsersAsync(cancellationToken);

        return users;
    }
}