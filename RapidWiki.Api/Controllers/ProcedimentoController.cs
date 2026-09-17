using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RapidWiki.Application.Common.Authorization;
using RapidWiki.Application.CreateProcedimento;
using RapidWiki.Application.DeleteProcedimento;
using RapidWiki.Application.DeleteRascunho;
using RapidWiki.Application.GetProcedimentoById;
using RapidWiki.Application.GetProcedimentoForEdit;
using RapidWiki.Application.GetProcedimentosRequest;
using RapidWiki.Application.GetRascunhos;
using RapidWiki.Application.UpdateProcedimento;

namespace RapidWiki.Api.Controllers;

[ApiController]
[Route("app/v1/procedimento")]
public class ProcedimentoController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProcedimentoController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize(Roles = $"{Roles.Admin}, {Roles.Editor}")]
    [HttpPost("create_procedimento")]
    public async Task<IActionResult> CreateProcedimento([FromBody] CreateProcedimentoRequest request)
    {
        var result = await _mediator.Send(request);
        return Created(string.Empty, result);
    }

    [Authorize]
    [HttpGet("get_procedimentos")]
    public async Task<IActionResult> GetProcedimentos([FromQuery] GetProcedimentosRequest request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
    }

    [Authorize]
    [HttpGet("get_procedimento_by_id/{id:guid}")]
    public async Task<IActionResult> GetProcedimentoById(Guid id)
    {
        var result = await _mediator.Send(new GetProcedimentoByIdRequest(id));
        return Ok(result);
    }

    [Authorize(Roles = $"{Roles.Admin}, {Roles.Editor}")]
    [HttpGet("get_procedimento_for_edit/{id:guid}")]
    public async Task<IActionResult> GetProcedimentoForEdit(Guid id)
    {
        var result = await _mediator.Send(new GetProcedimentoForEditRequest(id));
        return Ok(result);
    }

    [Authorize(Roles = $"{Roles.Admin}, {Roles.Editor}")]
    [HttpGet("get_rascunhos")]
    public async Task<IActionResult> GetRascunhos([FromQuery] GetRascunhosRequest request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
    }

    [Authorize(Roles = $"{Roles.Admin}, {Roles.Editor}")]
    [HttpPut("update_procedimento")]
    public async Task<IActionResult> UpdateProcedimento([FromBody] UpdateProcedimentoRequest request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
    }

    [Authorize(Roles = $"{Roles.Admin}, {Roles.Editor}")]
    [HttpDelete("delete_procedimento/{id:guid}")]
    public async Task<IActionResult> DeleteProcedimento(Guid id)
    {
        await _mediator.Send(new DeleteProcedimentoRequest(id));
        return NoContent();
    }

    [Authorize(Roles = $"{Roles.Admin}, {Roles.Editor}")]
    [HttpDelete("delete_rascunho/{id:guid}")]
    public async Task<IActionResult> DeleteRascunho(Guid id)
    {
        await _mediator.Send(new DeleteRascunhoRequest(id));
        return NoContent();
    }
}
