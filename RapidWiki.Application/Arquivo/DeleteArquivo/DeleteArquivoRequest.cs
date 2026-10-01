using MediatR;

namespace RapidWiki.Application.DeleteArquivo;

public record DeleteArquivoRequest(
    Guid ArquivoId
) : IRequest<bool>;