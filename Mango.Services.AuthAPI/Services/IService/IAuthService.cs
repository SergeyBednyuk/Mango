using Mango.Services.AuthAPI.Models.Dtos;

namespace Mango.Services.AuthAPI.Services.IService
{
    public interface IAuthService
    {
        Task<string> RegisterUser(RegistrationUserDto registrationUser);
        Task<LoginResponseDto> Login(LoginRequestDto loginRequest); 

    }
}
