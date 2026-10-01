using MediatR;

namespace RapidWiki.Application.GetArquivos;

public record GetArquivosRequest(
    int Page = 1,
    string? Search = null,
    Guid? DepartamentoId = null
) : IRequest<GetArquivosResult>;