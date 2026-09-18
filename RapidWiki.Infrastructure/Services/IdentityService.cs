using Microsoft.AspNetCore.Identity;
using RapidWiki.Application.Common.Dto;
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
            Email = email,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(identityUser, password);

        if (!result.Succeeded)
        {
            throw new InvalidOperationException(
                string.Join(", ", result.Errors.Select(x => x.Description))
            );
        }

        var roleResult = await _userManager.AddToRoleAsync(identityUser, role);

        if (!roleResult.Succeeded)
        {
            throw new InvalidOperationException(
                string.Join(", ", roleResult.Errors.Select(x => x.Description))
            );
        }

        return identityUser.Id;
    }

    public async Task UpdateUsuarioAsync(Guid id, string email, string role)
    {
        var identityUser = await _userManager.FindByIdAsync(id.ToString());

        if (identityUser is null)
        {
            throw new KeyNotFoundException("Usuário não encontrado no Identity.");
        }

        identityUser.Email = email;
        identityUser.UserName = email;

        var updateResult = await _userManager.UpdateAsync(identityUser);

        if (!updateResult.Succeeded)
        {
            throw new InvalidOperationException(
                string.Join(", ", updateResult.Errors.Select(x => x.Description))
            );
        }

        var currentRoles = await _userManager.GetRolesAsync(identityUser);

        if (currentRoles.Count == 1 && currentRoles.Contains(role))
        {
            return;
        }

        if (!currentRoles.Contains(role))
        {
            var addRoleResult = await _userManager.AddToRoleAsync(identityUser, role);

            if (!addRoleResult.Succeeded)
            {
                throw new InvalidOperationException(
                    string.Join(", ", addRoleResult.Errors.Select(x => x.Description))
                );
            }
        }

        var rolesToRemove = currentRoles
            .Where(currentRole => currentRole != role)
            .ToList();

        if (rolesToRemove.Count > 0)
        {
            var removeRoleResult = await _userManager.RemoveFromRolesAsync(identityUser, rolesToRemove);

            if (!removeRoleResult.Succeeded)
            {
                throw new InvalidOperationException(
                    string.Join(", ", removeRoleResult.Errors.Select(x => x.Description))
                );
            }
        }
    }

    public async Task UpdateUsuarioStatusAsync(Guid id, bool ativo)
    {
        var identityUser = await _userManager.FindByIdAsync(id.ToString());

        if (identityUser is null)
        {
            throw new KeyNotFoundException("Usuário não encontrado no Identity.");
        }

        identityUser.EmailConfirmed = ativo;

        var result = await _userManager.UpdateAsync(identityUser);

        if (!result.Succeeded)
        {
            throw new InvalidOperationException(
                string.Join(", ", result.Errors.Select(x => x.Description))
            );
        }
    }

    public async Task ChangePasswordAsync(Guid id, string senhaAtual, string novaSenha)
    {
        var identityUser = await _userManager.FindByIdAsync(id.ToString());

        if (identityUser is null)
        {
            throw new KeyNotFoundException("Usuário não encontrado no Identity.");
        }

        var result = await _userManager.ChangePasswordAsync(
            identityUser,
            senhaAtual,
            novaSenha
        );

        if (!result.Succeeded)
        {
            var passwordMismatch = result.Errors.Any(error =>
                error.Code == "PasswordMismatch"
            );

            if (passwordMismatch)
            {
                throw new UnauthorizedAccessException("Senha atual incorreta.");
            }

            throw new InvalidOperationException(
                string.Join(", ", result.Errors.Select(x => x.Description))
            );
        }
    }

    public async Task<UsuarioIdentityDto?> GetUsuarioAsync(Guid id)
    {
        var identityUser = await _userManager.FindByIdAsync(id.ToString());

        if (identityUser is null)
        {
            return null;
        }

        var roles = await _userManager.GetRolesAsync(identityUser);

        return new UsuarioIdentityDto
        {
            Id = identityUser.Id,
            Email = identityUser.Email ?? string.Empty,
            Role = roles.FirstOrDefault() ?? string.Empty,
            Ativo = identityUser.EmailConfirmed
        };
    }
}