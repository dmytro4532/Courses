using Courses.Domain.Courses;
using FluentValidation;

namespace Courses.Application.Courses.Commands.CreateCourse;

internal sealed class CreateCourseCommandValidator
    : AbstractValidator<CreateCourseCommand>
{
    public CreateCourseCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(Title.MaxLength);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(Description.MaxLength);
    }
}
