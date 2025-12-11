#define ALLOW_CORS

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Booking.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

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

			app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
