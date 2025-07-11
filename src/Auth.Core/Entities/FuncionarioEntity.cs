using Auth.Core.Enums;

namespace Auth.Core.Entities;
public class FuncionarioEntity : BaseEntity
{
    public string Nome { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Senha { get; private set; } = string.Empty;
    public FuncionarioFuncao Funcao { get; private set; }

    protected FuncionarioEntity() { }

    public FuncionarioEntity(string nome, string email, string senha, FuncionarioFuncao funcao)
    {
        Nome = nome;
        Email = email;
        Senha = senha;
        Funcao = funcao;
    }
}
