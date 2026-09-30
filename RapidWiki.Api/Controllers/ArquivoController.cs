using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RapidWiki.Application.Interfaces;
using RapidWiki.Application.AddArquivo;
using MediatR;

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

    private readonly ISender _sender;
    private readonly IImageStorageService _imageStorageService;

    public ArquivoController(IImageStorageService imageStorageService, ISender sender)
    {
        _imageStorageService = imageStorageService;
        _sender = sender;
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

        var fileName = await _imageStorageService.SaveAsync(stream, file.ContentType, cancellationToken);
        var url = $"{Request.Scheme}://{Request.Host}/app/v1/arquivo/image/{Uri.EscapeDataString(fileName)}";

        return Ok(new { url });
    }

    [HttpGet("image/{fileName}")]
    public async Task<IActionResult> GetImage([FromRoute] string fileName, CancellationToken cancellationToken)
    {
        var image = await _imageStorageService.OpenReadAsync(fileName, cancellationToken);

        if (image is null)
            return NotFound();

        return File(image.Value.Stream, image.Value.ContentType, enableRangeProcessing: true);
    }

    [HttpPost("upload_arquivo")]
    public async Task<IActionResult> UploadArquivo([FromForm] IFormFile file, [FromForm] List<Guid> departamentoIds, CancellationToken cancellationToken)
    {
        if (file is null || file.Length == 0)
            return BadRequest("Nenhum arquivo foi enviado.");

        if (departamentoIds.Count == 0)
            return BadRequest("Selecione pelo menos um departamento.");

        var nomeArquivo = Path.GetFileName(file.FileName);

        var tipo = Path
            .GetExtension(nomeArquivo)
            .TrimStart('.')
            .ToLowerInvariant();

        await using var stream = file.OpenReadStream();

        var request = new AddArquivoRequest(
            nomeArquivo,
            tipo,
            file.Length,
            stream,
            departamentoIds
        );

        var result = await _sender.Send(
            request,
            cancellationToken
        );

        return Ok(result);
    }
}
