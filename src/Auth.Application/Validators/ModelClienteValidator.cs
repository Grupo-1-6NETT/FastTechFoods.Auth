using Auth.Application.Commands.Cliente;
using Auth.Exception.ErrorMessages;
using FluentValidation;

namespace Auth.Application.Validators;
public class ModelClienteValidator : AbstractValidator<IClienteCommand>
{
    public ModelClienteValidator()
    {
        RuleFor(c => c.Nome).NotEmpty().WithMessage(ResourceErrorMessages.NAME_EMPTY);
        RuleFor(c => c.Email).NotEmpty().WithMessage(ResourceErrorMessages.EMAIL_EMPTY);
        RuleFor(c => c.Cpf).Must(BeAValidCpf).WithMessage(ResourceErrorMessages.CPF_INVALID);
        RuleFor(c => c.Senha.Length).GreaterThanOrEqualTo(6).WithMessage(ResourceErrorMessages.PASSWORD_INVALID);
        When(u => !string.IsNullOrEmpty(u.Email), () =>
        {
            RuleFor(u => u.Email).EmailAddress().WithMessage(ResourceErrorMessages.EMAIL_INVALID);
        });
    }

    private bool BeAValidCpf(string cpf)
    {
        // Remove caracteres não numéricos
        cpf = new string(cpf.Where(char.IsDigit).ToArray());

        // Verifica tamanho
        if (cpf.Length != 11)
            return false;

        // Verifica se todos os dígitos são iguais
        if (cpf.All(c => c == cpf[0]))
            return false;

        // Cálculo do primeiro dígito verificador
        var sum = 0;
        for (int i = 0; i < 9; i++)
            sum += (10 - i) * (cpf[i] - '0');

        var remainder = sum % 11;
        var digit1 = remainder < 2 ? 0 : 11 - remainder;

        if (cpf[9] - '0' != digit1)
            return false;

        // Cálculo do segundo dígito verificador
        sum = 0;
        for (int i = 0; i < 10; i++)
            sum += (11 - i) * (cpf[i] - '0');

        remainder = sum % 11;
        var digit2 = remainder < 2 ? 0 : 11 - remainder;

        return cpf[10] - '0' == digit2;
    }
}
