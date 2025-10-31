using ApiStarter.Middleware;

namespace ApiStarter.Extensions;

public static class PipelineExtensions
{
	public static WebApplication ConfigurePipeline(this WebApplication app)
	{
		app.UseMiddleware<ExceptionHandlingMiddleware>();

		// Desarrollo
		if (app.Environment.IsDevelopment())
		{
			app.MapOpenApi();
			app.MapGet("/", () => Results.Redirect("/scalar/v1"))
					.ExcludeFromDescription();
		}

		// Producción
		if (app.Environment.IsProduction())
		{
			app.UseHsts();
		}

		app.UseHttpsRedirection();
		app.UseAuthentication();
		app.UseAuthorization();
		app.MapControllers();

		return app;
	}
}