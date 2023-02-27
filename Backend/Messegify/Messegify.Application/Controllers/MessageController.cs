using Messegify.Application.Dtos;
using Messegify.Application.Services;
using Messegify.Application.Services.ChatRoomRequests;
using Microsoft.AspNetCore.Mvc;

namespace Messegify.Application.Controllers;

[ApiController]
[Route("api/chatRoom/{chatRoomId:guid}/message")]
public class MessageController : Controller
{
    private readonly IMessageRequestHandler _messageRequestHandler;
    
    public MessageController(IMessageRequestHandler messageRequestHandler)
    {
        _messageRequestHandler = messageRequestHandler;
    }

    [HttpPost]
    public async Task<IActionResult> Send([FromRoute] Guid chatRoomId, SendMessageDto dto, CancellationToken ct)
    {
        var request = new SendMessageRequest(dto, chatRoomId);

        await _messageRequestHandler.Handle(request, ct);

        return Ok();
    }
    
    
    [HttpGet("list")]
    public async Task<IActionResult> Send([FromRoute] Guid chatRoomId, CancellationToken ct)
    {
        var request = new GetMessagesRequest(chatRoomId);

        var messagesDto = await _messageRequestHandler.Handle(request, ct);

        return Ok(messagesDto);
    }
}