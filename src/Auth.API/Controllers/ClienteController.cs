using Auth.Application.Commands.Cliente;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Controllers;
[Route("[controller]")]
[ApiController]
public class ClienteController(ISender sender) : ControllerBase
{
    /// <summary>
    /// Adiciona um Cliente na base de dados 
    /// </summary>
    /// <remarks>
    /// Exemplo:    
    /// {   
    ///     "nome": "Alfred",   
    ///     "cpf": "11111111111"    
    ///     "email": "alfred@gotham.com"    
    ///     "senha": "P4ssw0rd"   
    /// }
    /// </remarks>
    /// <param name="command">Comando com os dados do Cliente</param>
    /// <returns>O Id do Cliente adicionado</returns>
    /// <response code="201">Cliente adicionado na base de dados</response>
    /// <response code="400">Falha ao processar a requisição</response>
    /// <response code="500">Erro inesperado</response>
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    [HttpPost]
    public async Task<IActionResult> CadastrarClienteAsync([FromBody] AddClienteCommand command)
    {
        var result = await sender.Send(command);
        return Created("", result);
    }

    /// <summary>
    /// Atualiza dados um Cliente na base de dados 
    /// </summary>
    /// <remarks>
    /// Exemplo:    
    ///  {  
    ///     "nome": "Alfred", 
    ///     "cpf": "11111111111" 
    ///     "email": "alfred@gotham.com"    
    ///     "senha": "P4ssw0rd"     
    /// }
    /// </remarks>
    /// <param name="command">Comando com os dados do Cliente</param>
    /// <returns>Resultado da operação</returns>
    /// <response code="200">Dados do cliente atualizados na base de dados</response>
    /// <response code="400">Falha ao processar a requisição</response>
    /// <response code="500">Erro inesperado</response>
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    [HttpPut]
    public async Task<IActionResult> AtualizarClienteAsync([FromBody] UpdateClienteCommand command)
    {
        var result = await sender.Send(command);
        return Ok("Cliente atualizado com sucesso.");
    }

    /// <summary>
    /// Remove o Cliente na base de dados com o ID informado
    /// </summary>
    /// <param name="id">O ID do Cliente a ser removido</param>
    /// <returns>Resultado da operação de remoção</returns>
    /// <response code="200">Cliente removido com sucesso</response>
    /// <response code="401">Usuário não autenticado</response>
    /// <response code="403">Usuário não autorizado</response>
    /// <response code="404">Cliente não encontrado</response>
    /// <response code="500">Erro inesperado</response>
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    [Authorize(Roles = "Gerente")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoverClienteAsync([FromRoute] Guid id)
    {
        var result = await sender.Send(new DeleteClienteCommand(id));
        return Ok("Cliente removido com sucesso");
    }
}
