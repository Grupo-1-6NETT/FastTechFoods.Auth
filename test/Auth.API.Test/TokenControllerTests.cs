using Auth.API.Controllers;
using Auth.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Auth.API.Test;

public class TokenControllerTests
{
    private readonly Mock<ISender> _senderMock;
    private readonly TokenController _sut;

    public TokenControllerTests()
    {
        _senderMock = new Mock<ISender>();
        _sut = new(_senderMock.Object);
    }

    #region GetFuncionarioToken
    [Fact]
    public async Task GetFuncionarioToken_InformadoDadosValidos_DeverRetornarOk()
    {
        var email = "user@test.com";
        var senha = "password";
        var expectedToken = "generatedToken";

        _senderMock
            .Setup(m => m.Send(It.IsAny<GetFuncionarioTokenQuery>(), default))
            .ReturnsAsync(expectedToken);

        var result = await _sut.GetFuncionarioToken(email, senha);

        var okResult = Assert.IsType<OkObjectResult>(result);
    }


    [Fact]
    public async Task GetFuncionarioToken_InformadoDadosInvalidos_DeverRetornarUnauthorized()
    {
        var nome = "";
        var senha = "";

        _senderMock
            .Setup(m => m.Send(It.IsAny<GetFuncionarioTokenQueryHandler>(), default))
            .ReturnsAsync(string.Empty);

        var result = await _sut.GetFuncionarioToken(nome, senha);

        Assert.IsType<UnauthorizedObjectResult>(result);
    }
    #endregion    

    #region GetClienteCpfToken
    [Fact]
    public async Task GetClienteCpfToken_InformadoDadosValidos_DeverRetornarOk()
    {
        var cpf = "00000000000";
        var senha = "password";
        var expectedToken = "generatedToken";

        _senderMock
            .Setup(m => m.Send(It.IsAny<GetClienteByCpfTokenQuery>(), default))
            .ReturnsAsync(expectedToken);

        var result = await _sut.GetClienteCpfToken(cpf, senha);

        var okResult = Assert.IsType<OkObjectResult>(result);
    }


    [Fact]
    public async Task GetClienteCpfToken_InformadoDadosInvalidos_DeverRetornarUnauthorized()
    {
        var cpf = "";
        var senha = "";

        _senderMock
            .Setup(m => m.Send(It.IsAny<GetClienteByCpfTokenQuery>(), default))
            .ReturnsAsync(string.Empty);

        var result = await _sut.GetClienteCpfToken(cpf, senha);

        Assert.IsType<UnauthorizedObjectResult>(result);
    }
    #endregion


    #region GetClienteEmailToken
    [Fact]
    public async Task GetClienteEmailToken_InformadoDadosValidos_DeverRetornarOk()
    {
        var email = "cliente@email.com";
        var senha = "password";
        var expectedToken = "generatedToken";

        _senderMock
            .Setup(m => m.Send(It.IsAny<GetClienteByEmailTokenQuery>(), default))
            .ReturnsAsync(expectedToken);

        var result = await _sut.GetClienteEmailToken(email, senha);

        var okResult = Assert.IsType<OkObjectResult>(result);
    }


    [Fact]
    public async Task GetClienteEmailToken_InformadoDadosInvalidos_DeverRetornarUnauthorized()
    {
        var email = "";
        var senha = "";

        _senderMock
            .Setup(m => m.Send(It.IsAny<GetClienteByEmailTokenQuery>(), default))
            .ReturnsAsync(string.Empty);

        var result = await _sut.GetClienteEmailToken(email, senha);

        Assert.IsType<UnauthorizedObjectResult>(result);
    }
    #endregion
}