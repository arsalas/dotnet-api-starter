using ApiStarter.DTOs.Users;

namespace ApiStarter.Services.Interfaces;

public interface IUserService
{
	Task<UserDto> CreateAsync(string email, string password, string? firstName = null, string? lastName = null);
	Task<UserDto?> GetByIdAsync(string id);
	Task<UserDto?> GetByEmailAsync(string email);
	Task<List<UserDto>> GetAllAsync();
	Task<UserDto> UpdateAsync(string id, UpdateUserDto dto);
	Task DeleteAsync(string id);
}
