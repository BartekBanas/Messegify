using AutoMapper;
using Messegify.Application.Dtos;
using Messegify.Domain.Entities;

namespace Messegify.Application;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Message, MessageDto>();

        CreateMap<Contact, ContactDto>();

        CreateMap<Account, AccountDto>();
    }
}