using RapidWiki.Infrastructure;
using RapidWiki.Api.ExceptionHandlers;
using RapidWiki.Application.CreateUsuario;
using System.Text.Json.Serialization;
using RapidWiki.Api.Authentication;
using RapidWiki.Application.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration); 
builder.Services.AddAutoMapper( cfg => { }, AppDomain.CurrentDomain.GetAssemblies());

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

var app = builder.Build();

app.MapControllers();
app.UseExceptionHandler(_ => {});


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();



app.Run();
