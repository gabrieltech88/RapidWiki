using MediatR;
using RapidWiki.Domain.Enums;

namespace RapidWiki.Application.GetProcedimentosRequest;

public record GetProcedimentosRequest(
    int Page,
    string? Search,
    Guid? DepartamentoId,
    StatusProcedimento? Status
) : IRequest<GetProcedimentosResult>;