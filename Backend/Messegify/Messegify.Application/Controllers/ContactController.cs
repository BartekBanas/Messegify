using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Messegify.Application.Service.Extensions;
using Messegify.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Messegify.Application.Controllers;

[ApiController]
[Route("api/contact")]
public class ContactController : Controller
{
    private readonly IAccountService _accountService;

    public ContactController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [Authorize]
    [HttpPost("{targetAccountGuid:guid}")]
    public async Task<IActionResult> Friend([FromRoute] Guid targetAccountGuid)
    {
        var senderGuid = Guid.Parse(User.Claims.First(x => x.Type == ClaimTypes.PrimarySid).Value);

        await _accountService.ContactAsync(senderGuid, targetAccountGuid);

        return Ok();
    }
    
    [Authorize]
    [HttpGet("list")]
    public async Task<IActionResult> GetFriends()
    {
        var accountGuid = User.GetId();

        var friendships = await _accountService.GetContactsAsync(accountGuid);

        return Ok(friendships);
    }
}