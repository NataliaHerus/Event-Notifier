using EventNotifier.Data.Entities;
using EventNotifier.Data.Entities.CategoryEntity;
using EventNotifier.Data.Entities.FormatEntity;
using EventNotifier.DTOs;
using AutoMapper;

namespace EventNotifier
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Event, EventDto>().ReverseMap();
            CreateMap<SelectedEvents, SelectedEventsDto>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Format, FormatDto>().ReverseMap();
        }
    }
}
