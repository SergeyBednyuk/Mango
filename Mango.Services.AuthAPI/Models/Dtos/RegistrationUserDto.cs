namespace Mango.Services.AuthAPI.Models.Dtos
{
    public sealed class RegistrationUserDto
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }
}
