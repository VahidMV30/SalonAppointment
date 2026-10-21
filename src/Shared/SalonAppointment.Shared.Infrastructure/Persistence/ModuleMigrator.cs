namespace SalonAppointment.Shared.Infrastructure.Persistence;

public class ModuleMigrator<TContext>(TContext dbContext) : IModuleMigrator where TContext : DbContext
{
    public async Task MigrateAsync(CancellationToken cancellationToken)
    {
        await dbContext.Database.MigrateAsync(cancellationToken);
    }
}