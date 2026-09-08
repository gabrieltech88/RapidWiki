using Microsoft.AspNetCore.Mvc;

namespace RapidWiki.Api.Controllers
{
   [ApiController]
   [Route("api/[controller]")]
   public class AuthController : ControllerBase
   {
       
       public AuthController()
       {
          
       }

       [HttpPost("register")]
       public async Task<IActionResult> Register([FromBody] RegisterRequest request)
       {
           
       }
   }
}