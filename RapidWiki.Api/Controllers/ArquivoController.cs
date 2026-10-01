using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RapidWiki.Application.AddArquivo;
using RapidWiki.Application.DeleteArquivo;
using RapidWiki.Application.DownloadArquivo;
using RapidWiki.Application.GetArquivos;
using RapidWiki.Application.Interfaces;

namespace RapidWiki.Api.Controllers;

[ApiController]
[Authorize]
[Route("app/v1/arquivo")]
public class ArquivoController : ControllerBase
{
    private const long MaxImageSize = 5L * 1024 * 1024;
    private const long MaxFileSize = 50L * 1024 * 1024;

    private static readonly HashSet<string> AllowedImageContentTypes =
        new(StringComparer.OrdinalIgnoreCase)
        {
            "image/jpeg",
            "image/png",
            "image/webp",
            "image/gif"
        };

    private static readonly HashSet<string> AllowedFileExtensions =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ".txt",
            ".cfg",
            ".conf",
            ".ini",
            ".log",
            ".json",
            ".xml",
            ".yaml",
            ".yml",
            ".csv",
            ".pdf",
            ".docx",
            ".xlsx"
        };

    private readonly ISender _sender;
    private readonly IImageStorageService _imageStorageService;

    public ArquivoController(
        IImageStorageService imageStorageService,
        ISender sender)
    {
        _imageStorageService = imageStorageService;
        _sender = sender;
    }

    [HttpPost("upload_image")]
    public async Task<IActionResult> UploadImage(
        [FromForm] IFormFile file,
        CancellationToken cancellationToken)
    {
        if (file is null || file.Length == 0)
            return BadRequest("Nenhuma imagem foi enviada.");

        if (file.Length > MaxImageSize)
            return BadRequest("A imagem deve possuir no máximo 5 MB.");

        if (!AllowedImageContentTypes.Contains(file.ContentType))
            return BadRequest("Formato de imagem não permitido.");

        await using var stream = file.OpenReadStream();

        var fileName = await _imageStorageService.SaveAsync(
            stream,
            file.ContentType,
            cancellationToken
        );

        var url =
            $"{Request.Scheme}://{Request.Host}/app/v1/arquivo/image/{Uri.EscapeDataString(fileName)}";

        return Ok(new { url });
    }

    [HttpGet("image/{fileName}")]
    public async Task<IActionResult> GetImage(
        [FromRoute] string fileName,
        CancellationToken cancellationToken)
    {
        var image = await _imageStorageService.OpenReadAsync(
            fileName,
            cancellationToken
        );

        if (image is null)
            return NotFound();

        return File(
            image.Value.Stream,
            image.Value.ContentType,
            enableRangeProcessing: true
        );
    }

    [HttpPost("upload_arquivo")]
    [RequestSizeLimit(60L * 1024 * 1024)]
    public async Task<IActionResult> UploadArquivo(
        [FromForm] IFormFile file,
        [FromForm] List<Guid> departamentoIds,
        CancellationToken cancellationToken)
    {
        if (file is null || file.Length == 0)
            return BadRequest("Nenhum arquivo foi enviado.");

        if (file.Length > MaxFileSize)
            return BadRequest("O arquivo deve possuir no máximo 50 MB.");

        if (departamentoIds.Count == 0)
            return BadRequest("Selecione pelo menos um departamento.");

        var nomeArquivo = Path.GetFileName(file.FileName);

        var extensao = Path.GetExtension(nomeArquivo);

        if (
            string.IsNullOrWhiteSpace(extensao) ||
            !AllowedFileExtensions.Contains(extensao)
        )
        {
            return BadRequest("Formato de arquivo não permitido.");
        }

        var tipo = extensao
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

    [HttpGet("get_arquivos")]
    public async Task<IActionResult> GetArquivos([FromQuery] int page = 1, [FromQuery] string? search = null, [FromQuery] Guid? departamentoId = null, CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetArquivosRequest(
                    page,
                    search,
                    departamentoId
                ),
                cancellationToken
            );

        return Ok(result);
    }

    [HttpGet("download_arquivo/{arquivoId:guid}")]
    public async Task<IActionResult> DownloadArquivo([FromRoute] Guid arquivoId, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new DownloadArquivoRequest(arquivoId), cancellationToken);

        if (result is null)
            return NotFound();

        return File(
            result.Stream,
            "application/octet-stream",
            result.Nome,
            enableRangeProcessing: true
        );
    }

    [HttpDelete("delete_arquivo/{arquivoId:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteArquivo(
    [FromRoute] Guid arquivoId,
    CancellationToken cancellationToken)
    {
        var deleted =
            await _sender.Send(
                new DeleteArquivoRequest(
                    arquivoId
                ),
                cancellationToken
            );

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}