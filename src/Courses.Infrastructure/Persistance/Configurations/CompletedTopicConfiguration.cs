using Courses.Domain.CompletedTopics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Courses.Infrastructure.Persistence.Configurations;

public class CompletedTopicConfiguration : IEntityTypeConfiguration<CompletedTopic>
{
    public void Configure(EntityTypeBuilder<CompletedTopic> builder)
    {
        builder.HasKey(ct => ct.Id);

        builder.Property(ct => ct.CreatedAt)
            .IsRequired();

        builder.Property(cp => cp.TopicId)
            .IsRequired();

        builder.Property(cp => cp.UserId)
            .IsRequired();

        builder.HasIndex(topic => topic.TopicId);

        builder.HasIndex(topic => topic.UserId);
    }
}
