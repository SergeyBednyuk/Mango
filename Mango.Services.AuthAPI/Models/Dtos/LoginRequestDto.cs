namespace Mango.Services.AuthAPI.Models.Dtos
{
    public sealed record LoginRequestDto
    {
        public string UserName { get; init; }
        public string Password { get; init; }
    }
}
