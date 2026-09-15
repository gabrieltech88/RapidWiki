using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RapidWiki.Application.Common.Authorization;
using RapidWiki.Application.CreateProcedimento;
using RapidWiki.Application.GetProcedimentoById;
using RapidWiki.Application.GetProcedimentosRequest;



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
}