using MediatR;

namespace RapidWiki.Application.UpdateUsuario;

public record UpdateUsuarioRequest(
    Guid Id,
    string Nome,
    string Email,
    string Role,
    List<Guid> DepartamentosIds
) : IRequest<Guid>;