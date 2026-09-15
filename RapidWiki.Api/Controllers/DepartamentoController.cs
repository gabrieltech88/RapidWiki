using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RapidWiki.Application.Common.Authorization;
using RapidWiki.Application.CreateDepartamento;
using RapidWiki.Application.GetAllDepartamentos;


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
}
