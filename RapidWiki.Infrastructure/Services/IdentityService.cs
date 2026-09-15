using Microsoft.AspNetCore.Identity;
using RapidWiki.Application.Interfaces;


namespace RapidWiki.Infrastructure.Services;

public class IdentityService : IIdentityService
{
    private readonly UserManager<IdentityUser<Guid>> _userManager;

    public IdentityService(UserManager<IdentityUser<Guid>> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Guid> CreateUsuarioAsync(Guid id, string email, string password, string role)
    {
        var identityUser = new IdentityUser<Guid>
        {
            Id = id,
            UserName = email,
            Email = email
        };

        var result = await _userManager.CreateAsync(identityUser, password);

        if (!result.Succeeded)
        {
            throw new InvalidOperationException(
                string.Join(
                    ", ",
                    result.Errors.Select(x => x.Description)
                )
            );
        }

        var roleResult = await _userManager.AddToRoleAsync(identityUser, role);

        if (!roleResult.Succeeded)
        {
            throw new InvalidOperationException(
                string.Join(
                    ", ",
                    roleResult.Errors.Select(x => x.Description)
                )
            );
        }

        return identityUser.Id;
    }

}