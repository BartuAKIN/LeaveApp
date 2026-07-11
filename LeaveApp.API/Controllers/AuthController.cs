using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LeaveApp.Business.DTOs;
using LeaveApp.Business.Interfaces;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization;

namespace LeaveApp.API.Controllers
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var result = await _authService.Register(registerDto);
            if (!result) return BadRequest("Kayıt Başarısız");
            return Ok("Kayıt Başarılı");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var token = await _authService.Login(loginDto);
            if (token == null) return Unauthorized("Geçersiz email veya şifre");
            return Ok(new { token });
        }
}
}
