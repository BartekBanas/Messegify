using Messegify.Application.Services;
using Messegify.Application.Services.ChatRoomRequests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Messegify.Application.Controllers;

[ApiController]
[Route("api/chatroom")]
public class ChatRoomController : Controller
{
    private readonly IChatRoomRequestHandler _chatRoomRequestHandler;

    public ChatRoomController(IChatRoomRequestHandler chatRoomRequestHandler)
    {
        _chatRoomRequestHandler = chatRoomRequestHandler;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateChatRoom(CancellationToken cancellationToken)
    {
        var request = new CreateChatRoomRequest();

        await _chatRoomRequestHandler.Handle(request, cancellationToken);

        return Ok();
    }
    
    [Authorize]
    [HttpGet("list")]
    public async Task<IActionResult> GetChatRoom(CancellationToken cancellationToken)
    {
        var request = new GetUserChatRoomsRequest();

        var requestResult = await _chatRoomRequestHandler.Handle(request, cancellationToken);

        return Ok(requestResult);
    }
}