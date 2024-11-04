using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DescopeTestApp.Pages
{
    [Authorize]
    public class ProfileModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
