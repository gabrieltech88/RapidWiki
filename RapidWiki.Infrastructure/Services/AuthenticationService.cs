using Microsoft.AspNetCore.Identity;
using RapidWiki.Application.Interfaces;
using RapidWiki.Application.SignIn;
using SignInResult = RapidWiki.Application.SignIn.SignInResult;

namespace RapidWiki.Infrastructure.Services;

public class AuthenticationService : IAuthService
{
    private readonly UserManager<IdentityUser<Guid>> _userManager;
    private readonly SignInManager<IdentityUser<Guid>> _signInManager;
    public AuthenticationService(UserManager<IdentityUser<Guid>> userManager, SignInManager<IdentityUser<Guid>> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<SignInResult> SignInAsync(SignInRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null)
        {
            throw new UnauthorizedAccessException("Email ou senha inválidos.");
        }

        var result = await _signInManager.PasswordSignInAsync(user, request.Password, false, false);

        if (!result.Succeeded)
        {
            throw new UnauthorizedAccessException(
                "Email ou senha inválidos."
            );
        }

        var roles = await _userManager.GetRolesAsync(user);

        return new SignInResult
        {
            Id = user.Id,

            Role =
                roles.SingleOrDefault()
                ?? throw new InvalidOperationException(
                    "Usuário não possui uma role."
                )
        };
    }

    public async Task SignOutAsync()
    {
        await _signInManager.SignOutAsync();
    }
    
}