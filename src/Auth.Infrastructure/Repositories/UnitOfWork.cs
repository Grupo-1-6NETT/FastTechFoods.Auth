using Auth.Core.Interfaces;
using Auth.Infrastructure.Data;

namespace Auth.Infrastructure.Repositories;
internal class UnitOfWork : IUnitOfWork
{
    private readonly AuthDbContext _dbContext;

    public UnitOfWork(AuthDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CommitAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}
