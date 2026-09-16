using MediatR;
using RapidWiki.Application.Common.Dto;

namespace RapidWiki.Application.GetUsuarios;

public record GetUsuariosRequest(
    int Page,
    string? Search
): IRequest<GetUsuariosResult>;