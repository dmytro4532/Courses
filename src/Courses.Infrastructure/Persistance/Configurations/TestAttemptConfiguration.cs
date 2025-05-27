using Courses.Domain.CompletedTopics;
using Courses.Domain.TestAttempts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Courses.Infrastructure.Persistence.Configurations;

public class TestAttemptConfiguration : IEntityTypeConfiguration<TestAttempt>
{
    public void Configure(EntityTypeBuilder<TestAttempt> builder)
    {
        builder.HasKey(ct => ct.Id);

        builder.Property(ct => ct.CreatedAt)
            .IsRequired();

        builder.Property(cp => cp.TestId)
            .IsRequired();

        builder.Property(cp => cp.UserId)
            .IsRequired();

        builder.HasIndex(testAttempt => testAttempt.TestId);

        builder.HasIndex(testAttempt => testAttempt.UserId);
    }
}
