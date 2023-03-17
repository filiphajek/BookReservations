using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;

namespace BookReservations.Infrastructure.BL.Services;

public class AzureStorageService : IFileStorageService
{
    private readonly BlobContainerClient containerClient;

    public AzureStorageService(BlobContainerClient containerClient)
    {
        this.containerClient = containerClient;
    }

    public async Task<bool> DownloadFileAsync(string path, Stream destination, CancellationToken cancellationToken = default)
    {
        var client = containerClient.GetBlobClient(path);

        if (!await client.ExistsAsync(cancellationToken))
        {
            return false;
        }
        var result = await client.DownloadToAsync(destination, cancellationToken);
        return !result.IsError;
    }

    public async Task<bool> FileExists(string path, CancellationToken cancellationToken = default)
    {
        var client = containerClient.GetBlobClient(path);
        return await client.ExistsAsync(cancellationToken);
    }

    public string GetUrl(string path)
    {
        var client = containerClient.GetBlobClient(path);
        return client.Uri.AbsoluteUri;
    }

    public async Task<(bool Success, string Uri)> UploadFileAsync(string dir, IFormFile file, CancellationToken cancellationToken = default)
    {
        using var stream = file.OpenReadStream();
        var path = $"{dir}/{file.FileName}";
        var client = containerClient.GetBlobClient(path);
        var headers = new BlobUploadOptions { HttpHeaders = new BlobHttpHeaders { ContentType = file.ContentType } };
        var response = await client.UploadAsync(stream, headers, cancellationToken);
        return (!response.GetRawResponse().IsError, GetUrl(path));
    }
}
