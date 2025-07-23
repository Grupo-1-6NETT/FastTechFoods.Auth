using Auth.API.Controllers;
using Auth.Application.Commands.Funcionario;
using Auth.Core.Enums;
using Auth.Exception.CustomExceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Auth.API.Test;

public class FuncionarioControllerTests
{
    private readonly Mock<ISender> _senderMock;
    private readonly FuncionarioController _sut;

    public FuncionarioControllerTests()
    {
        _senderMock = new Mock<ISender>();
        _sut = new(_senderMock.Object);
    }

    #region CadastrarFuncionarioAsync
    [Fact]
    public async Task CadastrarFuncionarioAsync_InformadosDadosValidos_DeveRetornarCreatedResult()
    {
        var guid = Guid.Empty;

        var command = new AddFuncionarioCommand(
            Nome: "nome",
            Email: "email@test.com",
            Senha: "senha",
            funcao: FuncionarioFuncao.Atendente
        );

        _senderMock
            .Setup(m => m.Send(It.IsAny<AddFuncionarioCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(guid);

        var result = await _sut.CadastrarFuncionarioAsync(command);
        var createdResult = Assert.IsType<CreatedResult>(result);

        Assert.Equal(guid, createdResult.Value);
    }

    [Fact]
    public async Task CadastrarFuncionarioAsync_DadosInvalidos_DeveJogarExcecao()
    {
        var command = new AddFuncionarioCommand("", "", "", FuncionarioFuncao.Atendente);

        _senderMock
            .Setup(m => m.Send(It.IsAny<AddFuncionarioCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ErrorOnValidationException([]));

        await Assert.ThrowsAsync<ErrorOnValidationException>(() => _sut.CadastrarFuncionarioAsync(command));
    }
    #endregion    

    #region RemoverFuncionarioAsync
    [Fact]
    public async Task RemoverFuncionarioAsync_InformadosDadosValidos_DeveRetornarOk()
    {
        var id = Guid.NewGuid();

        _senderMock
            .Setup(m => m.Send(It.IsAny<DeleteFuncionarioCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var result = await _sut.RemoverFuncionarioAsync(id);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task RemoverFuncionarioAsync_DadosInvalidos_DeveJogarExcecao()
    {
        var id = Guid.NewGuid();

        _senderMock
            .Setup(m => m.Send(It.IsAny<DeleteFuncionarioCommand>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new NotFoundException());

        await Assert.ThrowsAsync<NotFoundException>(() => _sut.RemoverFuncionarioAsync(id));
    }
    #endregion
}