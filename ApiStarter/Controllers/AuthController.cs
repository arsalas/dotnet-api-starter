using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApiStarter.DTOs.Auth;
using ApiStarter.Services.Interfaces;

namespace ApiStarter.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
	[HttpPost("register")]
	[AllowAnonymous]
	public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterDto dto)
	{
		var response = await authService.RegisterAsync(dto);
		return Ok(response);
	}

	[HttpPost("login")]
	[AllowAnonymous]
	public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginDto dto)
	{
		var response = await authService.LoginAsync(dto);
		return Ok(response);
	}
}