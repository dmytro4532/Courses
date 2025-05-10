using Microsoft.AspNetCore.Http;

namespace Courses.Application.Abstractions.Services;

public interface IFileStorageService
{
    Task<string> SaveFileAsync(Stream file, string contentType);

    Task<string> CreateUrlAsync(string fileName);

    string CreateUrl(string fileName);

    Task DeleteFileAsync(string fileName);
    
    Task<Stream> GetFileAsync(string fileName);
}
