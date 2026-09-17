namespace RapidWiki.Application.Common.Dto;

public record DepartamentoDto
{
    required public Guid Id { get; init; }
    required public string Nome { get; init; }
}