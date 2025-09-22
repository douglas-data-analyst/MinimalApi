using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinimalApi.Models.Entidades
{
    public class Administrador
    {
        public int Id { get; set; } = default!;
        public string Email { get; set; } = default!;
        public int Senha { get; set; } = default!;
        public string Perfil { get; set; } = default!;
    }
}