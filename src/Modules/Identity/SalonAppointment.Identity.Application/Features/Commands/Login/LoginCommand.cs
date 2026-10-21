namespace SalonAppointment.Identity.Application.Features.Commands.Login;

public record LoginCommand(string Mobile, string Password) : ICommand<ErrorOr<AuthResponse>>;