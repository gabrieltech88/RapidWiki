namespace RapidWiki.Application.Common.Dto;

public class UsuarioIdentityDto
{
    public Guid Id { get; init; }
    public string Email { get; init; } = string.Empty;
    public bool Ativo { get; init; }
    public string Role { get; init; } = string.Empty;
}