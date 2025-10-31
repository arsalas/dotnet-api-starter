using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ApiStarter.Models;
using ApiStarter.DTOs.Auth;
using ApiStarter.DTOs.Users;
using ApiStarter.Services.Interfaces;
using ApiStarter.Exceptions;

namespace ApiStarter.Services;

public class AuthService(
		IUserService userService,
		UserManager<User> userManager,
		SignInManager<User> signInManager,
		IConfiguration configuration,
		ILogger<AuthService> logger)
		: IAuthService
{

	public async Task<AuthResponse> RegisterAsync(RegisterDto dto)
	{
		var userDto = await userService.CreateAsync(
				dto.Email,
				dto.Password);

		var token = GenerateJwtToken(userDto);

		logger.LogInformation("Usuario registrado: {Email}", dto.Email);

		return new AuthResponse(token, userDto);
	}

	public async Task<AuthResponse> LoginAsync(LoginDto dto)
	{
		var user = await userManager.FindByEmailAsync(dto.Email);

		if (user == null)
		{
			logger.LogWarning("Intento de login con email inexistente: {Email}", dto.Email);
			throw new UnauthorizedException("Credenciales inválidas"); // 👈 No dar pistas
		}

		var result = await signInManager.CheckPasswordSignInAsync(
				user,
				dto.Password,
				lockoutOnFailure: true); // 👈 Bloquea después de X intentos

		if (result.IsLockedOut)
		{
			logger.LogWarning("Usuario bloqueado: {Email}", dto.Email);
			throw new UnauthorizedException("Usuario bloqueado temporalmente. Intenta más tarde.");
		}

		if (!result.Succeeded)
		{
			logger.LogWarning("Contraseña incorrecta para: {Email}", dto.Email);
			throw new UnauthorizedException("Credenciales inválidas");
		}

		var userDto = await userService.GetByEmailAsync(dto.Email);

		if (userDto == null)
		{
			logger.LogError("Usuario autenticado pero no encontrado en UserService: {Email}", dto.Email);
			throw new Exception("Error de sincronización");
		}

		var token = GenerateJwtToken(userDto);

		logger.LogInformation("Login exitoso: {Email}", dto.Email);

		return new AuthResponse(token, userDto);
	}

	private string GenerateJwtToken(UserDto user)
	{
		var claims = new[]
		{
						new Claim(ClaimTypes.NameIdentifier, user.Id),
						new Claim(ClaimTypes.Email, user.Email),
        };

		var key = new SymmetricSecurityKey(
				Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));

		var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

		var token = new JwtSecurityToken(
				issuer: configuration["Jwt:Issuer"],
				audience: configuration["Jwt:Issuer"],
				claims: claims,
				expires: DateTime.UtcNow.AddHours(24),
				signingCredentials: credentials);

		return new JwtSecurityTokenHandler().WriteToken(token);
	}
}