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
			try
			{
				(var accessToken, var refreshToken) = await _authService.LoginAsync(dto.Email, dto.Password);
				return Ok(new AccessRefreshDto{ AccessToken = accessToken, RefreshToken = refreshToken });
			}
			catch
			{
				return Unauthorized();
			}
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
				return BadRequest(new { Error = ex.Message });
			}
		}

		[HttpPost("refresh")]
		public async Task<IActionResult> Refresh([FromBody] AccessRefreshDto request)
		{
			var (access, refresh) = await _authService.RefreshAsync(request.AccessToken, request.RefreshToken);
			return Ok(new { accessToken = access, refreshToken = refresh });
		}
	}
}
