using Descope;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DescopeTestApp.Pages
{
    [Authorize]
    public class LogoutModel(DescopeClient authClient) : PageModel
    {
        private readonly DescopeClient _authClient = authClient;

        public void OnGet()
        {
            var refreshToken = User.FindFirst("refreshToken")?.Value;

            if (refreshToken is not null) { 
                _authClient.Auth.LogOut(refreshToken);
            }
        }
    }
}
