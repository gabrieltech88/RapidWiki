using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RapidWiki.Application.Interfaces;
using RapidWiki.Infrastructure.Persistence;
using RapidWiki.Infrastructure.Persistence.Repositories;
using RapidWiki.Infrastructure.Services;

namespace RapidWiki.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString =
            configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<RapidWikiDbContext>(options =>
            options.UseMySql(
                connectionString,
                ServerVersion.AutoDetect(connectionString)));

        services.AddIdentity<IdentityUser<Guid>, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<RapidWikiDbContext>()
            .AddDefaultTokenProviders();

        services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.Name = "RapidWiki.Auth";
            options.Cookie.HttpOnly = true;

            //options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            //options.Cookie.SameSite = SameSiteMode.Lax;

            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.Cookie.SameSite = SameSiteMode.None;

            options.ExpireTimeSpan = TimeSpan.FromHours(3);
        });

        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IImageStorageService, ImageStorageService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IAuthService, AuthenticationService>();

        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        services.AddScoped<IDepartamentoRepository, DepartamentoRepository>();
        services.AddScoped<IProcedimentoRepository, ProcedimentoRepository>();
        services.AddScoped<IProcedimentoRascunhoRepository, ProcedimentoRascunhoRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<DatabaseSeeder>();

        return services;
    }
}



