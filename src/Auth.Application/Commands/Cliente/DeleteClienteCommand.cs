using Auth.Core.Interfaces;
using MediatR;

namespace Auth.Application.Commands.Cliente;
public record DeleteClienteCommand(Guid Id): IRequest<bool>;

public class DeleteClienteCommandHandler(IClienteRepository repository, IPublisher mediator) : IRequestHandler<DeleteClienteCommand, bool>
{
    public Task<bool> Handle(DeleteClienteCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}