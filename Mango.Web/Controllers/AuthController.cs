using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
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
            var response = await _authService.LoginAsync(model);
            if (response != null && response.IsSuccess)
            {
                var LoginResponseDto = 
                    JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(response.Result));

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("CustomError", response.Message);
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

        public ActionResult Logout()
        {
            var assignRoleRequest = new RegistrationRequestDto();
            return View();
        }

    }
}
