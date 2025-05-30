using Microsoft.AspNetCore.Mvc;

namespace RegisterAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorController : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // Simple hardcoded check for doctor credentials
            if (request.Email == "doctor@gmail.com" && request.Password == "doctor")
            {
                return Ok(new { message = "Login successful" });
            }
            return Unauthorized(new { message = "Invalid credentials" });
        }
    }

    // Reuse the same DTO structure as AdminController
    public class DoctorRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
