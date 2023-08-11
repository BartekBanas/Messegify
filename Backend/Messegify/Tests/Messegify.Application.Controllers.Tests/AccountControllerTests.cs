using Moq;
using Messegify.Application.Dtos;
using Messegify.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Messegify.Application.Controllers.Tests
{
    public class AccountControllerTests
    {
        private readonly Mock<IAccountService> _mockAccountService;
        private readonly AccountController _controller;

        public AccountControllerTests()
        {
            _mockAccountService = new Mock<IAccountService>();
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            _controller = new AccountController(_mockAccountService.Object, mockHttpContextAccessor.Object);
        }

        [Fact]
        public async Task RegisterAccount_ValidDto_ReturnsOk()
        {
            // Arrange
            var validDto = new RegisterAccountDto
            {
                Username = "Username",
                Password = "Password",
                Email = "email.com"
            };

            // Act
            var result = await _controller.RegisterAccount(validDto) as OkResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }
    }
}
