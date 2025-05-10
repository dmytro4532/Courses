using Courses.Domain.Topics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Courses.Infrastructure.Persistance.Configurations;

internal sealed class TopicConfiguration : IEntityTypeConfiguration<Topic>
{
    public void Configure(EntityTypeBuilder<Topic> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasIndex(topic => topic.Id)
            .IsUnique();

        builder.Property(topic => topic.Title)
            .HasMaxLength(Title.MaxLength)
            .HasConversion(
                title => title.Value,
                value => Title.Create(value));

        builder.Property(topic => topic.Content)
            .HasMaxLength(Content.MaxLength)
            .HasConversion(
                content => content.Value,
                value => Content.Create(value));

        builder.Property(topic => topic.Media)
            .HasMaxLength(255);

        builder.Property(topic => topic.Order)
            .IsRequired();

        builder.Property(topic => topic.CourseId)
            .IsRequired();

        builder.HasIndex(topic => topic.CourseId);
    }
} 