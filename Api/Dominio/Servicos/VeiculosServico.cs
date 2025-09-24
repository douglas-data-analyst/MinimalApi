using MinimalApi.Dominio.DTOs;
using MinimalApi.Dominio.Entidades;
using MinimalApi.Infraestrutura.Db;
using MinimalApi.Dominio.Interfaces;
using Microsoft.EntityFrameworkCore; 

namespace MinimalApi.Dominio.Servicos;

public class VeiculosServico : IVeiculosServico
{
    private readonly DbContexto _contexto;
    public VeiculosServico(DbContexto contexto)
    {
        _contexto = contexto;
    }

    public Veiculo? BuscaPorId(int id) 
    {
        return _contexto.Veiculos.Find(id);
    }

    public Veiculo? Deletar(Veiculo veiculo) 
    {
        _contexto.Veiculos.Remove(veiculo);
        _contexto.SaveChanges();
        return veiculo;
    }

    public Veiculo? Atualizar(Veiculo veiculo) 
    {
        _contexto.Veiculos.Update(veiculo);
        _contexto.SaveChanges();
        return veiculo;
    }

    public void Adicionar(Veiculo veiculo)
    {
        _contexto.Veiculos.Add(veiculo);
        _contexto.SaveChanges();
    }

    public List<Veiculo> Todos(int? pagina = 1, string? nome = null, string? marca = null) 
    {
        var query = _contexto.Veiculos.AsQueryable();

        if (!string.IsNullOrEmpty(nome))
        {
            query = query.Where(v => v.Nome.ToLower().Contains(nome.ToLower())); 
        }
        
        if (!string.IsNullOrEmpty(marca))
        {
            query = query.Where(v => v.Marca.ToLower().Contains(marca.ToLower())); 
        }

        int itemporpagina = 10; 

        if(pagina != null)
            query = query.Skip(((int)pagina - 1) * itemporpagina).Take(itemporpagina);

        return query.ToList();   
    }
}