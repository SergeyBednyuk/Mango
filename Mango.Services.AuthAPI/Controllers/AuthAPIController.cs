using Mango.Services.AuthAPI.Models.Dtos;
using Mango.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private readonly IAuthService _authService;
        protected ResponseDto _response;
        public AuthAPIController(IAuthService authService)
        {
            _authService = authService;
            _response = new ResponseDto();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto model)
        {
            var result = await _authService.Register(model);
            if (!string.IsNullOrEmpty(result))
            {
                _response.IsSuccess = false;
                _response.Message = result;
                return BadRequest(result);
            }
            return Ok(_response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            var result = await _authService.Login(model);
            if (result.User == null)
            {
                _response.IsSuccess = false;
                _response.Message = "UserName or Password is incorrect";
                return BadRequest(_response);
            }
            _response.Result = result;
            return Ok(result);
        }

        [HttpPost("assignrole")]
        public async Task<IActionResult> AssignToRole([FromBody] RegistrationRequestDto model)
        {
            var result = await _authService.AssignRole(model.Email, model.Role.ToUpper());
            if (!result)
            {
                _response.IsSuccess = false;
                _response.Message = "Error encourted";
                return BadRequest(_response);
            }
            return Ok(result);
        }
    }
}
