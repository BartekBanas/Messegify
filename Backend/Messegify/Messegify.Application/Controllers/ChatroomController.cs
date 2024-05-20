using Messegify.Application.Services;
using Messegify.Application.Services.ChatroomRequests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Messegify.Application.Controllers;

[ApiController]
[Route("api/chatroom")]
public class ChatroomController : Controller
{
    private readonly IChatroomRequestHandler _chatroomRequestHandler;

    public ChatroomController(IChatroomRequestHandler chatroomRequestHandler)
    {
        _chatroomRequestHandler = chatroomRequestHandler;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateChatroom(CancellationToken cancellationToken)
    {
        var request = new CreateChatroomRequest();

        await _chatroomRequestHandler.Handle(request, cancellationToken);

        return Ok();
    }
    
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetChatroom(CancellationToken cancellationToken)
    {
        var request = new GetUserChatroomsRequest();

        var requestResult = await _chatroomRequestHandler.Handle(request, cancellationToken);

        return Ok(requestResult);
    }
}