using Auth.Core.Interfaces;
using MediatR;

namespace Auth.Application.Commands.Cliente;
public record DeleteFuncionarioCommand(Guid Id): IRequest<bool>;

public class DeleteClienteCommandHandler(IClienteRepository repository, IPublisher mediator) : IRequestHandler<DeleteFuncionarioCommand, bool>
{
    public Task<bool> Handle(DeleteFuncionarioCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}