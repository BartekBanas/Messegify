using Messegify.Application.DomainEventHandlers.ContactCreated;
using Messegify.Application.Services;
using Messegify.Application.Services.ChatRoomRequests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Messegify.Application.Controllers;

[ApiController]
[Route("chatroom")]
public class ChatRoomController : Controller
{
    private IChatRoomRequestHandler _chatRoomRequestHandler;

    public ChatRoomController(IChatRoomRequestHandler chatRoomRequestHandler)
    {
        _chatRoomRequestHandler = chatRoomRequestHandler;
    }

    [Authorize]
    [HttpPost("create")]
    public async Task<IActionResult> CreateChatRoom(CancellationToken cancellationToken)
    {
        var request = new CreateChatRoomRequest();

        await _chatRoomRequestHandler.Handle(request, cancellationToken);

        return Ok();
    }
}