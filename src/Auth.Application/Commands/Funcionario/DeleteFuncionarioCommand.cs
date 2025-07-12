using Auth.Core.Interfaces;
using Auth.Exception.CustomExceptions;
using MediatR;

namespace Auth.Application.Commands.Funcionario;
public record DeleteFuncionarioCommand(Guid Id): IRequest<bool>;

public class DeleteFuncionarioCommandHandler : IRequestHandler<DeleteFuncionarioCommand, bool>
{
    private readonly IFuncionarioRepository _repo;
    private readonly IUnitOfWork _unit;

    public DeleteFuncionarioCommandHandler(IFuncionarioRepository repo, IUnitOfWork unit)
    {
        _repo = repo;
        _unit = unit;
    }

    public async Task<bool> Handle(DeleteFuncionarioCommand request, CancellationToken cancellationToken)
    {
        var result = await _repo.DeleteAsync(request.Id);
        if (result == false)
            throw new NotFoundException();

        await _unit.CommitAsync();
        return result;
    }
}