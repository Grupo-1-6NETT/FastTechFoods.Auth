using Auth.Application.Commands.Funcionario;
using Auth.Exception.ErrorMessages;
using FluentValidation;

namespace Auth.Application.Validators;
public class ModelFuncionarioValidator : AbstractValidator<AddFuncionarioCommand>
{    public ModelFuncionarioValidator()
    {
        RuleFor(f => f.Nome).NotEmpty().WithMessage(ResourceErrorMessages.NAME_EMPTY);
        RuleFor(f => f.Email).NotEmpty().WithMessage(ResourceErrorMessages.EMAIL_EMPTY);
        RuleFor(f => f.funcao).IsInEnum().WithMessage(ResourceErrorMessages.INVALID_ROLE);
        RuleFor(f => f.Senha.Length).GreaterThanOrEqualTo(6).WithMessage(ResourceErrorMessages.PASSWORD_INVALID);
        When(f => !string.IsNullOrEmpty(f.Email), () =>
        {
            RuleFor(f => f.Email).EmailAddress().WithMessage(ResourceErrorMessages.EMAIL_INVALID);
        });
    }
}
