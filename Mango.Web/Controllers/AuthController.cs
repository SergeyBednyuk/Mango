using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet]
        public ActionResult Registration()
        {
            var registrationRequest = new RegistrationRequestDto();
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
