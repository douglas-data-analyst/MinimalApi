using MinimalApi.Dominio.DTOs;
using MinimalApi.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinimalApi.Dominio.Interfaces
{
    public interface IVeiculosServico
    {
        List<Veiculo> Todos(int pagina = 1, string nome? = null, string marca? = null);
        Veiculo? BuscaPorId(int id);
        void Adicionar(Veiculo veiculo);
        void Atualizar(int id, Veiculo veiculo);
        void Deletar(Veiculo veiculo);
    }
}