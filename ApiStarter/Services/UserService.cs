using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using ApiStarter.DTOs.Users;
using ApiStarter.Models;
using ApiStarter.Exceptions;
using ApiStarter.Services.Interfaces;

namespace ApiStarter.Services;

public class UserService(
		UserManager<User> userManager,
		IMapper mapper,
		ILogger<UserService> logger)
		: IUserService
{
	public async Task<UserDto> CreateAsync(
			string email,
			string password,
			string? firstName = null,
			string? lastName = null)
	{

		var existingUser = await userManager.FindByEmailAsync(email);
		if (existingUser != null)
		{
			throw new ValidationException("El email ya está registrado");
		}

		var user = new User
		{
			UserName = email,
			Email = email,
			AvatarUrl = "",
			CreatedAt = DateTime.UtcNow
		};

		var result = await userManager.CreateAsync(user, password);

		if (!result.Succeeded)
		{

			var errors = string.Join(", ", result.Errors.Select(e => e.Description));
			logger.LogWarning("Error al crear usuario {Email}: {Errors}", email, errors);
			throw new ValidationException(errors);
		}

		logger.LogInformation("Usuario creado: {Email}", email);

		return mapper.Map<UserDto>(user);
	}

	public async Task<UserDto?> GetByIdAsync(string id)
	{
		var user = await userManager.FindByIdAsync(id);
		return user == null ? null : mapper.Map<UserDto>(user);
	}

	public async Task<UserDto?> GetByEmailAsync(string email)
	{
		var user = await userManager.FindByEmailAsync(email);
		return user == null ? null : mapper.Map<UserDto>(user);
	}

	public async Task<List<UserDto>> GetAllAsync()
	{
		var users = await userManager.Users
				.OrderByDescending(u => u.CreatedAt)
				.ToListAsync();

		return mapper.Map<List<UserDto>>(users);
	}

	public async Task<UserDto> UpdateAsync(string id, UpdateUserDto dto)
	{
		var user = await userManager.FindByIdAsync(id);

		if (user == null)
		{
			throw new NotFoundException($"Usuario con id {id} no encontrado");
		}

		// Mapear cambios
		mapper.Map(dto, user);

		var result = await userManager.UpdateAsync(user);

		if (!result.Succeeded)
		{
			var errors = string.Join(", ", result.Errors.Select(e => e.Description));
			throw new ValidationException($"Error al actualizar usuario: {errors}");
		}

		logger.LogInformation("Usuario actualizado: {Id}", id);

		return mapper.Map<UserDto>(user);
	}

	public async Task DeleteAsync(string id)
	{
		var user = await userManager.FindByIdAsync(id);

		if (user == null)
		{
			throw new NotFoundException($"Usuario con id {id} no encontrado");
		}

		var result = await userManager.DeleteAsync(user);

		if (!result.Succeeded)
		{
			var errors = string.Join(", ", result.Errors.Select(e => e.Description));
			throw new ValidationException($"Error al eliminar usuario: {errors}");
		}

		logger.LogInformation("Usuario eliminado: {Id}", id);
	}
}