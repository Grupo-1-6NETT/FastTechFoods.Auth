using Auth.Application.Commands.Funcionario;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Controllers;
[Route("[controller]")]
[ApiController]
public class FuncionarioController(ISender sender) : ControllerBase
{
    /// <summary>
    /// Adiciona um Funcionário na base de dados 
    /// </summary>
    /// <remarks>
    /// Exemplo:    
    ///  {  
    ///     "nome": "batman",   
    ///     "email": "batman@gotham.com"    
    ///     "senha": "P4ssw0rd",    
    ///     "funcao": "2"    
    /// }   
    /// funcao: 1-gerente, 2-atendente  
    /// 
    /// </remarks>
    /// <param name="command">Comando com os dados do Funcionário</param>
    /// <returns>O Id do Funcionário adicionado</returns>
    /// <response code="201">Funcionário adicionado na base de dados</response>
    /// <response code="400">Falha ao processar a requisição</response>
    /// <response code="500">Erro inesperado</response>
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    [HttpPost]
    public async Task<IActionResult> CadastrarFuncionarioAsync([FromBody] AddFuncionarioCommand funcionario)
    {
        var result = await sender.Send(funcionario);
        return Created("", result);       
    }

    /// <summary>
    /// Remove o Funcionário na base de dados com o ID informado
    /// </summary>
    /// <param name="id">O ID do Funcionário a ser removido</param>
    /// <returns>Resultado da operação de remoção</returns>
    /// <response code="200">Funcionário removido com sucesso</response>
    /// <response code="401">Funcionário não autenticado</response>
    /// <response code="403">Funcionário não autorizado</response>
    /// <response code="404">Funcionário não encontrado</response>
    /// <response code="500">Erro inesperado</response>
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    [HttpDelete("{id}")]
    [Authorize(Roles = "gerente")]
    public async Task<IActionResult> RemoverFuncionarioAsync([FromRoute] Guid id)
    {
        var result = await sender.Send(new DeleteFuncionarioCommand(id));
        return Ok("Funcionário removido com sucesso");
    }
}
