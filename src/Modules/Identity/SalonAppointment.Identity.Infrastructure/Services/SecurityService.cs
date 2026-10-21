namespace SalonAppointment.Identity.Infrastructure.Services;

public class SecurityService(IConfiguration configuration, TimeProvider timeProvider, IHttpContextAccessor? httpContextAccessor) : ISecurityService
{
    public TimeSpan AccessTokenLifetime => TimeSpan.FromMinutes(configuration.GetValue<int>("AuthSettings:Jwt:AccessTokenExpiryInMinutes"));
    public TimeSpan RefreshTokenLifetime => TimeSpan.FromDays(configuration.GetValue<int>("AuthSettings:Jwt:RefreshTokenExpiryInDays"));

    public Guid? GetUserId
    {
        get
        {
            var value = httpContextAccessor?.HttpContext?.User.FindFirstValue("sub");
            return Guid.TryParse(value, out var id) ? id : null;
        }
    }

    public string HashPassword(string password)
    {
        var config = new Argon2Config
        {
            Type = Argon2Type.HybridAddressing,
            Version = Argon2Version.Nineteen,
            TimeCost = 3,
            MemoryCost = 65536,
            Lanes = 4,
            Threads = 4,
            Password = Encoding.UTF8.GetBytes(password),
            Salt = RandomNumberGenerator.GetBytes(16),
            HashLength = 32
        };

        using var argon2 = new Argon2(config);
        using var hash = argon2.Hash();
        return config.EncodeString(hash.Buffer);
    }

    public bool VerifyPassword(string hashedPassword, string password)
    {
        return Argon2.Verify(hashedPassword, password);
    }

    public string GenerateAccessToken(Guid userId, string fullName, string mobile, IEnumerable<string> roles)
    {
        var now = timeProvider.GetUtcNow().UtcDateTime;

        var claims = new List<Claim>
        {
            new("sub", userId.ToString()),
            new("jti", Guid.NewGuid().ToString()),
            new("fullName", fullName),
            new("mobile", mobile)
        };

        claims.AddRange(roles.Select(x => new Claim(ClaimTypes.Role, x)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("AuthSettings:Jwt:SecretKey")!));

        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = configuration.GetValue<string>("AuthSettings:Jwt:Issuer"),
            Audience = configuration.GetValue<string>("AuthSettings:Jwt:Audience"),
            IssuedAt = now,
            NotBefore = now,
            Expires = now.Add(AccessTokenLifetime),
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        };

        return new JsonWebTokenHandler().CreateToken(descriptor);
    }

    public (string refreshToken, string hashedRefreshToken) GenerateRefreshToken()
    {
        var refreshToken = Base64UrlEncoder.Encode(RandomNumberGenerator.GetBytes(64));

        return (refreshToken, HashRefreshToken(refreshToken));
    }

    public string HashRefreshToken(string refreshToken)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(refreshToken));

        return Convert.ToHexString(bytes);
    }
}