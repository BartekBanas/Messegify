using MediatR;
using Messegify.Application.Services.ChatroomRequests;
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
        var request = new CreateChatroomRequest();

        await _mediator.Send(request, cancellationToken);

        return Ok();
    }
    
    [Authorize]
    [HttpGet("list")]
    public async Task<IActionResult> GetChatRoom(CancellationToken cancellationToken)
    {
        var request = new GetUserChatroomsRequest();

        var requestResult = await _mediator.Send(request, cancellationToken);

        return Ok(requestResult);
    }
}