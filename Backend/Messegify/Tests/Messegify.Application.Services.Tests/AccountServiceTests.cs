using Moq;
using AutoMapper;
using FluentValidation;
using Messegify.Application.Dtos;
using Messegify.Domain.Abstractions;
using Messegify.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Messegify.Application.Services.Tests;

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
}