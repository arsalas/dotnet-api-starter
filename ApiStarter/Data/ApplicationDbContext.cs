using ApiStarter.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApiStarter.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
		: IdentityDbContext<User>(options)
{
	// Tus tablas
	// public DbSet<Course> Courses { get; set; }
}