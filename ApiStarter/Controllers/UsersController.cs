using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ApiStarter.DTOs.Users;
using ApiStarter.Services.Interfaces;

namespace ApiStarter.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]
public class UsersController(IUserService userService) : ControllerBase
{
	[HttpGet("me")]
	public async Task<ActionResult<UserDto>> GetMyProfile()
	{
		var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

		if (userId == null)
			return Unauthorized();

		var user = await userService.GetByIdAsync(userId);

		if (user == null)
			return NotFound();

		return Ok(user);
	}

	[HttpPut("me")]
	public async Task<ActionResult<UserDto>> UpdateMyProfile([FromBody] UpdateUserDto dto)
	{
		var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

		if (userId == null)
			return Unauthorized();

		var user = await userService.UpdateAsync(userId, dto);
		return Ok(user);
	}

	[HttpGet]
	[Authorize(Roles = "Admin")]
	public async Task<ActionResult<List<UserDto>>> GetAll()
	{
		var users = await userService.GetAllAsync();
		return Ok(users);
	}

	[HttpDelete("{id}")]
	[Authorize(Roles = "Admin")]
	public async Task<ActionResult> Delete(string id)
	{
		await userService.DeleteAsync(id);
		return NoContent();
	}
}