using MediatR;
using Messegify.Application.Services;
using Messegify.Application.Services.ChatroomRequests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Messegify.Application.Controllers;

[ApiController]
[Route("api/chatroom")]
public class ChatRoomController : Controller
{
    private readonly IMediator _mediator;
    private readonly IChatroomRequestHandler _chatroomRequestHandler;

    public ChatRoomController(IMediator mediator, IChatroomRequestHandler chatroomRequestHandler)
    {
        _mediator = mediator;
        _chatroomRequestHandler = chatroomRequestHandler;
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

        var requestResult = await _chatroomRequestHandler.Handle(request, cancellationToken);

        return Ok(requestResult);
    }
}