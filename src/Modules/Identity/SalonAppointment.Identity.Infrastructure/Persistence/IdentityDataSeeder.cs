namespace SalonAppointment.Identity.Infrastructure.Persistence;

public class IdentityDataSeeder(IdentityDbContext dbContext, IConfiguration configuration, ISecurityService securityService) : IDataSeeder
{
    public async Task SeedAllAsync()
    {
        if (!await dbContext.Roles.AnyAsync())
        {
            var roles = new List<Role>
            {
                Role.Create(configuration.GetValue<string>("AuthSettings:Roles:Admin")!),
                Role.Create(configuration.GetValue<string>("AuthSettings:Roles:User")!)
            };

            await dbContext.Roles.AddRangeAsync(roles);

            await dbContext.SaveChangesAsync();
        }

        if (!await dbContext.Users.AnyAsync())
        {
            var adminRole = await dbContext.Roles.FirstOrDefaultAsync(x => x.Name == configuration.GetValue<string>("AuthSettings:Roles:Admin"));
            var userRole = await dbContext.Roles.FirstOrDefaultAsync(x => x.Name == configuration.GetValue<string>("AuthSettings:Roles:User"));

            var fullName = configuration.GetValue<string>("AuthSettings:AdminUser:FullName")!;
            var mobile = MobileNumber.Create(configuration.GetValue<string>("AuthSettings:AdminUser:Mobile")!);
            var passwordHash = securityService.HashPassword(configuration.GetValue<string>("AuthSettings:AdminUser:Password")!);

            var adminUser = User.Create(fullName, mobile, passwordHash);
            adminUser.AssignRole(adminRole!.Id);
            adminUser.AssignRole(userRole!.Id);

            await dbContext.Users.AddAsync(adminUser);

            await dbContext.SaveChangesAsync();
        }
    }
}