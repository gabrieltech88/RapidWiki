namespace RapidWiki.Application.Interfaces;

public interface IImageStorageService
{
    Task<string> SaveAsync(Stream stream, string contentType, CancellationToken cancellationToken = default);
    Task<(Stream Stream, string ContentType)?> OpenReadAsync(string fileName, CancellationToken cancellationToken = default);
}
