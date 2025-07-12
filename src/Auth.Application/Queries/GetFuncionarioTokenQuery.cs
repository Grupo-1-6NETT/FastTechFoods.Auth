using Auth.Application.Services.Cryptography;
using Auth.Application.Services.TokenJwt;
using Auth.Core.Interfaces;
using MediatR;

namespace Auth.Application.Queries;
public record GetFuncionarioTokenQuery(string Email, string Senha) : IRequest<string>;
public class GetFuncionarioTokenQueryHandler : IRequestHandler<GetFuncionarioTokenQuery, string>
{
    private readonly IFuncionarioRepository _repo;
    private readonly ITokenService _tokenService;
    private readonly IEncrypter _encrypter;

    public GetFuncionarioTokenQueryHandler(
        IFuncionarioRepository repository,
        ITokenService tokenService,
        IEncrypter encrypter)
    {
        _repo = repository;
        _tokenService = tokenService;
        _encrypter = encrypter;
    }
    public async Task<string> Handle(GetFuncionarioTokenQuery request, CancellationToken cancellationToken)
    {
        var senhaHash = _encrypter.Encrypt(request.Senha);
        var funcionario = await _repo.GetAsync(request.Email, senhaHash);

        if (funcionario == null)
            return string.Empty;

        return _tokenService.GerarToken(funcionario.Id.ToString(), funcionario.Funcao.ToString());
    }
}
