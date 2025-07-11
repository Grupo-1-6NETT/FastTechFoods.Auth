using Auth.Core.Entities;

namespace Auth.Core.Interfaces;
public interface IFuncionarioRepository
{
    Task<FuncionarioEntity?> GetAsync(string email, string senha);
    Task<FuncionarioEntity> AddAsync(FuncionarioEntity funcionario);
    Task<bool> DeleteAsync(Guid id);
}
