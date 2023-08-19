using System.Security.Claims;
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
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly AccountController _controller;

        public AccountControllerTests()
        {
            _mockAccountService = new Mock<IAccountService>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _controller = new AccountController(_mockAccountService.Object, _httpContextAccessorMock.Object);
        }

        [Fact]
        public async Task RegisterAccount_ValidDto_ReturnsOk()
        {
            // Arrange
            var validDto = new RegisterAccountDto
            {
                Name = "Username",
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
        
        [Fact]
        public async Task Me_ReturnsOkResultWithClaims()
        {
            // Arrange
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "user_id"),
                new Claim(ClaimTypes.Email, "test@example.com")
                // Add more claims as needed
            };
            
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims));
            _controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = claimsPrincipal } };

            // Act
            var result = await _controller.Me();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var claimsInfo = Assert.IsAssignableFrom<IDictionary<string, string>>(okResult.Value);
        }
        
        [Fact]
        public async Task GetAccount_ReturnsOkResultWithAccountDto()
        {
            // Arrange
            var accountGuid = new Guid("05c2a638-8d33-4a7c-2a2a-08db96b815eb");
            var accountDto = new AccountDto()
            {
                Id = "05c2a638-8d33-4a7c-2a2a-08db96b815eb",
                Name = "Michael",
                Email = "michael@mail.com"
            };
            
            _mockAccountService.Setup(svc => svc.GetAccountAsync(accountGuid)).ReturnsAsync(accountDto);

            // Act
            var result = await _controller.GetAccount(accountGuid);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<AccountDto>(okResult.Value);
            Assert.Equal(accountDto, model);
        }
    }
}