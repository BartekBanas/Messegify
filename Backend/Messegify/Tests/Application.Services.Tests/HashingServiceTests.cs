using Messegify.Application.Services;
using Messegify.Domain.Entities;
using Xunit;

namespace Application.Services.Tests;

public class HashingServiceTests
{
    [Fact]
    public void HashPassword_ReturnsValidHash()
    {
        // Arrange
        var password = "password123";
        var hashingService = new HashingService();

        // Act
        var hashedPassword = hashingService.HashPassword(password);

        // Assert
        Assert.NotNull(hashedPassword);
        Assert.NotEqual(password, hashedPassword);
    }

    [Fact]
    public void VerifyPassword_ValidPassword_ReturnsTrue()
    {
        // Arrange
        var account = new Account
        {
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123")
        };
        var hashingService = new HashingService();

        // Act
        var result = hashingService.VerifyPassword(account, "password123");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void VerifyPassword_InvalidPassword_ReturnsFalse()
    {
        // Arrange
        var account = new Account
        {
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123")
        };
        var hashingService = new HashingService();

        // Act
        var result = hashingService.VerifyPassword(account, "wrongPassword");

        // Assert
        Assert.False(result);
    }
}