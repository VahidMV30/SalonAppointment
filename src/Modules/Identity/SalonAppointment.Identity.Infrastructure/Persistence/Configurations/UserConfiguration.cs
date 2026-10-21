namespace SalonAppointment.Identity.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.HasIndex(x => x.Mobile).IsUnique();

        builder.Property(x => x.FullName).HasMaxLength(128).IsRequired();
        builder.Property(x => x.Mobile).HasConversion(x => x.Value, y => MobileNumber.Create(y)).HasMaxLength(128).IsRequired();
        builder.Property(x => x.PasswordHash).HasMaxLength(2048).IsRequired();

        builder.Ignore(x => x.DomainEvents);

        builder.HasMany(x => x.UserRoles).WithOne().HasForeignKey(x => x.UserId);
        builder.Navigation(x => x.UserRoles).HasField("_userRoles").UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}