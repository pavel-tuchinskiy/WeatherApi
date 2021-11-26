using AutoMapper;
using Contracts.CityDTOs;
using Contracts.TemperatureDTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Extensions
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<City, CityDto>().ReverseMap();
            CreateMap<CityForCreationDto, City>().ReverseMap();
            CreateMap<CityForUpdateDto, City>().ReverseMap();

            CreateMap<Temperature, TemperatureDto>().ReverseMap();
            CreateMap<TemperatureForCreationDto, Temperature>().ReverseMap();
            CreateMap<TemperatureForUpdateDto, Temperature>().ReverseMap();
        }
    }
}
