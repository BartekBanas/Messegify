using Messegify.Application.Dtos;
using Messegify.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Messegify.Application.Controllers;

[ApiController]
[Route("account")]
public class AccountController : Controller
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    public async Task<IActionResult> RegisterAccount([FromBody] RegisterAccountDto registerDto)
    {
        await _accountService.RegisterAccount(registerDto);

        return Ok();    //  Could be entirely long, just don't want the error for now
    }

}