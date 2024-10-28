using Descope;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace DescopeTestApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RefreshController : Controller
    {
        private readonly DescopeClient _authClient;

        public RefreshController(DescopeClient authClient)
        {
            _authClient = authClient;
        }

        [HttpPost("refresh-token")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.JwtToken) || string.IsNullOrWhiteSpace(request.RefreshToken))
            {
                return BadRequest(new { message = "JWT token and refresh token are required." });
            }

            try
            {
                // Validate and refresh the session using Descope SDK
                var token = await _authClient.Auth.ValidateAndRefreshSession(request.JwtToken, request.RefreshToken);

                return Ok(new
                {
                    jwtToken = token.Jwt,
                });
            }
            catch (DescopeException ex)
            {
                Console.WriteLine($"Token refresh failed: {ex.Message}");
                return Unauthorized(new { message = "Invalid token or refresh failed." });
            }
        }
    }

    // DTO for the request body
    public class RefreshRequest
    {
        public string JwtToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
