namespace SalonAppointment.Identity.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddIdentityInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();

        services.AddDbContext<IdentityDbContext>((serviceProvider, options) =>
        {
            options.AddInterceptors(serviceProvider.GetServices<ISaveChangesInterceptor>());
            options.UseSqlServer(configuration.GetConnectionString("SalonAppointment"),
                sql => { sql.MigrationsHistoryTable("__EFMigrationsHistory", "Identity"); });
        });

        services.AddScoped<IModuleMigrator, ModuleMigrator<IdentityDbContext>>();
        services.AddScoped<IDataSeeder, IdentityDataSeeder>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(o =>
            {
                o.MapInboundClaims = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration.GetValue<string>("AuthSettings:Jwt:Issuer"),
                    ValidateAudience = true,
                    ValidAudience = configuration.GetValue<string>("AuthSettings:Jwt:Audience"),
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("AuthSettings:Jwt:SecretKey")!)),
                    ValidAlgorithms = [SecurityAlgorithms.HmacSha256],
                    ClockSkew = TimeSpan.Zero,
                    NameClaimType = "sub",
                    RoleClaimType = ClaimTypes.Role
                };
            });

        services.AddAuthorization();

        services.AddHttpContextAccessor();

        services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<IdentityDbContext>());

        services.AddSingleton(TimeProvider.System);

        services.AddScoped<ISecurityService, SecurityService>();

        services.AddScoped<IRolesRepository, RolesRepository>();
        services.AddScoped<IUsersRepository, UsersRepository>();
        services.AddScoped<IRefreshTokensRepository, RefreshTokensRepository>();

        return services;
    }
}