using MinimalApi.Dominio.DTOs;
using MinimalApi.Dominio.Entidades;
using MinimalApi.Infraestrutura.Db;
using MinimalApi.Dominio.Interfaces;

namespace MinimalApi.Dominio.Servicos
{
    public class VeiculoServico : IVeiculosServico
    {
        private readonly DbContexto _contexto;
        public VeiculoServico(DbContexto contexto)
        {
            _contexto = contexto;
        }
        public void Deletar(Veiculo veiculo)
        {
            _contexto.Veiculos.Remove(veiculo);
            _contexto.SaveChanges();
            return veiculo;
        }
        public void Atualizar(Veiculo veiculo)
        {
            _contexto.Veiculos.Update(veiculo);
            _contexto.SaveChanges();
            return veiculo;
        }
        public Veiculo? BuscaPorId(int id)
        {
            return _contexto.Veiculos.where(v => v.Id == id).FirstOrDefault();
        }
        public void Adicionar(Veiculo veiculo)
        {
            _contexto.Veiculos.Add(veiculo);
            _contexto.SaveChanges();
        }
        public List<Veiculo> Todos(int pagina = 1, string nome? = null, string marca? = null)
        {
            var query = _contexto.Veiculos.AsQueryable();

            if (!string.IsNullOrEmpty(nome))
            {
                query = query.Where(v => v.Nome.ToLower().Contains(nome));
            }
            int ItensPorPagina = 10;
            query = query.Skip((pagina - 1) * ItensPorPagina).Take(ItensPorPagina);

            if (!string.IsNullOrEmpty(marca))
            {
                query = query.Where(v => v.Marca.Contains(marca));
            }

            int pageSize = 10;
            int skip = (pagina - 1) * pageSize;

            return query.Skip(skip).Take(pageSize).ToList();
        }
        
    }
}