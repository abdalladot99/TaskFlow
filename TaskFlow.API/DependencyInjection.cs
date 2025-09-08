using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using TaskFlow.Application;
using TaskFlow.Core;
using TaskFlow.Infrastructure;

namespace TaskFlow.API
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddAPIDI(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddApplicationDI()
				.AddInfrastructureDI(configuration)
				.AddCoreDI();


			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(options =>
			{
				options.SaveToken = true;
				options.RequireHttpsMetadata = false;
				options.TokenValidationParameters = new TokenValidationParameters()
				{ 
					ValidateIssuer = true,
					ValidIssuer = configuration["JWT:ValidIssuer"],
					ValidateAudience = true,
					ValidAudience = configuration["JWT:ValidAudience"],
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))

				};
			});

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

				// Add JWT Authentication to Swagger
				c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					In = ParameterLocation.Header,
					Description = "Please enter 'Bearer' followed by a space and then your token",
					Name = "Authorization",
					Type = SecuritySchemeType.ApiKey,
					BearerFormat = "JWT",
					Scheme = "Bearer"
				});

				c.AddSecurityRequirement(new OpenApiSecurityRequirement {
				{
					new OpenApiSecurityScheme
					{
						Reference = new OpenApiReference
						{
							Type = ReferenceType.SecurityScheme,
							Id = "Bearer"
						}
					},
					new string[] { }
				}
				});
			});


			return services;
		}
	}
}
