using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Courses.Domain.Tests;

namespace Courses.Infrastructure.Persistance.Configurations;

internal sealed class TestConfiguration : IEntityTypeConfiguration<Test>
{
    public void Configure(EntityTypeBuilder<Test> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasIndex(test => test.Id)
            .IsUnique();

        builder.Property(test => test.Title)
            .HasMaxLength(Title.MaxLength)
            .HasConversion(
                title => title.Value,
                value => Title.Create(value));
    }
} 