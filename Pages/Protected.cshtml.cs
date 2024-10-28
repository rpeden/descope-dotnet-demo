using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DescopeTestApp.Pages
{
    [Authorize]
    public class ProtectedModel : PageModel
    {
        public void OnGet()
        {
            var user = User;
            Console.WriteLine("hi");
        }
    }
}
