using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RapidWiki.Application.Common.Authorization;
using RapidWiki.Application.CreateRole;
using RapidWiki.Application.CreateUsuario;
using RapidWiki.Application.GetCurrentUser;
using RapidWiki.Application.SignIn;
using RapidWiki.Application.SignOut;


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

    [Authorize(Roles = $"{Roles.Admin}")]
    [HttpPost("create_user")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUsuarioRequest request)
    {
        var result = await _mediator.Send(request);
        return Created(string.Empty, result);
    }

    [Authorize(Roles = $"{Roles.Admin}")]
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

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        var result = await _mediator.Send(new GetCurrentUserRequest());
        return Ok(result);
    }

     [Authorize]
    [HttpPost("sign_out")]
    public async Task<IActionResult> SignOutUser()
    {
        await _mediator.Send(new SignOutRequest());

        return NoContent();
    }

}
