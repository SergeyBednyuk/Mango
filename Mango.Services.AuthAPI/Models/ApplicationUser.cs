using Microsoft.AspNetCore.Identity;

namespace Mango.Services.AuthAPI.Models
{
    public sealed class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
