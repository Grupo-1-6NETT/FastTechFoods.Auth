using Auth.Application.Commands.Cliente;
using Auth.Application.Services.Cryptography;
using Auth.Core.Entities;
using Auth.Core.Interfaces;
using Auth.Exception.CustomExceptions;
using Auth.Exception.ErrorMessages;
using Moq;

namespace Auth.Application.Test.Commands.Cliente;
public class AddClienteCommandHandlerTests
{
    private readonly Mock<IClienteRepository> _repositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IEncrypter> _encrypterMock;

    private readonly AddClienteCommandHandler _sut;

    public AddClienteCommandHandlerTests()
    {
        _repositoryMock = new Mock<IClienteRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _encrypterMock = new Mock<IEncrypter>();
        _sut = new AddClienteCommandHandler(
            _repositoryMock.Object,
            _unitOfWorkMock.Object,
            _encrypterMock.Object
        );
    }

    [Fact]
    public async Task Handle_InformadosDadosValidos_DeveAdicionarCliente()
    {
        var command = new AddClienteCommand("nome", "98375115088", "email@test.com", "senhagrande");

        _repositoryMock
            .Setup(x => x.AddAsync(It.IsAny<ClienteEntity>()))
            .ReturnsAsync(new ClienteEntity(
                "nome",
                "98375115088",
                "email@test.com",
                "senhagrande")
            {
                Id = Guid.NewGuid()
            });

        var result = await _sut.Handle(command, CancellationToken.None);

        _repositoryMock.Verify(x => x.AddAsync(It.IsAny<ClienteEntity>()), Times.Once);
    }


    [Fact]
    public async Task Handle_EmailJaCadastrado_DeveLancarErrorOnValidationException()
    {
        var command = new AddClienteCommand("João", "907.169.600-60", "joao@email.com", "senha123");

        _repositoryMock.Setup(r => r.ClienteExisteAsync(command.Email)).ReturnsAsync(true);

        var ex = await Assert.ThrowsAsync<ErrorOnValidationException>(() =>
            _sut.Handle(command, default));

        Assert.Contains(ResourceErrorMessages.EMAIL_ALREADY_REGISTERED, ex.ErrorMessages[0]);
    }

    [Fact]
    public async Task Handle_ComDadosInvalidos_DeveLancarErrorOnValidationException()
    {
        var command = new AddClienteCommand("Maria", "", "maria@email.com", "senha123");

        _repositoryMock.Setup(r => r.ClienteExisteAsync(command.Email)).ReturnsAsync(false);

        var ex = await Assert.ThrowsAsync<ErrorOnValidationException>(() =>
            _sut.Handle(command, default));

        Assert.Contains(ResourceErrorMessages.CPF_INVALID, ex.ErrorMessages[0]);
    }

    [Fact]
    public async Task Handle_InformadosDadosInvalidos_ValidationException()
    {
        var command = new AddClienteCommand("", "", "", "");

        await Assert.ThrowsAsync<ErrorOnValidationException>(async () => await _sut.Handle(command, CancellationToken.None));
    }
}
