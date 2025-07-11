namespace Auth.Core.Interfaces;
public interface IUnitOfWork
{
    Task CommitAsync();
}
