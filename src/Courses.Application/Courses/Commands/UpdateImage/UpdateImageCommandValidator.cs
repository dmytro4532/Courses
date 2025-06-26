using Courses.Application.Common.Extensions;
using Courses.Application.Common.Settings;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace Courses.Application.Courses.Commands.UpdateImage;

internal sealed class UpdateImageCommandValidator
    : AbstractValidator<UpdateImageCommand>
{
    public UpdateImageCommandValidator(IOptions<FileSettings> fileSettings)
    {
        RuleFor(x => x.CourseId)
            .NotEmpty()
            .WithMessage("Ідентифікатор курсу є обов'язковим.");

        RuleFor(x => x.Image)
            .Must(image =>
                image is null || image.HasAllowedExtension(fileSettings.Value.AllowedImageExtensions))
            .WithMessage($"Зображення повинно мати дозволене розширення ({string.Join(", ", fileSettings.Value.AllowedImageExtensions)})");
    }
}
