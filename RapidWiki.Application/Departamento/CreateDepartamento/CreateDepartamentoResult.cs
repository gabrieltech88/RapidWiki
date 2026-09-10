namespace RapidWiki.Application.CreateDepartamento;

public record CreateDepartamentoResult
{
    public Guid Id { get; init; }
    public string Nome { get; init; }
}