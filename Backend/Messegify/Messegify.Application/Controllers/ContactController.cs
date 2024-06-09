using System.Security.Claims;
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
    public async Task<IActionResult> Friend([FromRoute] Guid targetAccountGuid, CancellationToken cancellationToken)
    {
        var senderGuid = Guid.Parse(User.Claims.First(claim => claim.Type == ClaimTypes.PrimarySid).Value);

        await _accountService.CreateContactAsync(senderGuid, targetAccountGuid, cancellationToken);

        return Ok();
    }
    
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetFriends()
    {
        var friendships = await _accountService.GetActiveContactsAsync();

        return Ok(friendships);
    }

    [Authorize]
    [HttpDelete("{targetContactGuid:guid}")]
    public async Task<IActionResult> DeleteContact([FromRoute] Guid targetContactGuid, CancellationToken cancellationToken)
    {
        await _accountService.DeleteContactAsync(targetContactGuid, cancellationToken);

        return Ok();
    }
}