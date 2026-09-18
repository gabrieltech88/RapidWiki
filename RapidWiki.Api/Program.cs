
using System.Text.Json.Serialization;
using RapidWiki.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using RapidWiki.Api.Authentication;
using RapidWiki.Api.ExceptionHandlers;
using RapidWiki.Application.CreateUsuario;
using RapidWiki.Application.Interfaces;
using RapidWiki.Infrastructure;
using RapidWiki.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration); 
builder.Services.AddAutoMapper( cfg => { }, AppDomain.CurrentDomain.GetAssemblies());

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8018);
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(CreateUsuarioHandler).Assembly);
});

builder.Services.AddExceptionHandler<ArgumentNullExceptionHandler>();
builder.Services.AddExceptionHandler<UnauthorizedAccessExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddScoped<ICurrentUser, CurrentUser>();

builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173", "https://rapidwiki.backbonne.com.br")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

app.MapControllers();
app.UseExceptionHandler(_ => {});

app.UseHttpsRedirection();

app.UseCors("Frontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await using (var scope = app.Services.CreateAsyncScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<RapidWikiDbContext>();
    await dbContext.Database.MigrateAsync();

    var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
    await seeder.SeedAsync();
}

app.Run();
