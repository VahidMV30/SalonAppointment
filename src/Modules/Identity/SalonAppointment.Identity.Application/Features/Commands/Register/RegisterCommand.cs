namespace SalonAppointment.Identity.Application.Features.Commands.Register;

public record RegisterCommand(string FullName, string Mobile, string Password) : ICommand<ErrorOr<AuthResponse>>;