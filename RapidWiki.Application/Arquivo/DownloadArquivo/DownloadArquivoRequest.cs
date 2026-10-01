using MediatR;

namespace RapidWiki.Application.DownloadArquivo;

public record DownloadArquivoRequest(
    Guid ArquivoId
) : IRequest<DownloadArquivoResult?>;