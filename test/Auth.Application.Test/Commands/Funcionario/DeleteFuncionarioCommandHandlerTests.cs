using Auth.Application.Commands.Funcionario;
using Auth.Core.Interfaces;
using Auth.Exception.CustomExceptions;
using Moq;

namespace Auth.Application.Test.Commands.Funcionario;
public class DeleteFuncionarioCommandHandlerTests
{
    private readonly Mock<IFuncionarioRepository> _repositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    private readonly DeleteFuncionarioCommandHandler _sut;

    public DeleteFuncionarioCommandHandlerTests()
    {
        _repositoryMock = new Mock<IFuncionarioRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _sut = new DeleteFuncionarioCommandHandler(
            _repositoryMock.Object,
            _unitOfWorkMock.Object
        );
    }

    [Fact]
    public async Task Handle_InformadoFuncionarioExistente_DeveRemoverFuncionario()
    {
        var id = new Guid();

        _repositoryMock
            .Setup(x => x.DeleteAsync(It.IsAny<Guid>()))
            .ReturnsAsync(true);

        var result = await _sut.Handle(new DeleteFuncionarioCommand(id), CancellationToken.None);

        _repositoryMock.Verify(x => x.DeleteAsync(It.IsAny<Guid>()), Times.Once);

        Assert.True(result);
    }

    [Fact]
    public async Task Handle_InformadoFuncionarioInexistente_ReturnFalse()
    {
        var id = new Guid();

        _repositoryMock
            .Setup(x => x.DeleteAsync(It.IsAny<Guid>()))
            .ReturnsAsync(false);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            _sut.Handle(new DeleteFuncionarioCommand(id), CancellationToken.None)
        );

        _repositoryMock.Verify(x => x.DeleteAsync(It.IsAny<Guid>()), Times.Once);
    }
}
