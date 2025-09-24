using MinimalApi.Dominio.DTOs;
using MinimalApi.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinimalApi.Dominio.Interfaces;

public interface IAdministradorServico
{
    Administrador? Login(LoginDTO loginDTO); 
    void Adicionar(Administrador administrador);
    Administrador? Atualizar(Administrador administrador);
    Administrador? Deletar(Administrador administrador);
    Administrador? BuscarPorId(int id);
    List<Administrador> Todos(int? pagina = 1);
}


