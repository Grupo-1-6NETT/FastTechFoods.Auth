namespace Auth.Core.Entities;
public abstract class BaseEntity
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public DateTime DataCriacao { get; init; } = DateTime.UtcNow;
    public DateTime? DataAtualizacao { get; protected set; }

    public void AtualizarData()
    {
        DataAtualizacao = DateTime.UtcNow;
    }
}
