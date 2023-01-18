﻿using System.Security.Claims;
using Messegify.Application.Dtos;
using Messegify.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Messegify.Application.Controllers;

[ApiController]
[Route("api/account")]
public class AccountController : Controller
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAccount([FromBody] RegisterAccountDto dto)
    {
        await _accountService.RegisterAccountAsync(dto);

        return Ok();    //  Could be entirely long, just don't want the error for now
    }
    
    [HttpPost("authenticate")]
    public async Task<IActionResult> Authenticate([FromBody] LoginDto dto)
    {
        var token = await _accountService.AuthenticateAsync(dto);

        return Ok(token);
    }
    
    [Authorize]
    [HttpGet("me")]
    public Task<IActionResult> Me()
    {
        var claims = User.Claims;

        var claimsInfo = claims.ToDictionary(claim => claim.Type, claim => claim.Value);
        
        return Task.FromResult<IActionResult>(Ok(claimsInfo));
    }
}