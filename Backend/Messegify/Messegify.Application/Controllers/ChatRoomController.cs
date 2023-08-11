using MediatR;
using Messegify.Application.Services.ChatRoomRequests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Messegify.Application.Controllers;

[ApiController]
[Route("api/chatroom")]
public class ChatRoomController : Controller
{
    private readonly IMediator _mediator;

    public ChatRoomController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateChatRoom(CancellationToken cancellationToken)
    {
        var request = new CreateChatRoomRequest();

        await _mediator.Send(request, cancellationToken);

        return Ok();
    }
    
    [Authorize]
    [HttpGet("list")]
    public async Task<IActionResult> GetChatRoom(CancellationToken cancellationToken)
    {
        var request = new GetUserChatRoomsRequest();

        var requestResult = await _mediator.Send(request, cancellationToken);

        return Ok(requestResult);
    }
}