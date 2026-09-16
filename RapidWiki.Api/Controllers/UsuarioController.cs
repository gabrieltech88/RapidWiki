using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RapidWiki.Application.Common.Authorization;
using RapidWiki.Application.CreateRole;
using RapidWiki.Application.CreateUsuario;
using RapidWiki.Application.GetCurrentUser;
using RapidWiki.Application.GetUsuarios;
using RapidWiki.Application.SignIn;
using RapidWiki.Application.SignOut;
using RapidWiki.Application.UpdateUsuario;


namespace RapidWiki.Api.Controllers;

[ApiController]
[Route("app/v1/auth")]
public class UsuarioController : ControllerBase
{
    private readonly IMediator _mediator;
    public UsuarioController(IMediator mediator)
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


    [Authorize(Roles=$"{Roles.Admin}")]
    [HttpPut("update_user")]
    public async Task<IActionResult> UpdateUsuario([FromBody] UpdateUsuarioRequest request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
    }

    [Authorize(Roles=$"{Roles.Admin}")]
    [HttpGet("get_users")]
    public async Task<IActionResult> GetUsers([FromQuery] int page = 1, [FromQuery] string? search = null)
    {
        var result = await _mediator.Send(new GetUsuariosRequest(page, search));
        return Ok(result);
    }

}
