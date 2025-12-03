using Auth.Api;
using Auth.Api.DTO;
using Auth.Application.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Auth.Tests.Integration
{
	public class AuthIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
	{
		private readonly HttpClient _client;
		private readonly WebApplicationFactory<Program> _factory;

		public AuthIntegrationTests(WebApplicationFactory<Program> factory)
		{
			_client = factory.CreateClient();
			_factory = factory;
		}

		[Fact]
		public async Task Register_Then_Login_ShouldWork()
		{
			var email = "test@domain.com";
			var password = "123456";
			try
			{
				var registerDto = new LoginDto { Email = email, Password = password };

				// 1. Регистрируем пользователя
				var registerResponse = await _client.PostAsJsonAsync("/api/auth/register", registerDto);
				registerResponse.EnsureSuccessStatusCode();

				// 2. Логинимся
				var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", registerDto);
				loginResponse.EnsureSuccessStatusCode();

				var loginResult = await loginResponse.Content.ReadFromJsonAsync<JsonElement>();
				var token = loginResult.GetProperty("token").GetString();
				Assert.NotNull(token);
			}
			finally
			{
				// 3. Cleanup: удаляем пользователя
				using var scope = _factory.Services.CreateScope();
				var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();
				await authService.DeleteUserAsync("test@domain.com");
			}
		}
	}
}
