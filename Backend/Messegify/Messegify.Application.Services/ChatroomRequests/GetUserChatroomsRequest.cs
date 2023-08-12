using MediatR;
using Messegify.Application.Dtos;
using Messegify.Domain.Entities;

namespace Messegify.Application.Services.ChatroomRequests;

public class GetUserChatroomsRequest : IRequest<IEnumerable<ChatRoomDto>>
{
    // Intentionally empty
}