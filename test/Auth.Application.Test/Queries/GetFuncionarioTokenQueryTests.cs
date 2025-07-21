using Auth.Application.Queries;
using Auth.Application.Services.Cryptography;
using Auth.Application.Services.TokenJwt;
using Auth.Core.Entities;
using Auth.Core.Enums;
using Auth.Core.Interfaces;
using Moq;

namespace Auth.Application.Test.Queries;
public class GetFuncionarioTokenQueryTests
{
    private readonly Mock<IFuncionarioRepository> _repositoryMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly Mock<IEncrypter> _encrypterMock;

    private readonly GetFuncionarioTokenQueryHandler _sut;

    public GetFuncionarioTokenQueryTests()
    {
        _repositoryMock = new Mock<IFuncionarioRepository>();
        _tokenServiceMock = new Mock<ITokenService>();
        _encrypterMock = new Mock<IEncrypter>();
        _sut = new GetFuncionarioTokenQueryHandler(
            _repositoryMock.Object,
            _tokenServiceMock.Object,
            _encrypterMock.Object);
    }

    [Fact]
    public async Task Handle_InformadoDadosValidos_DeverRetornarToken()
    {
        var query = new GetFuncionarioTokenQuery("func@mail.com", "password");
        var expectedToken = "generatedToken";


        _repositoryMock.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(new FuncionarioEntity("func", "func@mail.com", "password", FuncionarioFuncao.Atendente));
        _tokenServiceMock.Setup(x => x.GerarToken(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(expectedToken);

        var result = await _sut.Handle(query, CancellationToken.None);

        Assert.Equal(expectedToken, result);
    }

    [Fact]
    public async Task Handle_InformadoDadosInvalidos_DeverRetornarStringVazio()
    {
        var query = new GetFuncionarioTokenQuery("", "");
        var expectedToken = string.Empty;

        _repositoryMock.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(default(FuncionarioEntity));

        var result = await _sut.Handle(query, CancellationToken.None);

        _tokenServiceMock.Verify(x => x.GerarToken(It.IsAny<string>(), It.IsAny<string>()), Times.Never);

        Assert.Equal(expectedToken, result);
    }
}
