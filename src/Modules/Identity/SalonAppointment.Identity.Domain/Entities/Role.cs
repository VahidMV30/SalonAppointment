namespace SalonAppointment.Identity.Domain.Entities;

public class Role : Aggregate<Guid>
{
    private Role()
    {
    }

    public string Name { get; private set; } = string.Empty;

    public static Role Create(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        return new Role { Id = Guid.NewGuid(), Name = name.Trim() };
    }
}