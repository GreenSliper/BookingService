#define ALLOW_CORS

using Auth.Application.Repos;
using Auth.Application.Services;
using Auth.Infrastructure.Data;
using Auth.Infrastructure.Repository;
using Auth.Infrastructure.Services;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Stores;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Auth.Api
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddDbContext<AuthDbContext>(options =>
			{
				options.UseNpgsql(builder.Configuration.GetConnectionString("AuthDb"));
			});
			var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
			builder.Services.AddSingleton(jwtSettings!);
			//add .well-known/openid-configuration - public key access
			builder.Services.AddIdentityServer()
				//.AddInMemoryApiScopes(new[] { new ApiScope("booking") }) - scopes are not needed now
				.AddDeveloperSigningCredential();
			builder.Services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer();
			
			builder.Services.AddSingleton<IAccessTokenService, AccessTokenService>();
			builder.Services.AddScoped<IUserRepository, UserRepository>();
			builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
			builder.Services.AddScoped<IHasherService, HasherService>();
			builder.Services.AddScoped<IRefreshTokenGenerator, RefreshTokenGenerator>();
			builder.Services.AddScoped<IAuthService, AuthService>();

#if ALLOW_CORS
			builder.Services.AddCors(options =>
			{
				options.AddDefaultPolicy(policy =>
				{
					policy.AllowAnyOrigin()
						  .AllowAnyMethod()
						  .AllowAnyHeader();
				});
			});
#endif

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

#if ALLOW_CORS
			app.UseCors();
#endif
			ApplyMigrations(app);
			app.UseIdentityServer();
			var signingCredentials = app.Services
				.GetRequiredService<ISigningCredentialStore>()
				.GetSigningCredentialsAsync().Result;
			app.UseAuthentication();
			app.Use(async (context, next) =>
			{
				// Получаем текущую схему
				var opts = context.RequestServices
					.GetRequiredService<IOptionsMonitor<JwtBearerOptions>>()
					.Get(JwtBearerDefaults.AuthenticationScheme);

				// Вставляем ключ IdentityServer
				opts.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidIssuer = jwtSettings.Issuer,
					ValidateAudience = true,
					ValidAudience = jwtSettings.Audience,
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = signingCredentials.Key,
					ValidateLifetime = true
				};

				await next();
			});
			app.UseAuthorization();
			app.UseHttpsRedirection();

			app.MapControllers();
			app.Run();
		}

		static void ApplyMigrations(WebApplication app)
		{
			// Ensure migrations are applied
			using (var scope = app.Services.CreateScope())
			{
				var db = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
				var retries = 10;
				while (retries > 0)
				{
					try
					{
						db.Database.Migrate();
						break;
					}
					catch (Npgsql.NpgsqlException)
					{
						retries--;
						Console.WriteLine("Waiting for database...");
						Thread.Sleep(3000); // 3 секунды
					}
				}
			}
		}
	}
}