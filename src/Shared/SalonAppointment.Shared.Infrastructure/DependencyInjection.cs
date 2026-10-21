namespace SalonAppointment.Shared.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddSharedInfrastructure(this IServiceCollection services)
    {
        return services;
    }

    public static async Task UseSharedInfrastructure(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var serviceProvider = scope.ServiceProvider;

        foreach (var moduleMigrator in serviceProvider.GetServices<IModuleMigrator>()) await moduleMigrator.MigrateAsync(CancellationToken.None);
        foreach (var dataSeeder in serviceProvider.GetServices<IDataSeeder>()) await dataSeeder.SeedAllAsync();
    }
}