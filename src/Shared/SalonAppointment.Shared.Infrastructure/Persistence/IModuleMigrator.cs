namespace SalonAppointment.Shared.Infrastructure.Persistence;

public interface IModuleMigrator
{
    Task MigrateAsync(CancellationToken cancellationToken);
}