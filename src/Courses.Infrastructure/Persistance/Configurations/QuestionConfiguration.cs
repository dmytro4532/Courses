using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Courses.Domain.Questions;

namespace Courses.Infrastructure.Persistance.Configurations;

internal sealed class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
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

        builder.HasOne(question => question.Test)
            .WithMany(test => test.Questions)
            .HasForeignKey(question => question.TestId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.OwnsMany(q => q.Answers, answer =>
        {
            answer.ToJson();
            
            answer.Property(a => a.Value)
                .HasMaxLength(Answer.MaxLength);

            answer.Property(a => a.IsCorrect);

            answer.Property(a => a.Id);
        });
    }
}
