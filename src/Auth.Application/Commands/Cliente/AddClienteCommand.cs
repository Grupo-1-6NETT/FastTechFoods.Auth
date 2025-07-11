using Auth.Core.Interfaces;
using MediatR;

namespace Auth.Application.Commands.Cliente;
public record AddClienteCommand(string Nome, string Cpf, string Email, string Senha) : IRequest<Guid>;

public class AddClienteCommandHandler(IClienteRepository repository, IPublisher mediator) : IRequestHandler<AddClienteCommand, Guid>
{
    public Task<Guid> Handle(AddClienteCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
