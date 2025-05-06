using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Courses.Domain.User;

namespace Courses.Infrastructure.Persistance.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasIndex(user => user.Id)
            .IsUnique();

        builder.Property(user => user.Email)
            .HasMaxLength(Email.MaxLength)
            .HasConversion(
                email => email.Value,
                value => Email.Create(value));

        builder.Property(user => user.Username)
            .HasMaxLength(Username.MaxLength)
            .HasConversion(
                username => username.Value,
                value => Username.Create(value));
    }
}
