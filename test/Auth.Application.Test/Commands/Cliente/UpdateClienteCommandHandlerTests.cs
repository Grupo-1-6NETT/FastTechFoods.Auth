using Auth.Application.Commands.Cliente;
using Auth.Application.Services.Cryptography;
using Auth.Core.Entities;
using Auth.Core.Interfaces;
using Auth.Exception.CustomExceptions;
using Moq;

namespace Auth.Application.Test.Commands.Cliente;
public class UpdateClienteCommandHandlerTests
{
    private readonly Mock<IClienteRepository> _repositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IEncrypter> _encrypterMock;

    private readonly UpdateClienteCommandHandler _sut;

    public UpdateClienteCommandHandlerTests()
    {
        _repositoryMock = new Mock<IClienteRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _encrypterMock = new Mock<IEncrypter>();
        _sut = new UpdateClienteCommandHandler(
            _repositoryMock.Object,
            _unitOfWorkMock.Object,
            _encrypterMock.Object
        );
    }

    [Fact]
    public async Task Handle_InformadosDadosValidos_DeveAtualizarUsuario()
    {
        var command = new UpdateClienteCommand("nome", "98375115088", "email@test.com", "senhagrande");

        _repositoryMock
            .Setup(x => x.GetByCpfAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(new ClienteEntity(
                "nome",
                "98375115088",
                "email@test.com",
                "senhagrande"));

        _repositoryMock
            .Setup(x => x.UpdateAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(new ClienteEntity(
                "nome",
                "98375115088",
                "email@test.com",
                "senhagrande")
            {
                Id = Guid.NewGuid()
            });

        var result = await _sut.Handle(command, CancellationToken.None);

        _repositoryMock.Verify(x => x.UpdateAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()
            ),
            Times.Once);
    }

    [Fact]
    public async Task Handle_InformadosDadosInvalidos_ValidationException()
    {
        var command = new UpdateClienteCommand("", "", "", "");

        await Assert.ThrowsAsync<ErrorOnValidationException>(async () => await _sut.Handle(command, CancellationToken.None));
    }
}
