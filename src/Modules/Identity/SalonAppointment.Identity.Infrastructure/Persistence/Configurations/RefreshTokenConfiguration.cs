namespace SalonAppointment.Identity.Infrastructure.Persistence.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.HasIndex(x => x.TokenHash).IsUnique();

        builder.Property(x => x.TokenHash).HasMaxLength(256).IsRequired();

        builder.Ignore(x => x.DomainEvents);

        builder.HasOne<User>().WithMany().HasForeignKey(x => x.UserId);
    }
}