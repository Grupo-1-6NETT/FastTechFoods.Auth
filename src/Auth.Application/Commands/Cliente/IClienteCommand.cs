namespace Auth.Application.Commands.Cliente;
public interface IClienteCommand
{    string Nome { get; }
    string Cpf { get; }
    string Email { get; }
    string Senha { get; }
}
