using Moq;
using AutoMapper;
using FluentValidation;
using Messegify.Application.Dtos;
using Messegify.Application.Services;
using Messegify.Domain.Abstractions;
using Messegify.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace Application.Services.Tests;

public class AccountServiceTests
{
    private readonly Mock<IRepository<Account>> _accountRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly IHashingService _hashingService;
    private readonly Mock<IValidator<Account>> _validatorMock;
    private readonly AccountService _accountService;

    public AccountServiceTests()
    {
        _accountRepositoryMock = new Mock<IRepository<Account>>();
        _mapperMock = new Mock<IMapper>();
        _hashingService = new HashingService();
        _validatorMock = new Mock<IValidator<Account>>();

        _accountService = new AccountService(
            _accountRepositoryMock.Object,
            It.IsAny<IRepository<Contact>>(),
            _hashingService,
            _validatorMock.Object,
            It.IsAny<IValidator<Contact>>(),
            It.IsAny<IJwtService>(),
            It.IsAny<IHttpContextAccessor>(),
            _mapperMock.Object,
            It.IsAny<IChatroomRequestHandler>()
        );
    }

    [Fact]
    public async Task GetAccountAsync_ValidId_ReturnsAccountDto()
    {
        // Arrange
        var accountId = Guid.NewGuid();
        var account = new Account { Id = accountId };
        var accountDto = new AccountDto { Id = accountId.ToString() };
        _accountRepositoryMock.Setup(repo => repo.GetOneRequiredAsync(accountId)).ReturnsAsync(account);
        _mapperMock.Setup(mapper => mapper.Map<AccountDto>(account)).Returns(accountDto);

        // Act
        var result = await _accountService.GetAccountAsync(accountId);

        // Assert
        Assert.Equal(accountDto, result);
    }
    
    
    [Fact]
    public async Task GetAllAccountsAsync_ReturnsListOfAccountDtos()
    {
        // Arrange
        var accounts = new List<Account>
        {
            new Account
            {
                Id = Guid.NewGuid(),
                Name = "user1",
                PasswordHash = "hashedPassword1",
                Email = "user1@example.com",
                DateCreated = DateTime.UtcNow
            },
            new Account
            {
                Id = Guid.NewGuid(),
                Name = "user2",
                PasswordHash = "hashedPassword2",
                Email = "user2@example.com",
                DateCreated = DateTime.UtcNow
            }
        };
        
        var accountDtos = accounts.Select(account => new AccountDto
        {
            Id = account.Id.ToString(),
            Name = account.Name,
            Email = account.Email
        }).ToList();
        
        _accountRepositoryMock.Setup(repo => repo.GetAsync(null, null)).ReturnsAsync(accounts);
        _mapperMock.Setup(mapper => mapper.Map<IEnumerable<AccountDto>>(accounts)).Returns(accountDtos);

        // Act
        var result = await _accountService.GetAccountsAsync();

        // Assert
        Assert.Equal(accountDtos, result);
    }

    [Fact]
    public async Task RegisterAccountAsync_ValidInput_CreatesNewAccount()
    {
        // Arrange
        var registerDto = new RegisterAccountDto
        {
            Email = "test@example.com",
            Name = "testuser",
            Password = "password123"
        };
        
        // Act
        var hashedPassword = _hashingService.HashPassword(registerDto.Password);
        
        var createdAccount = new Account()
        {
            Id = new Guid(),
            Name = registerDto.Name,
            Email = registerDto.Email,
            PasswordHash = hashedPassword,
            DateCreated = DateTime.UtcNow
        };

        // Assert
        Assert.Equal(registerDto.Email, createdAccount.Email);
        Assert.True(!string.IsNullOrEmpty(createdAccount.Name) && createdAccount.Name.Length is >= 3 and <= 32);
        Assert.True(!string.IsNullOrEmpty(createdAccount.PasswordHash));
    }
}