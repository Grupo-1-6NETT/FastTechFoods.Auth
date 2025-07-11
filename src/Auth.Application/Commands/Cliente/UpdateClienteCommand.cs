using MediatR;

namespace Auth.Application.Commands.Cliente;
public record UpdateClienteCommand(string Nome, string Email, string Senha) : IRequest<bool>;

public class UpdateClienteCommandHandler() : IRequestHandler<UpdateClienteCommand, bool>
{
    public Task<bool> Handle(UpdateClienteCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}