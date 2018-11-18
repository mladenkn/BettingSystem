using AutoMapper;
using BetingSystem.Models;

namespace BetingSystem.DAL
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Ticket, TicketDto>()
                .ForMember(p => p.Pairs, c => c.MapFrom(p => p.BetedPairs));

            CreateMap<BetedPair, TicketDto.Pair>()
                .ForMember(p => p.Team1, c => c.MapFrom(p => p.BetablePair.Team1.Name))
                .ForMember(p => p.Team2, c => c.MapFrom(t => t.BetablePair.Team2.Name));
        }
    }
}
