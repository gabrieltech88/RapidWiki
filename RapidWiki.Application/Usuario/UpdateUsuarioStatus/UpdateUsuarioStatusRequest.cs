using MediatR;

namespace RapidWiki.Application.UpdateUsuarioStatus;

public record UpdateUsuarioStatusRequest(
    Guid Id,
    bool Ativo
) : IRequest;