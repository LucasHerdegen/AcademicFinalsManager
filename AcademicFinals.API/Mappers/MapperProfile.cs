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

            CreateMap<Subject, SubjectDto>();
            CreateMap<SubjectPostDto, Subject>();
            CreateMap<SubjectPutDto, Subject>();

            CreateMap<ExamDate, ExamDateDto>()
                .ForMember(x => x.EnrollmentsCount,
                    config => config.MapFrom(y => y.Enrollments == null ? 0 : y.Enrollments.Count));
            CreateMap<ExamDatePostDto, ExamDate>();
            CreateMap<ExamDatePutDto, ExamDate>();

            CreateMap<Enrollment, EnrollmentDto>();
        }
    }
}