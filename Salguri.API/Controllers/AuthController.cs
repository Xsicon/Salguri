using Microsoft.AspNetCore.Mvc;
using Salguri.Application.DTOS;
using Salguri.Application.Interfaces;

namespace Salguri.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRequestDTO request)
        {
            var result = await _authService.RegisterAsync(request);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDTO request)
        {
            var result = await _authService.LoginAsync(request);
            return Ok(result);
        }

    }
}

