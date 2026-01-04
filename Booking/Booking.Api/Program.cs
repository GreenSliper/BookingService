#define ALLOW_CORS

using Booking.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Booking.Application.Repos;
using Booking.Infrastructure.Repos;
using Booking.Api.Middleware;
using Booking.Application.Commands.Properties;
using Booking.Application.Services;
using Booking.Infrastructure.Services;

namespace Booking.Api
{
    public class Program
    {
		static void ConfigureServices(WebApplicationBuilder builder)
		{
			// Add services to the container.
			builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
				{
					//enable HTTP
					options.RequireHttpsMetadata = false;
					options.Authority = builder.Configuration["Auth:Authority"];
					options.Audience = "BookingService";
					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuer = true,
						ValidateAudience = true,
						ValidateLifetime = true
					};
				});
			builder.Services.AddAuthorization(options =>
			{
				//options.AddPolicy
			});
			builder.Services.AddHttpContextAccessor();
			builder.Services.AddScoped<ICurrentUser, CurrentUser>();

			builder.Services.AddDbContext<BookingDbContext>(options =>
			{
				options.UseNpgsql(builder.Configuration.GetConnectionString("BookingDb"));
			});
			builder.Services.AddMediatR(cfg => {
				cfg.RegisterServicesFromAssemblyContaining<CreatePropertyCommand>();
			});
			builder.Services.AddScoped<IPropertyRepository, PropertyRepository>();
			builder.Services.AddScoped<IRoomRepository, RoomRepository>();
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
		}

		public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
			ConfigureServices(builder);

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
			if (app.Environment.IsProduction())
			{
				ApplyMigrations(app);
			}
			app.UseMiddleware<ExceptionHandlingMiddleware>();
			app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }

		static void ApplyMigrations(WebApplication app)
		{
			// Ensure migrations are applied
			using (var scope = app.Services.CreateScope())
			{
				var db = scope.ServiceProvider.GetRequiredService<BookingDbContext>();
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
						Thread.Sleep(1000);
					}
				}
			}
		}
	}
}
