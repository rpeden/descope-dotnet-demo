using Descope;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DescopeTestApp.Pages
{
    [Authorize]
    public class LogoutModel(DescopeClient authClient) : PageModel
    {
        private readonly DescopeClient _authClient = authClient;

        public async void OnGet()
        {
            var refreshToken = User.FindFirst("refreshToken")?.Value;

            if (refreshToken is not null) { 
                await _authClient.Auth.LogOut(refreshToken);
                await HttpContext.SignOutAsync();
            }
        }
    }
}
