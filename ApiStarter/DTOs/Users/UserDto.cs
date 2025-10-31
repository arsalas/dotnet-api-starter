namespace ApiStarter.DTOs.Users;

public record UserDto(
		string Id,
		string Email,
		// string? FirstName,
		// string? LastName,
		// string? PhoneNumber,
		string? AvatarUrl,
		DateTime CreatedAt);