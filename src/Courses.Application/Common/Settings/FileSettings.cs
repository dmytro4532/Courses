namespace Courses.Application.Common.Settings;

public class FileSettings
{
    public const string SectionName = nameof(FileSettings);

    public required HashSet<string> AllowedImageExtensions { get; init; }

    public required HashSet<string> AllowedVideoExtensions { get; init; }
}
