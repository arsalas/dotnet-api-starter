using ApiStarter.Services;
using ApiStarter.Services.Interfaces;
using System.Reflection;

namespace ApiStarter.Extensions;

public static class ApplicationExtensions
{
	public static IServiceCollection AddApplicationServices(this IServiceCollection services)
	{
		services.AddAutoMapper(Assembly.GetExecutingAssembly());

		services.AddScoped<IAuthService, AuthService>();
		services.AddScoped<IUserService, UserService>();



		return services;
	}
}