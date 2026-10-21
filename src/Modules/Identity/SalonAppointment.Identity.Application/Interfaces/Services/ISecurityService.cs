namespace SalonAppointment.Identity.Application.Interfaces.Services;

public interface ISecurityService
{
    TimeSpan AccessTokenLifetime { get; }
    TimeSpan RefreshTokenLifetime { get; }
    Guid? GetUserId { get; }
    string HashPassword(string password);
    bool VerifyPassword(string hashedPassword, string password);
    string GenerateAccessToken(Guid userId, string fullName, string mobile, IEnumerable<string> roles);
    (string refreshToken, string hashedRefreshToken) GenerateRefreshToken();
    string HashRefreshToken(string refreshToken);
}