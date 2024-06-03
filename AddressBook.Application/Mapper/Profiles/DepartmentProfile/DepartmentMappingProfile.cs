using AddressBook.Application.DTOs.Departments;
using AddressBook.Core.Entities;
using AutoMapper;

namespace AddressBook.Application.Mapper.Profiles.DepartmentProfile
{
    public class DepartmentMappingProfile : Profile
    {
        public DepartmentMappingProfile()
        {
            CreateMap<Department, DepartmentDto>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
        }
    }
}
