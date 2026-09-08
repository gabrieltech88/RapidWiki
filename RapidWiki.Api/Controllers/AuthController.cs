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
           var user = new IdentityUser<Guid>
           {
               UserName = request.Email,
               Email = request.Email
           };

           var result = await _userManager.CreateAsync(user, request.Password);

           if (!result.Succeeded)
           {
               return BadRequest(result.Errors);
           }

           return Ok();
       }

       [HttpPost("login")]
       public async Task<IActionResult> Login([FromBody] LoginRequest request)
       {
           var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, false, false);

           if (!result.Succeeded)
           {
               return Unauthorized();
           }

           return Ok();
       }
   }
}