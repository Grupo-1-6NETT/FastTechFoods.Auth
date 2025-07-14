using Auth.Core.Entities;
using Auth.Core.Interfaces;
using Auth.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Repositories;
internal class FuncionarioRepository : IFuncionarioRepository
{
    private readonly AuthDbContext _dbContext;

    public FuncionarioRepository(AuthDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<FuncionarioEntity?> GetAsync(string email, string senhaHash)
    {
        return await _dbContext.Funcionarios.AsNoTracking()
                .FirstOrDefaultAsync(f => f.Email == email && f.Senha == senhaHash);
    }

    public async Task<FuncionarioEntity> AddAsync(FuncionarioEntity funcionario)
    {
        await _dbContext.Funcionarios.AddAsync(funcionario);
        return funcionario;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var funcionario = await _dbContext.Funcionarios.FindAsync(id);

        if(funcionario is null)
            return false;
        else
        {
            _dbContext.Funcionarios.Remove(funcionario);
            return true;
        }
    }

    public async Task<bool> FuncionarioExisteAsync(string email)
    {
        return await _dbContext.Funcionarios.AnyAsync(f => f.Email == email);
    }
}
