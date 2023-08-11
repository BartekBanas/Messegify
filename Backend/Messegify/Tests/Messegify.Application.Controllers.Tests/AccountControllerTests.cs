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

        [Fact]
        public async Task Authenticate_ValidDto_ReturnsOkWithToken()
        {
            // Arrange
            var validDto = new LoginDto
            {
                 UsernameOrEmail = "Bob",
                 Password = "Password"
            };
            var expectedToken = "sampleToken";
            _mockAccountService.Setup(accountService => accountService.AuthenticateAsync(validDto)).ReturnsAsync(expectedToken);

            // Act
            var result = await _controller.Authenticate(validDto) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(expectedToken, result.Value);
        }

        [Fact]
        public async Task GetAllAccounts_ReturnsOkWithAccountDtos()
        {
            // Arrange
            var expectedAccountDtos = new List<AccountDto>
            {
                new()
                {
                    Id = "1",
                    Name = "John Doe",
                    Email = "john@example.com"
                },
                new()
                {
                    Id = "2",
                    Name = "Jane Smith",
                    Email = "jane@example.com"
                },
                new()
                {
                    Id = "3",
                    Name = "Bob Johnson",
                    Email = "bob@example.com"
                }
            };
            
            _mockAccountService.Setup(accountService => accountService.GetAllAccountsAsync()).ReturnsAsync(expectedAccountDtos);

            // Act
            var result = await _controller.GetAllAccounts() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(expectedAccountDtos, result.Value);
        }
    }
}
