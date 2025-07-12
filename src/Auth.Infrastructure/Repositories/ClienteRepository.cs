using Auth.Core.Entities;
using Auth.Core.Interfaces;
using Auth.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Repositories;
internal class ClienteRepository : IClienteRepository
{
    private readonly AppDbContext _dbContext;

    public ClienteRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<ClienteEntity?> GetByCpfAsync(string cpf, string senhaHash)
    {
        var cliente = await _dbContext.Clientes.AsNoTracking()
            .FirstOrDefaultAsync(c => c.Cpf == cpf && c.SenhaHash == senhaHash);
        
        return cliente;
    }

    public async Task<ClienteEntity?> GetByEmailAsync(string email, string senhaHash)
    {
        var cliente = await _dbContext.Clientes.AsNoTracking()
            .FirstOrDefaultAsync(c => c.Email == email && c.SenhaHash == senhaHash);

        return cliente;
    }
    public async Task<ClienteEntity> AddAsync(ClienteEntity cliente)
    {
        await _dbContext.AddAsync(cliente);
        
        return cliente;
    }
    public async Task<ClienteEntity?> UpdateAsync(string cpf, string nome, string email)
    {
        var cliente = await _dbContext.Clientes.FirstOrDefaultAsync(c => c.Cpf == cpf);

        if (cliente is null)
            return null;

        cliente.AtualizarInfo(nome, email);
        return cliente;
    }
    public async Task<bool> DeleteAsync(Guid id)
    {
        var cliente = await _dbContext.Clientes.FindAsync(id);

        if (cliente is null)
            return false;
        else
        {
            _dbContext.Clientes.Remove(cliente);
            return true;
        }
    }

    public async Task<bool> ClienteExisteAsync(string email)
    {
        return await _dbContext.Clientes.AnyAsync(c => c.Email.Equals(email));
    }
}
