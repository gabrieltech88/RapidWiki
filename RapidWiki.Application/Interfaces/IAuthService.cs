using RapidWiki.Application.SignIn;

namespace RapidWiki.Application.Interfaces;

public interface IAuthService
{
    Task<SignInResult> SignInAsync(SignInRequest request);

    Task SignOutAsync();
}