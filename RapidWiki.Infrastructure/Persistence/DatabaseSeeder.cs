using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RapidWiki.Domain.Entities;

namespace RapidWiki.Infrastructure.Persistence;

public class DatabaseSeeder
{
    private readonly RapidWikiDbContext _context;
    private readonly UserManager<IdentityUser<Guid>> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly IConfiguration _configuration;

    public DatabaseSeeder(
        RapidWikiDbContext context,
        UserManager<IdentityUser<Guid>> userManager,
        RoleManager<IdentityRole<Guid>> roleManager,
        IConfiguration configuration)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
    }

    public async Task SeedAsync()
    {
        await SeedRolesAsync();
        await SeedAdminAsync();
    }

    private async Task SeedRolesAsync()
    {
        string[] roles =
        [
            "Admin",
            "Editor",
            "User"
        ];

        foreach (var role in roles)
        {
            if (await _roleManager.RoleExistsAsync(role))
            {
                continue;
            }

            var result = await _roleManager.CreateAsync(
                new IdentityRole<Guid>(role)
            );

            if (!result.Succeeded)
            {
                throw new InvalidOperationException(
                    $"Não foi possível criar a role {role}: {GetErrors(result)}"
                );
            }
        }
    }

    private async Task SeedAdminAsync()
    {
        var adminName = _configuration["Seed:AdminName"];
        var adminEmail = _configuration["Seed:AdminEmail"];

        if (string.IsNullOrWhiteSpace(adminName))
        {
            throw new InvalidOperationException(
                "A configuração Seed:AdminName não foi definida."
            );
        }

        if (string.IsNullOrWhiteSpace(adminEmail))
        {
            throw new InvalidOperationException(
                "A configuração Seed:AdminEmail não foi definida."
            );
        }

        var identityUser = await _userManager.FindByEmailAsync(adminEmail);

        if (identityUser is null)
        {
            var adminPassword = _configuration["Seed:AdminPassword"];

            if (string.IsNullOrWhiteSpace(adminPassword))
            {
                throw new InvalidOperationException(
                    "A variável de ambiente Seed__AdminPassword não foi definida."
                );
            }

            var id = Guid.NewGuid();

            identityUser = new IdentityUser<Guid>
            {
                Id = id,
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            var identityResult =
                await _userManager.CreateAsync(
                    identityUser,
                    adminPassword
                );

            if (!identityResult.Succeeded)
            {
                throw new InvalidOperationException(
                    $"Não foi possível criar o usuário administrador: {GetErrors(identityResult)}"
                );
            }

            var usuario = new Usuario(
                identityUser.Id,
                adminName
            );

            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();

            var roleResult =
                await _userManager.AddToRoleAsync(
                    identityUser,
                    "Admin"
                );

            if (!roleResult.Succeeded)
            {
                throw new InvalidOperationException(
                    $"Não foi possível atribuir a role Admin: {GetErrors(roleResult)}"
                );
            }

            await transaction.CommitAsync();

            return;
        }

        var usuarioExists =
            await _context.Usuarios.AnyAsync(
                usuario =>
                    usuario.Id == identityUser.Id
            );

        if (!usuarioExists)
        {
            var usuario = new Usuario(
                identityUser.Id,
                adminName
            );

            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();
        }

        if (!await _userManager.IsInRoleAsync(identityUser, "Admin"))
        {
            var roleResult =
                await _userManager.AddToRoleAsync(
                    identityUser,
                    "Admin"
                );

            if (!roleResult.Succeeded)
            {
                throw new InvalidOperationException(
                    $"Não foi possível atribuir a role Admin: {GetErrors(roleResult)}"
                );
            }
        }
    }

    private static string GetErrors(IdentityResult result)
    {
        return string.Join(
            "; ",
            result.Errors.Select(error => error.Description)
        );
    }
}