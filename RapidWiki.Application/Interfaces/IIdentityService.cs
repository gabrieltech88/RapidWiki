using RapidWiki.Application.Common.Dto;
using RapidWiki.Application.CreateUsuario;
using RapidWiki.Application.SignIn;

namespace RapidWiki.Application.Interfaces;

public interface IIdentityService
{
    Task<Guid> CreateUsuarioAsync(Guid id, string email, string password, string role);
    Task UpdateUsuarioAsync(Guid id, string email, string role);
    Task<UsuarioIdentityDto?> GetUsuarioAsync(Guid id);
    Task UpdateUsuarioStatusAsync(Guid id, bool ativo);
    Task ChangePasswordAsync(Guid id, string senhaAtual, string novaSenha);
}
