using Microsoft.AspNetCore.Mvc;

namespace RegisterAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (request.Email == "admin@gmail.com" && request.Password == "admin")
            {
                return Ok(new { message = "Login successful" });
            }
            return Unauthorized(new { message = "Invalid credentials" });
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
