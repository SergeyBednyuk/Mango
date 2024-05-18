using Mango.Services.AuthAPI.Data;
using Mango.Services.AuthAPI.Models;
using Mango.Services.AuthAPI.Models.Dtos;
using Mango.Services.AuthAPI.Services.IService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Mango.Services.AuthAPI.Services
{
    public sealed class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthService(AppDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequest)
        {
            var user = await _db.ApplicationUsers.FirstOrDefaultAsync(u => u.Email.ToLower() == loginRequest.UserName.ToLower());
            var isValid = await _userManager.CheckPasswordAsync(user, loginRequest.Password);

            if (user == null || !isValid)
            {
                return new LoginResponseDto() {  User = null, Token = String.Empty};
            }

            //If user was found, generate JWT Token

            return new LoginResponseDto()
            {
               User = new UserDto()
               {
                   Id = user.Id,
                   Name = user.Name,
                   Email = user.Email,
                   PhoneNumber = user.PhoneNumber
               },
               Token = String.Empty
            };
        }

        public async Task<string> RegisterUser(RegistrationUserDto registrationUser)
        {
            var newUser = new ApplicationUser() 
            { 
                UserName = registrationUser.Email,
                Name = registrationUser.Name,
                Email = registrationUser.Email,
                NormalizedEmail = registrationUser.Email.ToUpper(),
                PhoneNumber = registrationUser.PhoneNumber
            };

            try
            {
                var result = await _userManager.CreateAsync(newUser, registrationUser.Password);
                if (result.Succeeded)
                {
                    var userToReturn = await _db.ApplicationUsers.FirstAsync(u => u.Email == registrationUser.Email);

                    var userDto = new UserDto()
                    {   
                        Id = userToReturn.Id,
                        Name = userToReturn.Name,
                        Email = userToReturn.Email,
                        PhoneNumber = userToReturn.PhoneNumber
                    };

                    return string.Empty;
                }
                else
                {
                    return result.Errors.FirstOrDefault().Description;
                }
            }
            catch (Exception ex)
            {

            }

            return "Error Encountered";
        }
    }
}
