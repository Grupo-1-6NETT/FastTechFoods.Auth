using Auth.Application.Commands.Cliente;
using Auth.Application.Services.Cryptography;
using Auth.Core.Entities;
using Auth.Core.Interfaces;
using Auth.Exception.CustomExceptions;
using Auth.Exception.ErrorMessages;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Reflection;

namespace Auth.Application.Test;
public class AdicionaClienteTest
{
    private readonly AddClienteCommandHandler _handler;
    private readonly Mock<IClienteRepository> _repoMock = new();
    private readonly Mock<IUnitOfWork> _unitMock = new();
    private readonly IEncrypter _encrypter;

    public AdicionaClienteTest()
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "Settings:Secret", "da450cda202043fcb740346f45ace357" }
            })
            .Build();

        _encrypter = new Encrypter(config);
        _handler = new AddClienteCommandHandler(_repoMock.Object, _unitMock.Object, _encrypter);
    }

    [Fact]
    public async Task Handle_ComDadosValidos_DeveCriarClienteERetornarId()
    {        
        var command = new AddClienteCommand("Ana", "907.169.600-60", "ana@email.com", "senha123");

        _repoMock.Setup(r => r.ClienteExisteAsync(command.Email)).ReturnsAsync(false);

        var clienteAdicionado = new ClienteEntity(command.Nome, command.Cpf, command.Email, _encrypter.Encrypt(command.Senha));
        _repoMock.Setup(r => r.AddAsync(It.IsAny<ClienteEntity>())).ReturnsAsync(clienteAdicionado);
        
        var id = await _handler.Handle(command, default);

        Assert.NotEqual(Guid.Empty, id);
        _repoMock.Verify(r => r.AddAsync(It.IsAny<ClienteEntity>()), Times.Once);
        _unitMock.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_EmailJaCadastrado_DeveLancarErrorOnValidationException()
    {
        var command = new AddClienteCommand("João", "907.169.600-60", "joao@email.com", "senha123");

        _repoMock.Setup(r => r.ClienteExisteAsync(command.Email)).ReturnsAsync(true);

        var ex = await Assert.ThrowsAsync<ErrorOnValidationException>(() =>
            _handler.Handle(command, default));

        Assert.Contains(ResourceErrorMessages.EMAIL_ALREADY_REGISTERED, ex.ErrorMessages[0]);
    }

    [Fact]
    public async Task Handle_ComDadosInvalidos_DeveLancarErrorOnValidationException()
    {
        var command = new AddClienteCommand("Maria", "", "maria@email.com", "senha123");

        _repoMock.Setup(r => r.ClienteExisteAsync(command.Email)).ReturnsAsync(false);

        var ex = await Assert.ThrowsAsync<ErrorOnValidationException>(() =>
            _handler.Handle(command, default));

        Assert.Contains(ResourceErrorMessages.CPF_INVALID, ex.ErrorMessages[0]);
    }
}
