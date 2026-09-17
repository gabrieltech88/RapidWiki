using RapidWiki.Application.Common;
using RapidWiki.Application.Common.Dto;
using RapidWiki.Domain.Enums;

namespace RapidWiki.Application.GetProcedimentoById;

public record GetProcedimentoByIdResult
{
    required public Guid Id { get; init; }
    required public string Titulo { get; init; }
    required public AutorDto Autor { get; init; }
    required public string Descricao { get; init; }
    required public IReadOnlyCollection<DepartamentoDto> Departamentos { get; init; } = [];
    public DateTime CriadoEm { get;  init; }
    public DateTime AtualizadoEm { get;  init; }
    required public string Conteudo { get; init; }
    public StatusProcedimento Status { get; init; }
}