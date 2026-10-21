using IdentityApplication = SalonAppointment.Identity.Application;

var builder = WebApplication.CreateBuilder(args);

// Api Assemblies
var identityApiAssembly = typeof(IdentityModule).Assembly;

// Application Assemblies
var identityApplicationAssembly = typeof(IdentityApplication.DependencyInjection).Assembly;

builder.Services.AddIdentityModule(builder.Configuration);
builder.Services.AddSharedModule([identityApiAssembly], [identityApplicationAssembly]);

builder.Host.UseSerilog((context, services, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext());

var app = builder.Build();

app.UseSerilogRequestLogging();

app.UseExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();

app.UseSharedModule();

app.Run();