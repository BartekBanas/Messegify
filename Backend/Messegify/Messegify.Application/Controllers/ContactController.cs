using System.Security.Claims;
using Messegify.Application.Service.Extensions;
using Messegify.Application.Services;
using Microsoft.AspNetCore.Authorization;
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
        var senderGuid = Guid.Parse(User.Claims.First(claim => claim.Type == ClaimTypes.PrimarySid).Value);

        await _accountService.ContactAsync(senderGuid, targetAccountGuid);

        return Ok();
    }
    
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetFriends()
    {
        var accountGuid = User.GetId();

        var friendships = await _accountService.GetContactsAsync(accountGuid);

        return Ok(friendships);
    }
}