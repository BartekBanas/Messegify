using MediatR;
using Messegify.Application.Dtos;
using Messegify.Application.Services;
using Messegify.Application.Services.ChatroomRequests;
using Messegify.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Messegify.Application.Controllers;

[ApiController]
[Route("api/chatroom")]
public class ChatroomController : Controller
{
    private readonly IMediator _mediator;
    private readonly IChatroomRequestHandler _chatroomRequestHandler;

    public ChatroomController(IMediator mediator, IChatroomRequestHandler chatroomRequestHandler)
    {
        _mediator = mediator;
        _chatroomRequestHandler = chatroomRequestHandler;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateChatroom(ChatroomCreateDto dto, CancellationToken cancellationToken)
    {
        var request = new CreateChatroomRequest(ChatRoomType.Regular, dto.Name);

        await _mediator.Send(request, cancellationToken);

        return Ok();
    }

    [Authorize]
    [HttpPost("invite")]
    public async Task<IActionResult> InviteToChatroom([FromBody] ChatroomInvite dto, CancellationToken cancellationToken)
    {
        var request = new InviteToChatroomRequest(dto);

        await _mediator.Send(request, cancellationToken);

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