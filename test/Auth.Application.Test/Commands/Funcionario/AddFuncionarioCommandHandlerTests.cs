using Auth.Application.Commands.Funcionario;
using Auth.Application.Services.Cryptography;
using Auth.Core.Entities;
using Auth.Core.Enums;
using Auth.Core.Interfaces;
using Auth.Exception.CustomExceptions;
using Moq;

namespace Auth.Application.Test.Commands.Funcionario;
public class AddFuncionarioCommandHandlerTests
{
    private readonly Mock<IFuncionarioRepository> _repositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IEncrypter> _encrypterMock;

    private readonly AddFuncionarioCommandHandler _sut;

    public AddFuncionarioCommandHandlerTests()
    {
        _repositoryMock = new Mock<IFuncionarioRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _encrypterMock = new Mock<IEncrypter>();
        _sut = new AddFuncionarioCommandHandler(
            _repositoryMock.Object,
            _unitOfWorkMock.Object,
            _encrypterMock.Object
        );
    }

    [Fact]
    public async Task Handle_InformadosDadosValidos_DeveAdicionarUsuario()
    {
        var command = new AddFuncionarioCommand("nome", "email@test.com", "senhagrande", FuncionarioFuncao.Atendente);

        _repositoryMock
            .Setup(x => x.AddAsync(It.IsAny<FuncionarioEntity>()))
            .ReturnsAsync(new FuncionarioEntity(
                "nome",
                "email@test.com",
                "senhagrande",
                FuncionarioFuncao.Atendente)
            {
                Id = Guid.NewGuid()
            });

        var result = await _sut.Handle(command, CancellationToken.None);

        _repositoryMock.Verify(x => x.AddAsync(It.IsAny<FuncionarioEntity>()), Times.Once);
    }

    [Fact]
    public async Task Handle_InformadosDadosInvalidos_ValidationException()
    {
        var command = new AddFuncionarioCommand("", "", "", FuncionarioFuncao.Atendente);

        await Assert.ThrowsAsync<ErrorOnValidationException>(async () => await _sut.Handle(command, CancellationToken.None));
    }
}
