using MediatR;
using Microsoft.AspNetCore.Mvc;
using RapidWiki.Application.CreateRole;
using RapidWiki.Application.CreateUsuario;
using RapidWiki.Application.SignIn;


namespace RapidWiki.Api.Controllers;

[ApiController]
[Route("app/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("create_user")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUsuarioRequest request)
    {
        var result = await _mediator.Send(request);
        return Created(string.Empty, result);
    }

    [HttpPost("create_role")]
    public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest request)
    {
        var result = await _mediator.Send(request);
        return Created(string.Empty, result);
    }

    [HttpPost("sign_in")]
    public async Task<IActionResult> SignIn([FromBody] SignInRequest request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
    }
}
