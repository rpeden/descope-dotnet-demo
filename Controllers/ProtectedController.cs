using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Security.Claims;

namespace DescopeTestApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]  // Protects this controller with authentication
    public class ProtectedController : ControllerBase
    {
        [HttpGet("user-profile")]
        [Produces(MediaTypeNames.Application.Json)]
        public IActionResult GetUserProfile()
        {
            // Fake user data for demonstration
            var userProfile = new UserProfileDTO
            {
                UserId = "12345",
                Name = User.Identity?.Name ?? "Unknown",
                Email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value ?? "unknown@example.com",
                Role = "Member",
                JoinedDate = DateTime.UtcNow.AddYears(-2),
                LastLogin = DateTime.UtcNow.AddHours(-5)
            };

            return Ok(userProfile);
        }
    }

    // DTO for user profile
    public class UserProfileDTO
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public DateTime JoinedDate { get; set; }
        public DateTime LastLogin { get; set; }
    }
}
