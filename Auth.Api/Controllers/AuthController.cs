using Auth.Api.DTO;
using Auth.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService _authService;

		public AuthController(IAuthService authService)
		{
			_authService = authService;
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginDto dto)
		{
			var token = await _authService.LoginAsync(dto.Email, dto.Password);
			if (token == null) return Unauthorized();
			return Ok(new { Token = token });
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] LoginDto dto)
		{
			// simple validation
			if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
				return BadRequest("Email and password are required.");

			try
			{
				await _authService.RegisterAsync(dto.Email, dto.Password);
				return Ok(new { Message = "User registered successfully" });
			}
			catch (Exception ex)
			{
				// Например, пользователь с таким email уже существует
				return BadRequest(new { Error = ex.Message });
			}
		}

	}
}
