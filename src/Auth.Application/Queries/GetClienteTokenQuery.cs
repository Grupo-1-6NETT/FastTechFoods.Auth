using Auth.Application.Services.Cryptography;
using Auth.Application.Services.TokenJwt;
using Auth.Core.Interfaces;
using MediatR;

namespace Auth.Application.Queries;
public record GetClienteTokenQuery(string Cpf, string Senha) : IRequest<string>;

public class GetClienteTokenQueryHandler : IRequestHandler<GetClienteTokenQuery, string>
{
    private readonly IClienteRepository _repo;
    private readonly ITokenService _tokenService;
    private readonly IEncrypter _encrypter;

    public GetClienteTokenQueryHandler(IClienteRepository repo, ITokenService tokenService, IEncrypter encrypter)
    {
        _repo = repo;
        _tokenService = tokenService;
        _encrypter = encrypter;
    }

    public async Task<string> Handle(GetClienteTokenQuery request, CancellationToken cancellationToken)
    {
        var senhaHash = _encrypter.Encrypt(request.Senha);
        var cliente = await _repo.GetByCpfAsync(request.Cpf, senhaHash);

        if (cliente == null)
            return string.Empty;

        return _tokenService.GerarToken(cliente.Id.ToString(), "Cliente");
    }
}
