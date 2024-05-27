namespace Mango.Web.Models
{
    public record RegistrationRequestDto
    {
        public string Name { get; init; }
        public string Email { get; init; }
        public string PhoneNumber { get; init; }
        public string Password { get; init; }
        public string? Role { get; init; }
    }
}
