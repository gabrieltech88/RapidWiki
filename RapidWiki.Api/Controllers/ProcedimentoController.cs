using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RapidWiki.Application.CreateProcedimento;



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

    [Authorize(Roles = "Admin, Editor")]
    [HttpPost("create_procedimento")]
    public async Task<IActionResult> CreateProcedimento([FromBody] CreateProcedimentoRequest request)
    {
        var result = await _mediator.Send(request);
        return Created(string.Empty, result);
    }
}