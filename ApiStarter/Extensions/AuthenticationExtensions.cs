using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;


namespace ApiStarter.Extensions;

public static class AuthenticationExtensions
{
	public static IServiceCollection AddAuthenticationServices(
			this IServiceCollection services,
			IConfiguration configuration)
	{
		var jwtSettings = configuration.GetSection("Jwt");
		var jwtKey = jwtSettings["Key"]
				?? throw new InvalidOperationException("JWT Key no configurada");
		var jwtIssuer = jwtSettings["Issuer"]
				?? throw new InvalidOperationException("JWT Issuer no configurado");

		services.AddAuthentication(options =>
		{
			options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
		})
		.AddJwtBearer(options =>
		{
			options.TokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuer = true,
				ValidateAudience = true,
				ValidateLifetime = true,
				ValidateIssuerSigningKey = true,
				ValidIssuer = jwtIssuer,
				ValidAudience = jwtIssuer,
				IssuerSigningKey = new SymmetricSecurityKey(
									Encoding.UTF8.GetBytes(jwtKey)),
				ClockSkew = TimeSpan.Zero
			};

			options.Events = new JwtBearerEvents
			{
				OnAuthenticationFailed = context =>
						{
							if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
							{
								context.Response.Headers.Append("Token-Expired", "true");
							}
							return Task.CompletedTask;
						}
			};
		});

		return services;
	}
}