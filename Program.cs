using Microsoft.EntityFrameworkCore;
using MinimalApi.Models;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<Infraestrutura.Data.Db.DbContexto>(options =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("MySQL"), ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MySQL")));
});

var app = builder.Build();

app.MapGet("/", () => "Olá, Mundo!");

app.MapPost("/login", (LoginDTO loginDTO) =>
{
    if (loginDTO.Email == "adm@teste.com" && loginDTO.Senha == "123456")
    {
        return Results.Ok("Login com Secesso");
    }
    else
        return Results.Unauthorized();
});
app.Run();