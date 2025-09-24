using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinimalApi.Dominio.DTOs;
using MinimalApi.Dominio.Servicos;
using MinimalApi.Dominio.Interfaces;
using MinimalApi.Infraestrutura.Db;
using MinimalApi.Dominio.Entidades;
using MinimalApi.Dominio.ModelViews;
using MinimalApi.Dominio.Enuns;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

#region Builder
var builder = WebApplication.CreateBuilder(args);

var Key = builder.Configuration.GetSection("Jwt").ToString();
if (string.IsNullOrEmpty(Key)) Key = "123456";

builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option =>
{
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key))
    };
});

builder.Services.AddAuthorization();

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
// LOGIN
string GerarTokenJwt(Administrador administrador)
{
    if (string.IsNullOrEmpty(Key)) return string.Empty;

    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

    var claims = new List<Claim>()
    {
        new Claim("Email", administrador.Email),
        new Claim("Perfil", administrador.Perfil.ToString()),
    };
    var token = new JwtSecurityToken(
        claims: claims,
        expires: DateTime.Now.AddDays(1),
        signingCredentials: credentials
    );

    return new JwtSecurityTokenHandler().WriteToken(token);
}

app.MapPost("/administradores/login", ([FromBody] LoginDTO loginDTO, IAdministradorServico administradorServico) =>
{
    var administrador = administradorServico.Login(loginDTO);
    if (administrador != null)
    {
        string token = GerarTokenJwt(administrador);
        return Results.Ok(new AdministradorLogado
        {
            Email = administrador.Email,
            Perfil = administrador.Perfil.ToString(),
            Token = token,
        });
    }
    return Results.Unauthorized();
}).WithTags("Administradores");

// CRIAR
app.MapPost("/administradores", ([FromBody] AdministradorDTO administradorDTO, IAdministradorServico administradorServico) =>
{
    var validacao = new ErrosDeValidacao { Mensagens = new List<string>() };

    if (string.IsNullOrEmpty(administradorDTO.Email))
        validacao.Mensagens.Add("E-mail obrigatório");
    if (string.IsNullOrEmpty(administradorDTO.Senha) || administradorDTO.Senha.Length < 6)
        validacao.Mensagens.Add("Senha deve ter 6+ caracteres");
    if (administradorDTO.Perfil == null)
        validacao.Mensagens.Add("Perfil obrigatório");

    if (validacao.Mensagens.Count > 0)
        return Results.BadRequest(validacao);

    var administrador = new Administrador
    {
        Email = administradorDTO.Email,
        Senha = administradorDTO.Senha,
        Perfil = administradorDTO.Perfil.Value  
    };

    administradorServico.Adicionar(administrador);
    return Results.Created($"/administradores/{administrador.Id}", administrador);
}).RequireAuthorization().WithTags("Administradores");

// LISTAR
app.MapGet("/administradores", ([FromQuery] int? pagina, IAdministradorServico administradorServico) =>
{
    var administradores = administradorServico.Todos(pagina);
    
    var administradoresModelView = administradores.Select(adm => new AdministradorModelView
    {
        Id = adm.Id,
        Email = adm.Email,
        Perfil = adm.Perfil
    }).ToList();

    return Results.Ok(administradoresModelView);
}).RequireAuthorization().WithTags("Administradores");

// BUSCAR POR ID
app.MapGet("/administradores/{id}", ([FromRoute] int id, IAdministradorServico administradorServico) =>
{
    var administrador = administradorServico.BuscarPorId(id);
    if (administrador == null)
        return Results.NotFound();

    var administradorModelView = new AdministradorModelView
    {
        Id = administrador.Id,
        Email = administrador.Email,
        Perfil = administrador.Perfil
    };

    return Results.Ok(administradorModelView);
}).RequireAuthorization().WithTags("Administradores");

// ATUALIZAR
app.MapPut("/administradores/{id}", ([FromRoute] int id, [FromBody] AdministradorDTO administradorDTO, IAdministradorServico administradorServico) =>
{
    var administrador = administradorServico.BuscarPorId(id);
    if (administrador == null) 
        return Results.NotFound();

    administrador.Email = administradorDTO.Email;
    administrador.Senha = administradorDTO.Senha;
    
    if (administradorDTO.Perfil.HasValue)
        administrador.Perfil = administradorDTO.Perfil.Value;

    administradorServico.Atualizar(administrador);
    
    return Results.Ok(administrador);
}).RequireAuthorization().WithTags("Administradores");

// DELETAR
app.MapDelete("/administradores/{id}", ([FromRoute] int id, IAdministradorServico administradorServico) =>
{
    var administrador = administradorServico.BuscarPorId(id);
    if (administrador == null) 
        return Results.NotFound();

    administradorServico.Deletar(administrador);
    return Results.NoContent();
}).RequireAuthorization().WithTags("Administradores");
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
}).RequireAuthorization().WithTags("Veiculos");

app.MapGet("/Veiculos", ([FromQuery] int? pagina, string? nome, string? marca, IVeiculosServico VeiculosServico) =>
{
    var veiculos = VeiculosServico.Todos(pagina, nome, marca);
    
    return Results.Ok(veiculos);
}).RequireAuthorization().WithTags("Veiculos");

app.MapGet("/Veiculo/{id}", ([FromRoute] int id, IVeiculosServico VeiculosServico) =>
{
    var veiculo = VeiculosServico.BuscaPorId(id);
    if (veiculo == null)
        return Results.NotFound();
    
    return Results.Ok(veiculo);
}).RequireAuthorization().WithTags("Veiculos");

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
}).RequireAuthorization().WithTags("Veiculos");

app.MapDelete("/Veiculo/{id}", ([FromRoute] int id, IVeiculosServico VeiculosServico) =>
{
    var veiculo = VeiculosServico.BuscaPorId(id);
    if (veiculo == null)
        return Results.NotFound();

    VeiculosServico.Deletar(veiculo);
    
    return Results.NoContent();
}).RequireAuthorization().WithTags("Veiculos");
#endregion

#region App
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
#endregion
