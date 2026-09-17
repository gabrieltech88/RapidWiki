using RapidWiki.Application.Commmon.Dto;

namespace RapidWiki.Application.GetRascunhos;

public record GetRascunhosResult
{
    public required int Page { get; init; }
    public required int PageSize { get; init; }
    public required int TotalItems { get; init; }
    public required IReadOnlyCollection<RascunhoDto> Items { get; init; } = [];
}