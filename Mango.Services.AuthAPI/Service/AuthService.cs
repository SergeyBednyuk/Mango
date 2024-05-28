using Mango.Services.AuthAPI.Data;
using Mango.Services.AuthAPI.Models;
using Mango.Services.AuthAPI.Models.Dtos;
using Mango.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;

namespace Mango.Services.AuthAPI.Service
{
    public sealed class AuthService : IAuthService
    {
        private readonly AuthAPIAppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(AuthAPIAppDbContext db, 
                           UserManager<ApplicationUser> userManager, 
                           RoleManager<IdentityRole> roleManager,
                           IJwtTokenGenerator jwtTokenGenerator)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<bool> AssignRole(string email, string roleName)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
            if (user != null) 
            {
                if (!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult()) 
                {
                    //create role if not exist
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }
                await _userManager.AddToRoleAsync(user, roleName);
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequest)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequest.UserName.ToLower());
            if (user != null)
            {
                var isValid = await _userManager.CheckPasswordAsync(user, loginRequest.UserPassword);

                if (!isValid)
                {
                    return new LoginResponseDto() {User = null, Token = string.Empty };
                }
                var roles = await _userManager.GetRolesAsync(user);
                var token = _jwtTokenGenerator.GenerateToken(user, roles);

                var userDto = new UserDto()
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber
                };

                LoginResponseDto loginResponseDto = new LoginResponseDto() 
                { 
                    User = userDto,
                    Token = token
                };

                return loginResponseDto;
            }
            return new LoginResponseDto();
        }

        public async Task<string> Register(RegistrationRequestDto registrationRequest)
        {
            var user = new ApplicationUser()
            {
                UserName = registrationRequest.Email,
                Email = registrationRequest.Email,
                NormalizedEmail = registrationRequest.Email.ToUpper(),
                Name = registrationRequest.Name,
                PhoneNumber = registrationRequest.PhoneNumber
            };

            try
            {
                var result = await _userManager.CreateAsync(user, registrationRequest.Password);
                if (result.Succeeded)
                {
                    var userToReturn = _db.ApplicationUsers.FirstOrDefault(u => u.UserName == registrationRequest.Email);
                    if (userToReturn != null)
                    {
                        var userDto = new UserDto()
                        {
                            Id = userToReturn.Id,
                            Name = userToReturn.Name,
                            Email = userToReturn.Email,
                            PhoneNumber = userToReturn.PhoneNumber
                        };

                        return string.Empty;
                    }
                }
                else 
                {
                    return result.Errors.FirstOrDefault().Description;
                }
            }
            catch (Exception ex)
            {

            }

            return "Error Encounted";
        }
    }
}
