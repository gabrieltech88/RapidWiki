using RapidWiki.Domain.Enums;

namespace RapidWiki.Application.Common.Dto;

public record ProcedimentoDto
{
    required public string Titulo { get; init; }
    required public Guid Id { get; init; }
    required public string Conteudo { get; init; }
    required public string Descricao { get; init; }
    required public AutorDto Autor  { get; init; }
    required public DateTime AtualizadoEm { get; init; }
    required public StatusProcedimento Status { get; init; }
}