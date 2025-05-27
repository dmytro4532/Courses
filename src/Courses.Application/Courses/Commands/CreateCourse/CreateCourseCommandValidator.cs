using Courses.Application.Common.Extensions;
using Courses.Application.Common.Settings;
using Courses.Domain.Courses;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace Courses.Application.Courses.Commands.CreateCourse;

internal sealed class CreateCourseCommandValidator
    : AbstractValidator<CreateCourseCommand>
{
    public CreateCourseCommandValidator(
        IOptions<FileSettings> fileSettings)
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(Title.MaxLength);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(Description.MaxLength);

        RuleFor(x => x.Image)
            .Must(image =>
            image is null || image.HasAllowedExtension(fileSettings.Value.AllowedImageExtensions))
            .WithMessage($"Image must have allowed extension ({string.Join(' ', fileSettings.Value.AllowedImageExtensions)})");
    }
}
