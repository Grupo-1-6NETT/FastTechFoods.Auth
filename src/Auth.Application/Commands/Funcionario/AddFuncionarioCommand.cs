using Auth.Application.Services.Cryptography;
using Auth.Application.Validators;
using Auth.Core.Entities;
using Auth.Core.Enums;
using Auth.Core.Interfaces;
using Auth.Exception.CustomExceptions;
using Auth.Exception.ErrorMessages;
using MediatR;

namespace Auth.Application.Commands.Funcionario;
public record AddFuncionarioCommand(string Nome, string Email, string Senha, FuncionarioFuncao funcao) : IRequest<Guid>;

public class AddFuncionarioCommandHandler : IRequestHandler<AddFuncionarioCommand, Guid>
{
    private readonly IFuncionarioRepository _repo;
    private readonly IUnitOfWork _unit;
    private readonly IEncrypter _encrypter;

    public AddFuncionarioCommandHandler(IFuncionarioRepository repo, IUnitOfWork unit, IEncrypter encrypter)
    {
        _repo = repo;
        _unit = unit;
        _encrypter = encrypter;
    }

    public async Task<Guid> Handle(AddFuncionarioCommand request, CancellationToken cancellationToken)
    {
        await ValidateAsync(request);

        var senhaHash = _encrypter.Encrypt(request.Senha);
        var funcionario = new FuncionarioEntity(request.Nome, request.Email, senhaHash, request.funcao);
        var funcionarioAdicionado = await _repo.AddAsync(funcionario);
        await _unit.CommitAsync();
        return funcionarioAdicionado!.Id;
    }
    private async Task ValidateAsync(AddFuncionarioCommand request)
    {
        var validator = new ModelFuncionarioValidator();
        var result = validator.Validate(request);
        var funcionarioExiste = await _repo.FuncionarioExisteAsync(request.Email);
        if (funcionarioExiste)
            result.Errors.Add(new FluentValidation.Results.ValidationFailure("", ResourceErrorMessages.EMAIL_ALREADY_REGISTERED));

        if (result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
