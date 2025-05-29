using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegisterAPI.Data;
using RegisterAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace RegisterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserProfileController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/UserProfile
        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] UserProfile user)
        {
            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(user, serviceProvider: null, items: null);
            if (!Validator.TryValidateObject(user, context, validationResults, true))
            {
                return BadRequest(validationResults);
            }

            bool emailExists = await _context.UserProfiles.AnyAsync(u => u.Email == user.Email);
            if (emailExists)
            {
                return BadRequest(new { message = "Email already exists." });
            }

            _context.UserProfiles.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "User profile saved successfully!", user });
        }

        // GET: api/UserProfile
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserProfile>>> GetAllUsers()
        {
            var users = await _context.UserProfiles.ToListAsync();
            return Ok(users);
        }

        // PUT: api/UserProfile
        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] UserProfile updatedUser)
        {
            if (string.IsNullOrEmpty(updatedUser.Email))
            {
                return BadRequest(new { message = "Email is required to update profile." });
            }

            var existingUser = await _context.UserProfiles.FirstOrDefaultAsync(u => u.Email == updatedUser.Email);
            if (existingUser == null)
            {
                return NotFound(new { message = "User not found." });
            }

            // Update fields
            existingUser.FullName = updatedUser.FullName;
            existingUser.Phone = updatedUser.Phone;
            existingUser.Address = updatedUser.Address;
            existingUser.Gender = updatedUser.Gender;
            existingUser.Birthday = updatedUser.Birthday;

            await _context.SaveChangesAsync();

            return Ok(new { message = "User profile updated successfully!", user = existingUser });
        }
    }
}
