using RapidWiki.Application.CreateUsuario;
using RapidWiki.Application.SignIn;

namespace RapidWiki.Application.Interfaces
{
    public interface IIdentityService
    {
        Task<Guid> CreateUsuarioAsync(CreateUsuarioRequest request);
        Task<Guid> CreateRoleAsync(string roleName);
        Task<SignInResult> SignInAsync(SignInRequest request);
        
    }
}