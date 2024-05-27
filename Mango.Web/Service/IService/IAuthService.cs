using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface IAuthService
    {
        Task<ResponseDto?> LoginAsync(LoginRequestDto loginRequest);
        Task<ResponseDto?> RegistrationAsync(RegistrationRequestDto registrationRequest);
        Task<ResponseDto?> AssignRoleAsync(RegistrationRequestDto assignRoleRequest);
    }
}
