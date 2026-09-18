using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RapidWiki.Application.Interfaces;

namespace RapidWiki.Infrastructure.Services;

public class ImageStorageService : IImageStorageService
{
    private static readonly IReadOnlyDictionary<string, string> ExtensionsByContentType =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["image/jpeg"] = ".jpg",
            ["image/png"] = ".png",
            ["image/webp"] = ".webp",
            ["image/gif"] = ".gif"
        };

    private static readonly IReadOnlyDictionary<string, string> ContentTypesByExtension =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            [".jpg"] = "image/jpeg",
            [".jpeg"] = "image/jpeg",
            [".png"] = "image/png",
            [".webp"] = "image/webp",
            [".gif"] = "image/gif"
        };

    private readonly string _storagePath;

    public ImageStorageService(IConfiguration configuration)
    {

        _storagePath = configuration["Storage:ImagesPath"] ?? throw new InvalidOperationException("Storage:ImagesPath não foi configurado.");

        Directory.CreateDirectory(_storagePath);
    }

    public async Task<string> SaveAsync(Stream stream, string contentType, CancellationToken cancellationToken = default)
    {
        if (!ExtensionsByContentType.TryGetValue(contentType, out var extension))
            throw new ArgumentException("Formato de imagem não permitido.");

        var fileName = $"{Guid.NewGuid():N}{extension}";
        var filePath = Path.Combine(_storagePath, fileName);

        await using var fileStream =
            new FileStream(
                filePath,
                FileMode.CreateNew,
                FileAccess.Write,
                FileShare.None,
                81920,
                useAsync: true
            );

        await stream.CopyToAsync(fileStream, cancellationToken);
        return fileName;
    }

    public Task<(Stream Stream, string ContentType)?> OpenReadAsync(
        string fileName,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            return Task.FromResult<(Stream Stream, string ContentType)?>(null);

        if (!string.Equals(
                fileName,
                Path.GetFileName(fileName),
                StringComparison.Ordinal))
        {
            return Task.FromResult<(Stream Stream, string ContentType)?>(null);
        }

        var extension =
            Path.GetExtension(fileName);

        if (!ContentTypesByExtension.TryGetValue(extension, out var contentType))
            return Task.FromResult<(Stream Stream, string ContentType)?>(null);

        var filePath =
            Path.Combine(_storagePath, fileName);

        if (!File.Exists(filePath))
            return Task.FromResult<(Stream Stream, string ContentType)?>(null);

        Stream stream =
            new FileStream(
                filePath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read,
                81920,
                useAsync: true
            );

        return Task.FromResult<(Stream Stream, string ContentType)?>(
            (stream, contentType)
        );
    }
}
