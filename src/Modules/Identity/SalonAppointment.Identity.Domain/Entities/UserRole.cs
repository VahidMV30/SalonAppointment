namespace SalonAppointment.Identity.Domain.Entities;

public class UserRole : Entity<Guid>
{
    private UserRole()
    {
    }

    public Guid RoleId { get; private set; }
    public Guid UserId { get; private set; }

    public static UserRole Create(Guid roleId, Guid userId)
    {
        return new UserRole { Id = Guid.NewGuid(), RoleId = roleId, UserId = userId };
    }
}