using Auth.Application.Queries;
using Auth.Application.Services.Cryptography;
using Auth.Application.Services.TokenJwt;
using Auth.Core.Entities;
using Auth.Core.Enums;
using Auth.Core.Interfaces;
using Moq;

namespace Auth.Application.Test.Queries;
public class GetClienteByEmailTokenQueryTests
{
    private readonly Mock<IClienteRepository> _repositoryMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly Mock<IEncrypter> _encrypterMock;

    private readonly GetClienteByEmailTokenQueryHandler _sut;

    public GetClienteByEmailTokenQueryTests()
    {
        _repositoryMock = new Mock<IClienteRepository>();
        _tokenServiceMock = new Mock<ITokenService>();
        _encrypterMock = new Mock<IEncrypter>();
        _sut = new GetClienteByEmailTokenQueryHandler(
            _repositoryMock.Object,
            _tokenServiceMock.Object,
            _encrypterMock.Object);
    }

    [Fact]
    public async Task Handle_InformadoDadosValidos_DeverRetornarToken()
    {
        var query = new GetClienteByEmailTokenQuery("cliente@mail.com", "password");
        var expectedToken = "generatedToken";

        _encrypterMock
            .Setup(x => x.Encrypt(It.IsAny<string>()))
            .Returns(string.Empty);
        _repositoryMock
            .Setup(x => x.GetByEmailAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(new ClienteEntity("cliente", "00000000000", "cliente@mail.com", "password"));
        _tokenServiceMock
            .Setup(x => x.GerarToken(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(expectedToken);

        var result = await _sut.Handle(query, CancellationToken.None);

        Assert.Equal(expectedToken, result);
    }

    [Fact]
    public async Task Handle_InformadoDadosInvalidos_DeverRetornarStringVazio()
    {
        var query = new GetClienteByEmailTokenQuery("", "");
        var expectedToken = string.Empty;

        _repositoryMock.Setup(x => x.GetByEmailAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(default(ClienteEntity));

        var result = await _sut.Handle(query, CancellationToken.None);

        _tokenServiceMock.Verify(x => x.GerarToken(It.IsAny<string>(), It.IsAny<string>()), Times.Never);

        Assert.Equal(expectedToken, result);
    }
}
