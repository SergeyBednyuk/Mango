using System.ComponentModel.DataAnnotations;

namespace Mango.Web.Models
{
    public record RegistrationRequestDto
    {
        [Required]
        public string Name { get; init; }
        [Required]
        public string Email { get; init; }
        [Required]
        public string PhoneNumber { get; init; }
        [Required]
        public string Password { get; init; }
        public string? Role { get; init; }
    }
}
