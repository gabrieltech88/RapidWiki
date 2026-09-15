using RapidWiki.Application.CreateUsuario;
using RapidWiki.Application.SignIn;

namespace RapidWiki.Application.Interfaces;

public interface IIdentityService
{
    Task<Guid> CreateUsuarioAsync(Guid id, string email, string password, string role);
}
