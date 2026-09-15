namespace RapidWiki.Application.Common;

public record DepartamentoDto
{
    required public Guid Id { get; init; }
    required public string Nome { get; init; }
}