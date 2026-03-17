using Microsoft.AspNetCore.Mvc;
using Salguri.Application.DTOS;
using Salguri.Application.Interfaces;
using System.Runtime.InteropServices;

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
        public async Task<IActionResult> Register([FromBody] UserRequestDTO request)
        {
            var result = await _authService.RegisterAsync(request);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDTO request)
        {
            var result = await _authService.LoginAsync(request);
            return Ok(result);
        }


        [HttpPost("VerifyOtp")]
        public async Task<IActionResult> VerifyOtp(VerifyOtpRequest request)
        {
            var result = await _authService.VerifyOtp(request);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("ResendOtp")]
        public async Task<IActionResult> ResendOtp([FromBody] ResendOtpRequestDTO request)
        {
            var result = await _authService.ResendOtp(request);

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPost("EnrollTotp")]
        public async Task<IActionResult> EnrollTotp()
        {
            // Get access token from Authorization header
            // Frontend sends this after login/registration
            // Format is: "Bearer eyJhbG..."
            var accessToken = Request.Headers["Authorization"]
                .ToString()
                .Replace("Bearer ", "");
            var refreshToken = Request.Headers["X-Refresh-Token"].ToString();

            if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
                return Unauthorized("Both access token and refresh token are required");

            var result = await _authService.EnrollTotpAsync(accessToken,refreshToken);

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPost("VerifyTotp")]
        public async Task<IActionResult> VerifyTotp([FromBody] TotpVerifyRequestDTO request)
        {
            var accessToken = Request.Headers["Authorization"]
                .ToString()
                .Replace("Bearer ", "");
            var refreshToken = Request.Headers["X-Refresh-Token"].ToString();

            if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
                return Unauthorized("Both access token and refresh token are required");

            var result = await _authService.VerifyTotpAsync(request, accessToken,refreshToken);

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }
    }
}

