using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AcademicFinals.API.DTOs;
using AcademicFinals.API.Models;

namespace AcademicFinals.API.Mappers
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<RegisterDto, ApplicationUser>()
                .ForMember(user => user.UserName, config => config.MapFrom(dto => dto.Email));

            CreateMap<ApplicationUser, UserDto>();
        }
    }
}