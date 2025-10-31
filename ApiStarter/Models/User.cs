using Microsoft.AspNetCore.Identity;

namespace ApiStarter.Models;

public class User : IdentityUser
{
	public string? AvatarUrl { get; set; }
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}