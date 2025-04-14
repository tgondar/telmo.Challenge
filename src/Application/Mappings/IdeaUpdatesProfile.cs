using Application.Dto;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{
    public class IdeaUpdatesProfile : Profile
    {
        public IdeaUpdatesProfile()
        {
            CreateMap<Domain.Entities.IdeaUpdates, IdeaUpdatesDto>();
            CreateMap<Idea, IdeaDto>();
            CreateMap<Status, StatusDto>();
            CreateMap<User, UserDto>();
            CreateMap<PagedResult<Domain.Entities.IdeaUpdates>, PagedResult<IdeaUpdatesDto>>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
                .ForMember(dest => dest.PageNumber, opt => opt.MapFrom(src => src.PageNumber))
                .ForMember(dest => dest.RowCount, opt => opt.MapFrom(src => src.RowCount));
        }
    }
}
