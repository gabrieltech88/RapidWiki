using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RapidWiki.Application.Interfaces;

namespace RapidWiki.Api.Controllers;

[ApiController]
[Authorize]
[Route("app/v1/arquivo")]
public class ArquivoController : ControllerBase
{
    private const long MaxFileSize = 5 * 1024 * 1024;

    private static readonly HashSet<string> AllowedContentTypes =
        new(StringComparer.OrdinalIgnoreCase)
        {
            "image/jpeg",
            "image/png",
            "image/webp",
            "image/gif"
        };

    private readonly IImageStorageService _imageStorageService;

    public ArquivoController(IImageStorageService imageStorageService)
    {
        _imageStorageService = imageStorageService;
    }

    [HttpPost("upload_image")]
    public async Task<IActionResult> UploadImage([FromForm] IFormFile file, CancellationToken cancellationToken)
    {
        if (file is null || file.Length == 0)
            return BadRequest("Nenhuma imagem foi enviada.");

        if (file.Length > MaxFileSize)
            return BadRequest("A imagem deve possuir no máximo 5 MB.");

        if (!AllowedContentTypes.Contains(file.ContentType))
            return BadRequest("Formato de imagem não permitido.");

        await using var stream = file.OpenReadStream();

        var fileName =await _imageStorageService.SaveAsync(stream, file.ContentType, cancellationToken);
        var url = $"{Request.Scheme}://{Request.Host}/app/v1/arquivo/image/{Uri.EscapeDataString(fileName)}";

        return Ok(new{url});
    }

    [HttpGet("image/{fileName}")]
    public async Task<IActionResult> GetImage([FromRoute] string fileName, CancellationToken cancellationToken)
    {
        var image = await _imageStorageService.OpenReadAsync(fileName, cancellationToken);

        if (image is null)
            return NotFound();

        return File(image.Value.Stream, image.Value.ContentType, enableRangeProcessing: true);
    }
}
