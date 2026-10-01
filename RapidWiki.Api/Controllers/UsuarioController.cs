using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RapidWiki.Application.Common.Authorization;
using RapidWiki.Application.CreateUsuario;
using RapidWiki.Application.GetUsuarios;
using RapidWiki.Application.UpdateUsuario;
using RapidWiki.Application.UpdateUsuarioStatus;


namespace RapidWiki.Api.Controllers;

[ApiController]
[Route("app/v1/auth")]
public class UsuarioController : ControllerBase
{
    private readonly ISender _sender;
    public UsuarioController(ISender sender)
    {
        _sender = sender;
    }

    [Authorize(Roles = $"{Roles.Admin}")]
    [HttpPost("create_user")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUsuarioRequest request)
    {
        var result = await _sender.Send(request);
        return Created(string.Empty, result);
    }


    [Authorize(Roles=$"{Roles.Admin}")]
    [HttpPut("update_user")]
    public async Task<IActionResult> UpdateUsuario([FromBody] UpdateUsuarioRequest request)
    {
        var result = await _sender.Send(request);
        return Ok(result);
    }

    [Authorize(Roles=$"{Roles.Admin}")]
    [HttpPut("update_user_status")]
    public async Task<IActionResult> UpdateUsuarioStatus([FromBody] UpdateUsuarioStatusRequest request)
    {
        await _sender.Send(request);
        return NoContent();
    }

    [Authorize(Roles=$"{Roles.Admin}")]
    [HttpGet("get_users")]
    public async Task<IActionResult> GetUsers([FromQuery] int page = 1, [FromQuery] string? search = null)
    {
        var result = await _sender.Send(new GetUsuariosRequest(page, search));
        return Ok(result);
    }
}
