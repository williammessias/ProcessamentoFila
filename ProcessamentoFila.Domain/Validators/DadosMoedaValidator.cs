using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using ProcessamentoFila.Domain.DTO;

namespace ProcessamentoFila.Domain.Validators
{
    public class DadosMoedaValidator : AbstractValidator<List<MoedaDto>>
    {
        public DadosMoedaValidator() 
        {
            RuleForEach(x => x)
                .NotNull().NotEmpty().WithMessage("As moedas precisam ser informadas");

            RuleForEach(x => x.Select(y => y.Data_Inicio)).NotEmpty().WithMessage("Informe a data de inicio")
                .OverridePropertyName("data_inicio");
            RuleForEach(x => x.Select(y => y.Data_Fim))
                .NotEmpty().WithMessage("Informe a data fim")
                .OverridePropertyName("data_fim");

            RuleForEach(x => x.Select(y => y.Moeda))
                .NotEmpty().WithMessage("Informe a moeda")
                .Length(3).WithMessage("O tamanho da moeda não pode exceder 3 caracteres")
                .OverridePropertyName("moeda");

        }
    }   
}
