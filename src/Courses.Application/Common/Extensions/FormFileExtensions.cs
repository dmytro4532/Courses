using Microsoft.AspNetCore.Http;

namespace Courses.Application.Common.Extensions;

public static class FormFileExtensions
{
    public static bool HasAllowedExtension(this IFormFile file, HashSet<string> allowedExtensions)
    {
        var extension = Path.GetExtension(file.FileName);

        return allowedExtensions.Contains(extension.ToLower());
    }
}
