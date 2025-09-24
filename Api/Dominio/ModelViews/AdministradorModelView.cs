using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MinimalApi.Dominio.Enuns;

namespace MinimalApi.Dominio.ModelViews;

public record AdministradorModelView
{
    public int Id { get; set; } = default; 
    public string Email { get; set; } = string.Empty; 
    public string Senha { get; set; } = string.Empty;   
    public Perfil Perfil { get; set; } = default; 
}