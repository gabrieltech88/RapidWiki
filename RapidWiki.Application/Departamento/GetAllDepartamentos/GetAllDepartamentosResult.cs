namespace RapidWiki.Application.GetAllDepartamentos;

public record GetAllDepartamentosResult
{
    required public Guid Id { get; init; }
    required public string Nome { get; init; }
}