using Messegify.Domain.Entities;

namespace Messegify.Application.Dtos;

public class ChatRoomDto
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required ChatRoomType ChatRoomType { get; init; }
    public required IEnumerable<Guid> Members { get; init; }
}