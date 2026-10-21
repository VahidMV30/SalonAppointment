namespace SalonAppointment.Identity.Application.Features.Commands.Logout;

public record LogoutCommand(string? RefreshToken) : ICommand<Unit>;