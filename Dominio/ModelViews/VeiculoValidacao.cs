using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using MinimalApi.Dominio.DTOs;

namespace MinimalApi.Dominio.ModelViews
{
    public class VeiculoValidacao : AbstractValidator<VeiculoDTO>
    {
        public VeiculoValidacao()
        {
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("Nome é obrigatório")
                .MaximumLength(150).WithMessage("Nome máximo 150 caracteres");

            RuleFor(x => x.Marca)
                .NotEmpty().WithMessage("Marca é obrigatória")
                .MaximumLength(100).WithMessage("Marca máximo 100 caracteres");

            RuleFor(x => x.Ano)
                .InclusiveBetween(1900, 2100).WithMessage("Ano deve estar entre 1900 e 2100");
        }
    }
}