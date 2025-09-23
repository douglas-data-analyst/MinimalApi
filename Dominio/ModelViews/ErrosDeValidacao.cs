using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MinimalApi.Dominio.ModelViews;

public class ErrosDeValidacao 
{
    public List<string> Mensagens { get; set; } = new List<string>(); 
}