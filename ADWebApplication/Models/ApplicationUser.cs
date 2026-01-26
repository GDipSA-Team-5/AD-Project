using Microsoft.AspNetCore.Identity;

namespace ADWebApplication.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; } = string.Empty;
        // Add other common profile fields here if needed
    }
}
