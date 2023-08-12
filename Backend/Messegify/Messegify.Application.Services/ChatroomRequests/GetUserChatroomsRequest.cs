using MediatR;
using Messegify.Application.Dtos;

namespace Messegify.Application.Services.ChatroomRequests;

public class GetUserChatroomsRequest : IRequest<IEnumerable<ChatRoomDto>>
{
    // Intentionally empty
}