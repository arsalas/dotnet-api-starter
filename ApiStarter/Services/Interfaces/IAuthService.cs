using ApiStarter.DTOs.Auth;

namespace ApiStarter.Services.Interfaces;

public interface IAuthService
{
	Task<AuthResponse> RegisterAsync(RegisterDto model);
	Task<AuthResponse> LoginAsync(LoginDto model);
}