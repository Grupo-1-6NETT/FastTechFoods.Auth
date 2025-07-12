using Auth.Application.Services.Cryptography;
using Auth.Application.Validators;
using Auth.Core.Entities;
using Auth.Core.Interfaces;
using Auth.Exception.CustomExceptions;
using Auth.Exception.ErrorMessages;
using MediatR;

namespace Auth.Application.Commands.Cliente;
public record AddClienteCommand(string Nome, string Cpf, string Email, string Senha) : IRequest<Guid>, IClienteCommand;

public class AddClienteCommandHandler : IRequestHandler<AddClienteCommand, Guid>
{
    private readonly IClienteRepository _repo;
    private readonly IUnitOfWork _unit;
    private readonly IEncrypter _encrypter;

    public AddClienteCommandHandler(IClienteRepository repo, IUnitOfWork unit, IEncrypter encrypter)
    {
        _repo = repo;
        _unit = unit;
        _encrypter = encrypter;
    }

    public async Task<Guid> Handle(AddClienteCommand request, CancellationToken cancellationToken)
    {
        await ValidateAsync(request);

        var senhaHash = _encrypter.Encrypt(request.Senha);
        var cliente = new ClienteEntity(request.Nome, request.Cpf, request.Email, senhaHash);
        var clienteAdicionado = await _repo.AddAsync(cliente);
        await _unit.CommitAsync();
        return clienteAdicionado!.Id;
    }

    private async Task ValidateAsync(AddClienteCommand request)
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
