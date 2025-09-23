using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using MinimalApi.Dominio.DTOs;

namespace MinimalApi.Dominio.ModelViews
{
    public class AdministradorValidacao : AbstractValidator<AdministradorDTO>
    {
        public AdministradorValidacao()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email é obrigatório")
                .EmailAddress().WithMessage("Email em formato inválido")
                .MaximumLength(255).WithMessage("Email máximo 255 caracteres");

            RuleFor(x => x.Senha)
                .NotEmpty().WithMessage("Senha é obrigatória")
                .MinimumLength(6).WithMessage("Senha mínimo 6 caracteres")
                .MaximumLength(50).WithMessage("Senha máximo 50 caracteres");

            RuleFor(x => x.Perfil)
                .IsInEnum().WithMessage("Perfil inválido");
        }
    }
}