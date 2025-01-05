using AutoMapper;
using World.API.DTOs.Country;
using World.API.DTOs.States;
using World.API.Models;

namespace World.API.Common
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Country, CreateCountryDTO>().ReverseMap();
            CreateMap<Country, CountryDTO>().ReverseMap();
            CreateMap<Country, UpdateCountryDTO>().ReverseMap();

            CreateMap<State, CreateStateDTO>().ReverseMap();
            CreateMap<State, StateDTO>().ReverseMap();
            CreateMap<State, UpdateStateDTO>().ReverseMap();
        }
    }
}
