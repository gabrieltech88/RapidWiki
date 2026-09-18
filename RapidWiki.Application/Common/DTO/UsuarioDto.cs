namespace RapidWiki.Application.Common.Dto;

public class UsuarioDto
{
    public Guid Id { get; init; }
    public string Nome { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Role { get; init; } = string.Empty;
    public bool Ativo { get; init; }
    public IReadOnlyCollection<DepartamentoDto> Departamentos { get; init; } = [];
}