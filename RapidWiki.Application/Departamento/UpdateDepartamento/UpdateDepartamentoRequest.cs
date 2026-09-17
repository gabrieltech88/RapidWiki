using MediatR;

namespace RapidWiki.Application.UpdateDepartamento;

public record UpdateDepartamentoRequest(
    Guid Id,
    string Nome
) : IRequest<Guid>;