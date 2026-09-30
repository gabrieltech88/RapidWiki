using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RapidWiki.Application.ChangePassword;
using RapidWiki.Application.Common.Authorization;
using RapidWiki.Application.CreateRole;
using RapidWiki.Application.GetCurrentUser;
using RapidWiki.Application.SignIn;
using RapidWiki.Application.SignOut;

namespace RapidWiki.Api.Controllers;

[ApiController]
[Route("app/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly ISender _sender;

    public AuthController(ISender sender)
    {
        _sender = sender;
    }

    [Authorize(Roles = $"{Roles.Admin}")]
    [HttpPost("create_role")]
    public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest request)
    {
        var result = await _sender.Send(request);
        return Created(string.Empty, result);
    }

    [HttpPost("sign_in")]
    public async Task<IActionResult> SignIn([FromBody] SignInRequest request)
    {
        var result = await _sender.Send(request);
        return Ok(result);
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        var result = await _sender.Send(new GetCurrentUserRequest());
        return Ok(result);
    }

    [Authorize]
    [HttpPut("change_password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        await _sender.Send(request);
        return NoContent();
    }

    [Authorize]
    [HttpPost("sign_out")]
    public async Task<IActionResult> SignOutUser()
    {
        await _sender.Send(new SignOutRequest());
        return NoContent();
    }
}