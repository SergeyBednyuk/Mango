using Mango.Services.AuthAPI.Models.Dtos;

namespace Mango.Services.AuthAPI.Service.IService
{
    public interface IAuthService
    {
        Task<string> Register(RegistrationRequestDto registrationRequest);
        Task<LoginResponseDto> Login(LoginRequestDto loginRequest);
        Task<bool> AssignRole(string email, string roleName);
    }
}
