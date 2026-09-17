using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace RapidWiki.Infrastructure.Persistence;

public static class DatabaseInitializer
{
    public static async Task InitializeDatabaseAsync(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();

        var context =
            scope.ServiceProvider
                .GetRequiredService<RapidWikiDbContext>();

        await context.Database.MigrateAsync();

        var seeder =
            scope.ServiceProvider
                .GetRequiredService<DatabaseSeeder>();

        await seeder.SeedAsync();
    }
}