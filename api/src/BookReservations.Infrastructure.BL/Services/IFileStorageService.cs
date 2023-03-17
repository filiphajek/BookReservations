using Microsoft.AspNetCore.Http;

namespace BookReservations.Infrastructure.BL.Services;

public interface IFileStorageService
{
    Task<(bool Success, string Uri)> UploadFileAsync(string dir, IFormFile file, CancellationToken cancellationToken = default);
    Task<bool> DownloadFileAsync(string path, Stream destination, CancellationToken cancellationToken = default);
    Task<bool> FileExists(string path, CancellationToken cancellationToken = default);
    string GetUrl(string path);
}
