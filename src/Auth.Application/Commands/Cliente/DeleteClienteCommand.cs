using Auth.Core.Interfaces;
using Auth.Exception.CustomExceptions;
using MediatR;

namespace Auth.Application.Commands.Cliente;
public record DeleteClienteCommand(Guid Id): IRequest<bool>;

public class DeleteClienteCommandHandler : IRequestHandler<DeleteClienteCommand, bool>
{
    private readonly IClienteRepository _repo;
    private readonly IUnitOfWork _unit;

    public DeleteClienteCommandHandler(IClienteRepository repo, IUnitOfWork unit)
    {
        _repo = repo;
        _unit = unit;
    }

    public async Task<bool> Handle(DeleteClienteCommand request, CancellationToken cancellationToken)
    {
        var result = await _repo.DeleteAsync(request.Id);
        if (result == false)
            throw new NotFoundException();

        await _unit.CommitAsync();
        return result;
    }
}