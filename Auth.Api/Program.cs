using Auth.Application.Repos;
using Auth.Application.Services;
using Auth.Infrastructure.Data;
using Auth.Infrastructure.Repository;
using Auth.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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
			builder.Services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = jwtSettings.Issuer,
					ValidAudience = jwtSettings.Audience,
					IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSettings.Secret))
				};
			});
			builder.Services.AddSingleton<IAccessTokenService, AccessTokenService>();
			builder.Services.AddScoped<IUserRepository, UserRepository>();
			builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
			builder.Services.AddScoped<IHasherService, HasherService>();
			builder.Services.AddScoped<IRefreshTokenGenerator, RefreshTokenGenerator>();
			builder.Services.AddScoped<IAuthService, AuthService>();

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

			app.UseHttpsRedirection();

			app.MapControllers();
			app.Run();
		}
	}
}