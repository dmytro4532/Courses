using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Courses.Domain.AttemptQuestions;

namespace Courses.Infrastructure.Persistance.Configurations;

internal sealed class AttemptQuestionConfiguration : IEntityTypeConfiguration<AttemptQuestion>
{
    public void Configure(EntityTypeBuilder<AttemptQuestion> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasIndex(question => question.Id)
            .IsUnique();

        builder.Property(question => question.Content)
            .HasMaxLength(Content.MaxLength)
            .HasConversion(
                content => content.Value,
                value => Content.Create(value));

        builder.Property(question => question.Order)
            .HasConversion(
                order => order.Value,
                value => Order.Create(value));

        builder.HasOne(question => question.TestAttempt)
            .WithMany(test => test.AttemptQuestions)
            .HasForeignKey(question => question.TestAttemptId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.OwnsMany(q => q.Answers, answer =>
        {
            answer.ToJson();
            
            answer.Property(a => a.Value)
                .HasMaxLength(AttemptQuestionAnswer.MaxLength);

            answer.Property(a => a.IsCorrect);
            
            answer.Property(a => a.IsSelected);

            answer.Property(a => a.Id);
        });
    }
}
