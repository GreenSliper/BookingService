using Auth.Api.Controllers;
using Auth.Api.DTO;
using Auth.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Tests
{
	public class AuthControllerTests
	{
		[Fact]
		public async Task Register_ReturnsOk_WhenSuccessful()
		{
			// Arrange
			var authServiceMock = new Mock<IAuthService>();
			authServiceMock.Setup(x => x.RegisterAsync(It.IsAny<string>(), It.IsAny<string>()))
				.Returns(Task.CompletedTask);

			var controller = new AuthController(authServiceMock.Object);
			var dto = new LoginDto { Email = "test@example.com", Password = "123456" };

			// Act
			var result = await controller.Register(dto);

			// Assert
			var okResult = Assert.IsType<OkObjectResult>(result);
			Assert.Equal(200, okResult.StatusCode);
		}

		[Fact]
		public async Task Register_ReturnsBadRequest_WhenEmailOrPasswordIsEmpty()
		{
			// Arrange
			var authServiceMock = new Mock<IAuthService>();
			var controller = new AuthController(authServiceMock.Object);
			var dto = new LoginDto { Email = "", Password = "" };

			// Act
			var result = await controller.Register(dto);

			// Assert
			var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
			Assert.Equal(400, badRequestResult.StatusCode);
		}

		[Fact]
		public async Task Register_ReturnsBadRequest_WhenAuthServiceThrows()
		{
			// Arrange
			var authServiceMock = new Mock<IAuthService>();
			authServiceMock.Setup(x => x.RegisterAsync(It.IsAny<string>(), It.IsAny<string>()))
				.ThrowsAsync(new System.Exception("User already exists"));

			var controller = new AuthController(authServiceMock.Object);
			var dto = new LoginDto { Email = "test@example.com", Password = "123456" };

			// Act
			var result = await controller.Register(dto);

			// Assert
			var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
			Assert.Equal(400, badRequestResult.StatusCode);
		}

		[Fact]
		public async Task Login_ReturnsOk_WhenCredentialsCorrect()
		{
			var authServiceMock = new Mock<IAuthService>();
			authServiceMock.Setup(x => x.LoginAsync(It.IsAny<string>(), It.IsAny<string>()))
				.ReturnsAsync(("mock-access-token", "mock-refresh-token"));

			var controller = new AuthController(authServiceMock.Object);
			var dto = new LoginDto { Email = "test@example.com", Password = "123456" };

			var result = await controller.Login(dto);

			var okResult = Assert.IsType<OkObjectResult>(result);
			Assert.Equal(200, okResult.StatusCode);
		}
	}
}
