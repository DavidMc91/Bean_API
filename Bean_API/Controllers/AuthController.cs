using Bean_API.Services;
using Microsoft.AspNetCore.Mvc;
using Bean_API.Models;

namespace Bean_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthService _authService;

        public AuthController(ILogger<AuthController> logger, IAuthService authService)
        {
            _logger = logger;
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] Login model)
        {
            try
            {
                if (model.Username == "user" && model.Password == "password") //TODO: Replace with real validation
                {
                    var token = _authService.GenerateToken(model.Username);
                    return Ok(new { Token = token });
                }

                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred");
                return StatusCode(500); //500 Internal Server Error if something goes wrong  
            }                                                      
        }
    }
}
