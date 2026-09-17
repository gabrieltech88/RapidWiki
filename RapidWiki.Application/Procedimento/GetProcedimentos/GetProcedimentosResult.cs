using RapidWiki.Application.Common.Dto;

namespace RapidWiki.Application.GetProcedimentosRequest;

public record GetProcedimentosResult
{
    required public int Page { get; init; }
    required public int TotalItems { get; init; }
    required public IReadOnlyCollection<ProcedimentoDto> Items { get; init; } = [];
    required public int PageSize  { get; init; }
   
}