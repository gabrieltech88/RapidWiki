using RapidWiki.Application.Common.Dto;

namespace RapidWiki.Application.GetUsuarios;

public class GetUsuariosResult
{
    public IEnumerable<UsuarioDto> Items { get; init; } = [];
    public int Page { get; init; }
    public int PageSize { get; init; }
    public int TotalItems { get; init; }
}