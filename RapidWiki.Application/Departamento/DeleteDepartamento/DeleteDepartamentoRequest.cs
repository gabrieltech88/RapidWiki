using MediatR;

namespace RapidWiki.Application.DeleteDepartamento;

public record DeleteDepartamentoRequest(
    Guid Id
) : IRequest<Guid>;