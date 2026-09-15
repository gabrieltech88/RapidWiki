using Microsoft.AspNetCore.Identity;
using RapidWiki.Application.Interfaces;

namespace RapidWiki.Infrastructure.Services;

public class RoleService : IRoleService
{
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;

    public RoleService(RoleManager<IdentityRole<Guid>> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<Guid> CreateAsync(string roleName)
    {
        if (string.IsNullOrWhiteSpace(roleName))
        {
            throw new ArgumentException("Role name cannot be empty.", nameof(roleName));
        }

        var role = new IdentityRole<Guid>
        {
            Id = Guid.NewGuid(),
            Name = roleName
        };

        var result = await _roleManager.CreateAsync(role);

        if (!result.Succeeded)
        {
            throw new InvalidOperationException(string.Join(", ", result.Errors.Select(x => x.Description)));
        }

        return role.Id;
    }
}