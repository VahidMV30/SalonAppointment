namespace SalonAppointment.Shared.Api;

public static class SharedModule
{
    public static IServiceCollection AddSharedModule(this IServiceCollection services, Assembly[] apiAssemblies, Assembly[] applicationAssemblies)
    {
        services.AddSharedInfrastructure();

        services.AddSharedApplication(applicationAssemblies);

        services.AddCarter(configurator: config =>
        {
            var modules = apiAssemblies
                .SelectMany(a => a.GetTypes())
                .Where(t => typeof(ICarterModule).IsAssignableFrom(t) && t is { IsAbstract: false, IsInterface: false })
                .ToArray();

            config.WithModules(modules);
        });

        services.AddProblemDetails();
        services.AddExceptionHandler<GlobalExceptionHandler>();

        return services;
    }

    public static WebApplication UseSharedModule(this WebApplication app)
    {
        app.UseSharedInfrastructure().GetAwaiter().GetResult();

        app.MapCarter();

        return app;
    }
}