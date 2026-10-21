namespace SalonAppointment.Identity.Domain.Entities;

public class RefreshToken : Aggregate<Guid>
{
    private RefreshToken()
    {
    }

    public Guid UserId { get; private set; }
    public string TokenHash { get; private set; } = string.Empty;
    public DateTime ExpiresAt { get; private set; }
    public DateTime? RevokedAt { get; private set; }

    public bool IsRevoked => RevokedAt.HasValue;

    public static RefreshToken Create(Guid userId, string tokenHash, DateTime now, TimeSpan lifetime)
    {
        ArgumentOutOfRangeException.ThrowIfEqual(userId, Guid.Empty);
        ArgumentException.ThrowIfNullOrWhiteSpace(tokenHash);
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(lifetime, TimeSpan.Zero);

        return new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            TokenHash = tokenHash,
            ExpiresAt = now.Add(lifetime)
        };
    }

    public bool IsExpired(DateTime now)
    {
        return now >= ExpiresAt;
    }

    public bool IsActive(DateTime now)
    {
        return !IsRevoked && !IsExpired(now);
    }

    public void Revoke(DateTime now)
    {
        if (IsRevoked) return;
        RevokedAt = now;
    }
}