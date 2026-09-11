using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RapidWiki.Application.Interfaces;
using RapidWiki.Domain.Entities;
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
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.ExpireTimeSpan = TimeSpan.FromHours(3);
            options.SlidingExpiration = true;
        });

        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IRepository<Usuario>, UsuarioRepository>();
        services.AddScoped<IRepository<Departamento>, DepartamentoRepository>();
         services.AddScoped<IRepository<Procedimento>, ProcedimentoRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}



