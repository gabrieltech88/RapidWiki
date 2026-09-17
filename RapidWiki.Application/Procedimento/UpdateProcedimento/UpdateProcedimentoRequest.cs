using MediatR;
using RapidWiki.Domain.Enums;

namespace RapidWiki.Application.UpdateProcedimento;

public record UpdateProcedimentoRequest(
    Guid Id,
    string Titulo,
    string Descricao,
    List<Guid> DepartamentosIds,
    string Conteudo,
    StatusProcedimento Status
) : IRequest<Guid>;

