using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinimalApi.Dominio.DTOs;
using MinimalApi.Dominio.Servicos;
using MinimalApi.Dominio.Interfaces;
using MinimalApi.Infraestrutura.Db;
using MinimalApi.Dominio.Entidades;
using MinimalApi.Dominio.ModelViews;

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
ErrosDeValidacao validaDTO(VeiculoDTO veiculoDTO)
{
    var validacao = new ErrosDeValidacao();
    validacao.Mensagens = new List<string>();

    if (string.IsNullOrEmpty(veiculoDTO.Nome))
        validacao.Mensagens.Add("O nome do veiculo é obrigatório");
    if (string.IsNullOrEmpty(veiculoDTO.Marca))
        validacao.Mensagens.Add("A marca do veiculo é obrigatória");
    if (veiculoDTO.Ano < 1900 || veiculoDTO.Ano > DateTime.Now.Year)
        validacao.Mensagens.Add("O ano do veiculo é inválido");

    return validacao;
}
app.MapPost("/Veiculos", ([FromBody] VeiculoDTO veiculoDTO, IVeiculosServico VeiculosServico) =>
{
    var validacao = validaDTO(veiculoDTO);

    if (validacao.Mensagens.Count > 0)
        return Results.BadRequest(validacao);

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

    var validacao = validaDTO(veiculoDTO);

    if (validacao.Mensagens.Count > 0)
        return Results.BadRequest(validacao);

    veiculo.Nome = veiculoDTO.Nome;
    veiculo.Marca = veiculoDTO.Marca;
    veiculo.Ano = veiculoDTO.Ano;

    VeiculosServico.Atualizar(veiculo);
    
    return Results.Ok(veiculo);
}).WithTags("Veiculos");

app.MapDelete("/Veiculo/{id}", ([FromRoute] int id, IVeiculosServico VeiculosServico) =>
{
    var veiculo = VeiculosServico.BuscaPorId(id);
    if (veiculo == null)
        return Results.NotFound();

    VeiculosServico.Deletar(veiculo);
    
    return Results.NoContent();
}).WithTags("Veiculos");
#endregion

#region App
app.UseSwagger();
app.UseSwaggerUI();

app.Run();
#endregion
