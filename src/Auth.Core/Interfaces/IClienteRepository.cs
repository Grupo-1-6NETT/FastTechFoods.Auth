using Auth.Core.Entities;

namespace Auth.Core.Interfaces;
public interface IClienteRepository
{
    Task<ClienteEntity?> GetByCpfAsync(string cpf, string senha);
    Task<ClienteEntity?> GetByEmailAsync(string email, string senha);
    Task<ClienteEntity?> AddAsync(ClienteEntity cliente);
    Task<ClienteEntity?> UpdateAsync(string cpf, string nome, string email);
    Task<bool> DeleteAsync(Guid id);
}
