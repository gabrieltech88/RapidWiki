using MediatR;
using RapidWiki.Domain.Enums;

namespace RapidWiki.Application.CreateProcedimento;

public record CreateProcedimentoRequest(
    string Titulo, 
    string Descricao,
    List<Guid> DepartamentosIds,
    string Conteudo,
    StatusProcedimento Status
) : IRequest<Guid>;