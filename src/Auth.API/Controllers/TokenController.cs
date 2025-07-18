using Auth.Application.Queries;
using Auth.Exception.ErrorMessages;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Controllers;
[Route("[controller]")]
[ApiController]
public class TokenController(ISender sender) : ControllerBase
{
    /// <summary>
    /// Gera um token de autenticação para o e-mail e senha informados
    /// </summary>
    /// <param name="email">E-mail</param>
    /// <param name="senha">Senha</param>
    /// <returns>O token de autenticação da API</returns>
    /// <response code="200">Token gerado com sucesso</response>
    /// <response code="401">Funcionário não autenticado</response>    
    /// <response code="500">Erro inesperado</response>
    [HttpGet]
    [Route("funcionario")]
    public async Task<IActionResult> GetFuncionarioToken(string email, string senha)
    {
        var token = await sender.Send(new GetFuncionarioTokenQuery(email, senha));
        return string.IsNullOrEmpty(token) ? Unauthorized(ResourceErrorMessages.UNAUTHORIZED) : Ok(new { Token = token });
    }

    /// <summary>
    /// Gera um token de autenticação para o CPF e senha informados. 
    /// </summary>
    /// <param name="cpf">Informe CPF</param>
    /// <param name="senha">Senha</param>
    /// <returns>O token de autenticação da API</returns>
    /// <response code="200">Token gerado com sucesso</response>
    /// <response code="401">Funcionário não autenticado</response>    
    /// <response code="500">Erro inesperado</response>
    [HttpGet]
    [Route("cpf")]
    public async Task<IActionResult> GetClienteCpfToken(string cpf, string senha)
    {
        var token = await sender.Send(new GetClienteByCpfTokenQuery(cpf, senha));
        return string.IsNullOrEmpty(token) ? Unauthorized(ResourceErrorMessages.UNAUTHORIZED) : Ok(new { Token = token });
    }
    /// <summary>
    /// Gera um token de autenticação para o email e senha informados. 
    /// </summary>
    /// <param name="email">Informe email</param>
    /// <param name="senha">Senha</param>
    /// <returns>O token de autenticação da API</returns>
    /// <response code="200">Token gerado com sucesso</response>
    /// <response code="401">Funcionário não autenticado</response>    
    /// <response code="500">Erro inesperado</response>
    /// 
    [HttpGet]
    [Route("email")]
    public async Task<IActionResult> GetClienteEmailToken(string email, string senha)
    {
        var token = await sender.Send(new GetClienteByEmailTokenQuery(email, senha));
        return string.IsNullOrEmpty(token) ? Unauthorized(ResourceErrorMessages.UNAUTHORIZED) : Ok(new { Token = token });
    }
}
