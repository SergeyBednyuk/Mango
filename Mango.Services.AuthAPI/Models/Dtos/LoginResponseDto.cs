namespace Mango.Services.AuthAPI.Models.Dtos
{
    public sealed class LoginResponseDto
    {
        public UserDto User { get; set; }
        public string Token { get; set; }
    }
}
