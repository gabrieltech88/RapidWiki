using MediatR;

namespace RapidWiki.Application.CreateDepartamento;

public record CreateDepartamentoRequest : IRequest<CreateDepartamentoResult>
{
    required public string Nome { get; init; }
}