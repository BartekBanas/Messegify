using Messegify.Application.Dtos;
using Messegify.Application.Services.ChatroomRequests;

namespace Messegify.Application.Services;

public interface IChatroomRequestHandler
{
    Task Handle(CreateChatroomRequest request, CancellationToken cancellationToken);
    Task Handle(DeleteChatroomRequest request, CancellationToken cancellationToken);
    Task<IEnumerable<ChatRoomDto>> Handle(GetUserChatroomsRequest request, CancellationToken cancellationToken);
}