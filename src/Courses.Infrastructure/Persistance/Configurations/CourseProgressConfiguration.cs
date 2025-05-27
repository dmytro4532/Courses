using Courses.Domain.CourseProgresses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Courses.Infrastructure.Persistence.Configurations;

public class CourseProgressConfiguration : IEntityTypeConfiguration<CourseProgress>
{
    public void Configure(EntityTypeBuilder<CourseProgress> builder)
    {
        builder.HasKey(cp => cp.Id);

        builder.Property(cp => cp.Completed)
            .IsRequired();

        builder.Property(cp => cp.CompletedAt);

        builder.Property(cp => cp.CreatedAt)
            .IsRequired();

        builder.Property(cp => cp.CourseId)
            .IsRequired();

        builder.Property(cp => cp.UserId)
            .IsRequired();

        builder.HasIndex(topic => topic.CourseId);

        builder.HasIndex(topic => topic.UserId);
    }
}
