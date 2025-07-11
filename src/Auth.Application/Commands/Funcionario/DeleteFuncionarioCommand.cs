using Auth.Core.Interfaces;
using MediatR;

namespace Auth.Application.Commands.Funcionario;
public record DeleteFuncionarioCommand(Guid Id): IRequest<bool>;

public class DeleteFuncionarioCommandHandler(IClienteRepository repository, IPublisher mediator) : IRequestHandler<DeleteFuncionarioCommand, bool>
{
    public Task<bool> Handle(DeleteFuncionarioCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}