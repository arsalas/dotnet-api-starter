using Microsoft.AspNetCore.Mvc;

namespace ApiStarter.Extensions;

public static class WebExtensions
{
	public static IServiceCollection AddWebServices(this IServiceCollection services)
	{
		services.AddControllers()
				.ConfigureApiBehaviorOptions(options =>
				{
					options.InvalidModelStateResponseFactory = context =>
							{
							var errors = context.ModelState.Values
											.SelectMany(v => v.Errors)
											.Select(e => e.ErrorMessage)
											.ToList();

							var errorMessage = string.Join(", ", errors);

							return new BadRequestObjectResult(new
							{
								statusCode = 400,
								message = errorMessage
							});
						};
				});
		services.AddOpenApi();

		services.AddCors(options =>
		{
			options.AddPolicy("AllowFrontend", policy =>
					{
					policy.WithOrigins("http://localhost:3000") // Tu frontend
									.AllowAnyMethod()
									.AllowAnyHeader()
									.AllowCredentials();
				});
		});

		return services;
	}
}