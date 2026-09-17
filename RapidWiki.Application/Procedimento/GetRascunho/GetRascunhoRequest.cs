using MediatR;

namespace RapidWiki.Application.GetRascunhos;

public record GetRascunhosRequest(
    int Page,
    string? Search
) : IRequest<GetRascunhosResult>;