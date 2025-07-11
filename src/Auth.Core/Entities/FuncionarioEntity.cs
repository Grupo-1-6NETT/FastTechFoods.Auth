using Auth.Core.Enums;

namespace Auth.Core.Entities;
internal class FuncionarioEntity
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
    public FuncionarioFuncao Funcao { get; set; }
}
