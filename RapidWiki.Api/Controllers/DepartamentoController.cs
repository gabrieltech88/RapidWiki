using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RapidWiki.Application.Common.Authorization;
using RapidWiki.Application.CreateDepartamento;
using RapidWiki.Application.DeleteDepartamento;
using RapidWiki.Application.GetAllDepartamentos;
using RapidWiki.Application.GetDepartamentosParaProcedimento;
using RapidWiki.Application.UpdateDepartamento;


namespace RapidWiki.Api.Controllers;

[ApiController]
[Route("app/v1/departamento")]
public class DepartamentoController : ControllerBase
{
    private readonly IMediator _mediator;
    public DepartamentoController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize(Roles = $"{Roles.Admin}")]
    [HttpPost("create_departamento")]
    public async Task<IActionResult> CreateDepartamento([FromBody] CreateDepartamentoRequest request)
    {
        var result = await _mediator.Send(request);
        return Created(string.Empty, result);
    }

    [Authorize]
    [HttpGet("get_all_departamentos")]
    public async Task<IActionResult> GetAllDepartamentos()
    {
        var request = new GetAllDepartamentosRequest();
        var result = await _mediator.Send(request);
        return Ok(result);
    }

    [Authorize(Roles = $"{Roles.Admin}")]
    [HttpPut("update_departamento")]
    public async Task<IActionResult> UpdateDepartamento(
        [FromBody] UpdateDepartamentoRequest request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
    }

    [Authorize(Roles = $"{Roles.Admin}")]
    [HttpDelete("delete_departamento/{id:guid}")]
    public async Task<IActionResult> DeleteDepartamento([FromRoute] Guid id)
    {
        await _mediator.Send(new DeleteDepartamentoRequest(id));
        return NoContent();
    }

    [Authorize(Roles = $"{Roles.Admin}, {Roles.Editor}")]
    [HttpGet("get_departamentos_para_procedimento")]
    public async Task<IActionResult> GetDepartamentosParaProcedimento()
    {
        var result = await _mediator.Send(new GetDepartamentosParaProcedimentoRequest());

        return Ok(result);
    }

}
