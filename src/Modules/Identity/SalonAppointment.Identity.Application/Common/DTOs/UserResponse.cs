namespace SalonAppointment.Identity.Application.Common.DTOs;

public record UserResponse(Guid UserId, string FullName, string Mobile, bool IsActive, IReadOnlyList<string> Roles);