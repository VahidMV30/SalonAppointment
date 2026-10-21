namespace SalonAppointment.Identity.Domain.Entities;

public class User : Aggregate<Guid>
{
    private readonly List<UserRole> _userRoles = [];

    private User()
    {
    }

    public string FullName { get; private set; } = string.Empty;
    public MobileNumber Mobile { get; private set; } = null!;
    public string PasswordHash { get; private set; } = string.Empty;
    public bool IsActive { get; private set; }
    public IReadOnlyCollection<UserRole> UserRoles => _userRoles.AsReadOnly();

    public static User Create(string fullName, MobileNumber mobile, string passwordHash)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fullName);
        ArgumentNullException.ThrowIfNull(mobile);
        ArgumentException.ThrowIfNullOrWhiteSpace(passwordHash);

        return new User
        {
            Id = Guid.NewGuid(),
            FullName = fullName.Trim(),
            Mobile = mobile,
            PasswordHash = passwordHash,
            IsActive = true
        };
    }

    public void AssignRole(Guid roleId)
    {
        ArgumentOutOfRangeException.ThrowIfEqual(roleId, Guid.Empty);

        if (_userRoles.Any(x => x.RoleId == roleId)) return;

        _userRoles.Add(UserRole.Create(roleId, Id));
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}