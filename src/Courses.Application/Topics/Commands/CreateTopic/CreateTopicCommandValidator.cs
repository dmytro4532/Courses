using Courses.Application.Common.Extensions;
using Courses.Application.Common.Settings;
using Courses.Domain.Topics;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace Courses.Application.Topics.Commands.CreateTopic;

internal sealed class CreateTopicCommandValidator : AbstractValidator<CreateTopicCommand>
{
    public CreateTopicCommandValidator(IOptions<FileSettings> fileSettings)
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(Title.MaxLength);

        RuleFor(x => x.Content)
            .NotEmpty()
            .MaximumLength(Content.MaxLength);

        RuleFor(x => x.Media)
            .Must(media =>
                media is null || media.HasAllowedExtension(fileSettings.Value.AllowedImageExtensions))
            .WithMessage($"Media must have allowed extension ({string.Join(' ', fileSettings.Value.AllowedImageExtensions)})");

        RuleFor(x => x.Order)
            .GreaterThan(0);

        RuleFor(x => x.CourseId)
            .NotEmpty();
    }
} 