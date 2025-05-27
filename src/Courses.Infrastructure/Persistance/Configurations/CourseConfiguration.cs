using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Courses.Domain.Courses;

namespace Courses.Infrastructure.Persistance.Configurations;

internal sealed class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasIndex(course => course.Id)
            .IsUnique();

        builder.Property(course => course.Title)
            .HasMaxLength(Title.MaxLength)
            .HasConversion(
                title => title.Value,
                value => Title.Create(value));

        builder.Property(course => course.Description)
            .HasMaxLength(Description.MaxLength)
            .HasConversion(
                username => username.Value,
                value => Description.Create(value));
    }
}
