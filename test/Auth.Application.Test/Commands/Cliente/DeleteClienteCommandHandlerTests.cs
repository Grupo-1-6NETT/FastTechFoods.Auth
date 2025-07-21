using Auth.Application.Commands.Cliente;
using Auth.Core.Interfaces;
using Auth.Exception.CustomExceptions;
using Moq;

namespace Auth.Application.Test.Commands.Cliente;
public class DeleteClienteCommandHandlerTests
{
    private readonly Mock<IClienteRepository> _repositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    private readonly DeleteClienteCommandHandler _sut;

    public DeleteClienteCommandHandlerTests()
    {
        _repositoryMock = new Mock<IClienteRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _sut = new DeleteClienteCommandHandler(
            _repositoryMock.Object,
            _unitOfWorkMock.Object
        );
    }

    [Fact]
    public async Task Handle_InformadoClienteExistente_DeveRemoverCliente()
    {
        var id = new Guid();

        _repositoryMock
            .Setup(x => x.DeleteAsync(It.IsAny<Guid>()))
            .ReturnsAsync(true);

        var result = await _sut.Handle(new DeleteClienteCommand(id), CancellationToken.None);

        _repositoryMock.Verify(x => x.DeleteAsync(It.IsAny<Guid>()), Times.Once);

        Assert.True(result);
    }

    [Fact]
    public async Task Handle_InformadoClienteInexistente_ReturnFalse()
    {
        var id = new Guid();

        _repositoryMock
            .Setup(x => x.DeleteAsync(It.IsAny<Guid>()))
            .ReturnsAsync(false);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            _sut.Handle(new DeleteClienteCommand(id), CancellationToken.None)
        );

        _repositoryMock.Verify(x => x.DeleteAsync(It.IsAny<Guid>()), Times.Once);
    }
}
