using Auth.API.Controllers;
using Auth.Application.Commands.Cliente;
using Auth.Exception.CustomExceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Auth.API.Test;

public class ClienteControllerTests
{
    private readonly Mock<ISender> _senderMock;
    private readonly ClienteController _sut;

    public ClienteControllerTests()
    {
        _senderMock = new Mock<ISender>();
        _sut = new(_senderMock.Object);
    }

    #region CadastrarClienteAsync
    [Fact]
    public async Task CadastrarClienteAsync_InformadosDadosValidos_DeveRetornarCreatedResult()
    {
        var guid = Guid.Empty;

        var command = new AddClienteCommand(
            Nome: "nome",
            Email: "email@test.com",
            Senha: "senha",
            Cpf: "00000000000"
        );

        _senderMock
            .Setup(m => m.Send(It.IsAny<AddClienteCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(guid);

        var result = await _sut.CadastrarClienteAsync(command);
        var createdResult = Assert.IsType<CreatedResult>(result);

        Assert.Equal(guid, createdResult.Value);
    }

    [Fact]
    public async Task CadastrarClienteAsync_DadosInvalidos_DeveJogarExcecao()
    {
        var command = new AddClienteCommand("", "", "", "");

        _senderMock
            .Setup(m => m.Send(It.IsAny<AddClienteCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ErrorOnValidationException([]));

        await Assert.ThrowsAsync<ErrorOnValidationException>(() => _sut.CadastrarClienteAsync(command));
    }
    #endregion

    #region AtualizarClienteAsync
    [Fact]
    public async Task AtualizarClienteAsync_InformadosDadosValidos_DeveRetornarOk()
    {
        var command = new UpdateClienteCommand(
            Nome: "nome",
            Email: "email@test.com",
            Senha: "senha",
            Cpf: "00000000000"
        );

        _senderMock
            .Setup(m => m.Send(It.IsAny<UpdateClienteCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var result = await _sut.AtualizarClienteAsync(command);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task AtualizarClienteAsync_DadosInvalidos_DeveJogarExcecao()
    {
        var command = new UpdateClienteCommand("", "", "", "");

        _senderMock
            .Setup(m => m.Send(It.IsAny<UpdateClienteCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ErrorOnValidationException([]));

        await Assert.ThrowsAsync<ErrorOnValidationException>(() => _sut.AtualizarClienteAsync(command));
    }
    #endregion

    #region RemoverClienteAsync
    [Fact]
    public async Task RemoverClienteAsync_InformadosDadosValidos_DeveRetornarOk()
    {
        var id = Guid.NewGuid();

        _senderMock
            .Setup(m => m.Send(It.IsAny<DeleteClienteCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var result = await _sut.RemoverClienteAsync(id);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task RemoverClienteAsync_DadosInvalidos_DeveJogarExcecao()
    {
        var id = Guid.NewGuid();

        _senderMock
            .Setup(m => m.Send(It.IsAny<DeleteClienteCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new NotFoundException());

        await Assert.ThrowsAsync<NotFoundException>(() => _sut.RemoverClienteAsync(id));
    }
    #endregion
}