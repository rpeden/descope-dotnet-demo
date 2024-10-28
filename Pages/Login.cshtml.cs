using Descope;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace DescopeTestApp.Pages
{
    public class LoginModel(DescopeClient authClient) : PageModel
    {
        private readonly DescopeClient _authClient = authClient;

        [BindProperty]
        public string JwtToken { get; set; }
        [BindProperty]
        public string RefreshToken { get; set; }

        public void OnGet()
        {
            Console.WriteLine("Getting");
        }

        public async Task<IActionResult> OnPostAsync()//[FromBody]string jwtToken, [FromBody]string refreshToken) 
        {
            Console.WriteLine("Posting");
            try
            {
                var sessionInfo = await _authClient.Auth.ValidateAndRefreshSession(JwtToken, RefreshToken);
                var user = await _authClient.Auth.Me(RefreshToken);
               
                // Create claims for the authenticated user

                Claim[] claims = [
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("sessionToken", JwtToken),
                    new Claim("refreshToken", RefreshToken),
                ];

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(principal);

                return RedirectToPage("/Index");
            }
            catch (DescopeException ex)
            {
                // Authentication failed
                Console.WriteLine($"Login failed: {ex.Message}");
                return RedirectToPage("/Login");
            }
        }
    }
}
