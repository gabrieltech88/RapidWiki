using RapidWiki.Infrastructure;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration); 
builder.Services.AddAutoMapper( cfg => { }, AppDomain.CurrentDomain.GetAssemblies());


builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();



app.Run();
