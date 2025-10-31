namespace ApiStarter.DTOs.Auth;

public record AuthResponse(
		string Token,
		UserDto User);