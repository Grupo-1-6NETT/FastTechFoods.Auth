namespace Auth.Core.Entities;
public class ClienteEntity : BaseEntity
{
    public string Nome { get; private set; } = string.Empty;
    public string Cpf { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string SenhaHash { get; private set; } = string.Empty;

    protected ClienteEntity() { }

    public ClienteEntity(string nome, string cpf, string email, string senhaHash)
    {
        Nome = nome;
        Cpf = cpf;
        Email = email;
        SenhaHash = senhaHash;
    }

    public void AtualizarInfo(string nome, string email)
    {
        Nome = nome;        
        Email = email;
        AtualizarData();
    }
}
