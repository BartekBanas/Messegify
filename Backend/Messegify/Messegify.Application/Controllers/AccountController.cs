using Messegify.Application.Dtos;
using Messegify.Application.Service.Extensions;
using Messegify.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Messegify.Application.Controllers;

[ApiController]
[Route("api/account")]
public class AccountController : Controller
{
    private readonly IAccountService _accountService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AccountController(IAccountService accountService, IHttpContextAccessor httpContextAccessor)
    {
        _accountService = accountService;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpPost]
    public async Task<IActionResult> RegisterAccount([FromBody] RegisterAccountDto dto)
    {
        await _accountService.RegisterAccountAsync(dto);

        return Ok();
    }
    
    [HttpPost("authenticate")]
    public async Task<IActionResult> Authenticate([FromBody] LoginDto dto)
    {
        var token = await _accountService.AuthenticateAsync(dto);

        return Ok(token);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllAccounts()
    {
        var accountDtos = await _accountService.GetAllAccountsAsync();
        
        return Ok(accountDtos);
    }
    
    [Authorize]
    [HttpGet("me")]
    public Task<IActionResult> Me()
    {
        var claims = User.Claims;

        var claimsInfo = claims.ToDictionary(claim => claim.Type, claim => claim.Value);
        
        return Task.FromResult<IActionResult>(Ok(claimsInfo));
    }

    [Authorize]
    [HttpGet("{accountGuid:guid}")]
    public async Task<IActionResult> GetAccount([FromRoute] Guid accountGuid)
    {
        var user = await _accountService.GetAccountAsync(accountGuid);
        
        return Ok(user);
    }

    [Authorize]
    [HttpDelete("me")]
    public async Task<IActionResult> DeleteMyAccount()
    {
        if (_httpContextAccessor.HttpContext != null)
        {
            var userId = _httpContextAccessor.HttpContext.User.GetId();

            await _accountService.DeleteAccountAsync(userId);

            return Ok();
        }
        else
        {
            return Unauthorized();
        }
    }

    [Authorize]
    [HttpPut("me")]
    public async Task<IActionResult> UpdateMyAccount([FromBody] UpdateAccountDto dto)
    {
        if (_httpContextAccessor.HttpContext != null)
        {
            var userId = _httpContextAccessor.HttpContext.User.GetId();

            var updatedAccount = await _accountService.UpdateAccountAsync(userId, dto);

            return Ok(updatedAccount);
        }
        else
        {
            return Unauthorized();
        }
    }
}