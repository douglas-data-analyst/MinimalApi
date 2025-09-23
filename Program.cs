using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinimalApi.Dominio.DTOs;
using MinimalApi.Dominio.Servicos;
using MinimalApi.Dominio.Interfaces;
using MinimalApi.Infraestrutura.Db;

var builder = WebApplication.CreateBuilder(args);

IServiceCollection serviceCollection = builder.Services.AddScoped<IAdministradorServico, AdministradorServico>();
builder.Services.AddScoped<IVeiculosServico, VeiculosServico>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MinimalApi.Infraestrutura.Db.DbContexto>(options =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("MySQL"), ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MySQL")));
});

var app = builder.Build();

app.MapGet("/", () => Results.Json(new MinimalApi.Dominio.ModelViews.Home()));

app.MapPost("/login", ([FromBody] LoginDTO loginDTO, IAdministradorServico administradorServico) =>
{
    if (administradorServico.Login(loginDTO) != null)
    {
        return Results.Ok("Login com Secesso");
    }
    else
        return Results.Unauthorized();
});

app.UseSwagger();
app.UseSwaggerUI();

app.Run();


