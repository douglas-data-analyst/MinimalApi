using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinimalApi.Dominio.DTOs;
using MinimalApi.Dominio.Servicos;
using MinimalApi.Dominio.Interfaces;
using MinimalApi.Infraestrutura.Db;
using MinimalApi.Dominio.Entidades;

#region Builder
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
#endregion

#region Home
app.MapGet("/", () => Results.Json(new MinimalApi.Dominio.ModelViews.Home())).WithTags("Home");
#endregion

#region Administradores
app.MapPost("/Administradores/login", ([FromBody] LoginDTO loginDTO, IAdministradorServico administradorServico) =>
{
    if (administradorServico.Login(loginDTO) != null)
    {
        return Results.Ok("Login com Secesso");
    }
    else
        return Results.Unauthorized();
}).WithTags("Administradores");
#endregion

#region Veiculos
app.MapPost("/Veiculos", ([FromBody] VeiculoDTO veiculoDTO, IVeiculosServico VeiculosServico) =>
{
    var veiculo = new Veiculo
    {
        Nome = veiculoDTO.Nome,
        Marca = veiculoDTO.Marca,
        Ano = veiculoDTO.Ano
    };
    VeiculosServico.Adicionar(veiculo);

    return Results.Created($"/Veiculos/{veiculo.Id}", veiculo);
}).WithTags("Veiculos");

app.MapGet("/Veiculos", ([FromQuery] int? pagina, string? nome, string? marca, IVeiculosServico VeiculosServico) =>
{
    var veiculos = VeiculosServico.Todos(pagina, nome, marca);
    
    return Results.Ok(veiculos);
}).WithTags("Veiculos");

app.MapGet("/Veiculo/{id}", ([FromRoute] int id, IVeiculosServico VeiculosServico) =>
{
    var veiculo = VeiculosServico.BuscaPorId(id);
    if (veiculo == null)
        return Results.NotFound();
    
    return Results.Ok(veiculo);
}).WithTags("Veiculos");

app.MapPut("/Veiculo/{id}", ([FromRoute] int id, VeiculoDTO veiculoDTO, IVeiculosServico VeiculosServico) =>
{
    var veiculo = VeiculosServico.BuscaPorId(id);
    if (veiculo == null)
        return Results.NotFound();

    veiculo.Nome = veiculoDTO.Nome;
    veiculo.Marca = veiculoDTO.Marca;
    veiculo.Ano = veiculoDTO.Ano;

    VeiculosServico.Atualizar(veiculo);
    
    return Results.Ok(veiculo);
}).WithTags("Veiculos");
#endregion

#region App
app.UseSwagger();
app.UseSwaggerUI();

app.Run();
#endregion
