namespace SalonAppointment.Identity.Application.Features.Commands.Refresh;

public record RefreshCommand(string? RefreshToken) : ICommand<ErrorOr<AuthResponse>>;