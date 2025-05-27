using Courses.Domain.Common;
using Courses.Domain.Common.Guards;

namespace Courses.Domain.AttemptQuestions;

public class AttemptQuestionAnswer : ValueObject
{
    public const int MaxLength = 100;

    public AttemptQuestionAnswer() { }

    private AttemptQuestionAnswer(Guid id, string value, bool isCorrect, bool isSelected, Guid questionId)
    {
        Id = id;
        Value = value;
        IsCorrect = isCorrect;
        IsSelected = isSelected;
        QuestionId = questionId;
    }

    public Guid Id { get; }

    public string Value { get; }

    public bool IsCorrect { get; }

    public Guid QuestionId { get; }

    public bool IsSelected { get; }

    public static AttemptQuestionAnswer Create(Guid id, string value, bool isCorrect, bool isSelected, Guid questionId)
    {
        Ensure.NotEmpty(value, "Answer is required.", nameof(value));
        Ensure.NotLonger(value, MaxLength, "Answer is too long.", nameof(value));

        return new AttemptQuestionAnswer(id, value, isCorrect, isSelected, questionId);
    }

    public override string ToString() => Value;

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Id;
        yield return Value;
        yield return IsCorrect;
        yield return IsSelected;
        yield return QuestionId;
    }
}
