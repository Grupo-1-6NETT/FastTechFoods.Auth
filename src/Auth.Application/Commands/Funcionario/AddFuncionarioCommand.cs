using Auth.Core.Interfaces;
using MediatR;

namespace Auth.Application.Commands.Funcionario;
public record AddFuncionarioCommand(string Nome, string Cpf, string Email, string Senha) : IRequest<Guid>;

public class AddFuncionarioCommandHandler(IClienteRepository repository, IPublisher mediator) : IRequestHandler<AddFuncionarioCommand, Guid>
{
    public Task<Guid> Handle(AddFuncionarioCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
