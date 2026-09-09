using MediatR;
using Microsoft.AspNetCore.Mvc;
using RapidWiki.Api.Usuario.CreateUsuario;

namespace RapidWiki.Api.Controllers
{
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
   }
}