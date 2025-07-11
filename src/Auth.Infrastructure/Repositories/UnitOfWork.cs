using Auth.Core.Interfaces;
using Auth.Infrastructure.Data;

namespace Auth.Infrastructure.Repositories;
internal class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _dbContext;

    public UnitOfWork(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CommitAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}
