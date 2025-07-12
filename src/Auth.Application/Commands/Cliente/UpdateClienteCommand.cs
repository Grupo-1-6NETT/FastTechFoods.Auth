using Auth.Application.Services;
using Auth.Application.Validators;
using Auth.Core.Interfaces;
using Auth.Exception.CustomExceptions;
using Auth.Exception.ErrorMessages;
using MediatR;

namespace Auth.Application.Commands.Cliente;
public record UpdateClienteCommand(string Nome, string Cpf, string Email, string Senha) : IRequest<bool>, IClienteCommand;

public class UpdateClienteCommandHandler : IRequestHandler<UpdateClienteCommand, bool>
{
    private readonly IClienteRepository _repo;
    private readonly IUnitOfWork _unit;
    private readonly IEncrypter _encrypter;


    public UpdateClienteCommandHandler(IClienteRepository repo, IUnitOfWork unit, IEncrypter encrypter)
    {
        _repo = repo;
        _unit = unit;
        _encrypter = encrypter;
    }
    public async Task<bool> Handle(UpdateClienteCommand request, CancellationToken cancellationToken)
    {
        await ValidateAsync(request);
        var senhaHash = _encrypter.Encrypt(request.Senha);
        var cliente = await _repo.GetByCpfAsync(request.Cpf, senhaHash);
        if (cliente == null)
            return false;

        var clienteAtualizado = await _repo.UpdateAsync(request.Cpf, request.Nome, request.Email);
        if (clienteAtualizado == null)
            return false;

        await _unit.CommitAsync();
        return true;
    }
    private async Task ValidateAsync(UpdateClienteCommand request)
    {
        var validator = new ModelClienteValidator();
        var result = validator.Validate(request);
        var clienteExiste = await _repo.ClienteExisteAsync(request.Email);
        if (clienteExiste)
            result.Errors.Add(new FluentValidation.Results.ValidationFailure("", ResourceErrorMessages.EMAIL_ALREADY_REGISTERED));

        if (result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessages);
        }
    }
}