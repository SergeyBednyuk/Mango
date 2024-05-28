using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Mango.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenProvider _tokenProvider;
        public AuthController(IAuthService authService, ITokenProvider tokenProvider)
        {
            _authService = authService;
            _tokenProvider = tokenProvider;
        }

        [HttpGet]
        public ActionResult Login()
        {
            var loginRequest = new LoginRequestDto();
            return View(loginRequest);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto model)
        {
            var result = await _authService.LoginAsync(model);
            if (result != null && result.IsSuccess)
            {
                var loginResponseDto = 
                    JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(result.Result));
                await SignInAsync(loginResponseDto);
                _tokenProvider.SetToken(loginResponseDto.Token);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["error"] = result.Message;
                return View(model);
            }
        }

        [HttpGet]
        public ActionResult Registration()
        {
            //TMP
            var roleList = new List<SelectListItem>()
            {
                new SelectListItem() { Text = SD.RoleAdmin, Value = SD.RoleAdmin },
                new SelectListItem() { Text = SD.RoleCustomer, Value = SD.RoleCustomer }
            };

            ViewBag.RoleList = roleList;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationRequestDto model)
        {
            var result = await _authService.RegistrationAsync(model);
            if (result != null && result.IsSuccess)
            {
                //ResponseDto assignRole;
                //It's bad but I want to practice using of record for RegistrationRequestDto
                RegistrationRequestDto userModel;
                if (string.IsNullOrEmpty(model.Role))
                {
                    userModel = model with { Role = SD.RoleCustomer };
                }
                else
                {
                    userModel = model;
                }

                var assignRole = await _authService.AssignRoleAsync(userModel);
                if (assignRole != null && assignRole.IsSuccess)
                {
                    TempData["success"] = "Registaration is successful";
                }
                return RedirectToAction(nameof(Login));
            }
            else
            {
                TempData["error"] = result.Message;
            }
            //TMP
            var roleList = new List<SelectListItem>()
            {
                new SelectListItem() { Text = SD.RoleAdmin, Value = SD.RoleAdmin },
                new SelectListItem() { Text = SD.RoleCustomer, Value = SD.RoleCustomer }
            };

            ViewBag.RoleList = roleList;
            return View();
        }

        [HttpGet]
        public ActionResult AssignRole()
        {
            var assignRoleRequest = new RegistrationRequestDto();
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            _tokenProvider.ClearToken();
            return RedirectToAction("Index", "Home");
        }

        private async Task SignInAsync(LoginResponseDto model)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(model.Token);

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email, 
                                        jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub,
                            jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name,
                            jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Name).Value));

            identity.AddClaim(new Claim(ClaimTypes.Name,
                            jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(ClaimTypes.Role,
                jwt.Claims.FirstOrDefault(c => c.Type == "role").Value));

            var principle = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principle);
        }
    }
}
