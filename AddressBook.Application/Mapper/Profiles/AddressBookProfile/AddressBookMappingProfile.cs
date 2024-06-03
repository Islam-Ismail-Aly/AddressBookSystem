using AddressBook.Application.DTOs.AddressBooks;
using AddressBook.Core.Entities;
using AutoMapper;

namespace AddressBook.Application.Mapper.Profiles.AddressBookProfile
{
    public class AddressBookMappingProfile : Profile
    {
        public AddressBookMappingProfile()
        {
            CreateMap<BaseEntity, int>().ConvertUsing(src => src.Id);

            CreateMap<EmployeeAddressBook, EmployeeAddressBookDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.MobileNumber, opt => opt.MapFrom(src => src.MobileNumber))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => src.Photo))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth))
                .ForMember(dest => dest.JobTitle, opt => opt.MapFrom(src => src.JobTitle.Name))
                .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Department.Name))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.Age))
                .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(src => src.CreatedOn));

            //// Job to JobDto mapping
            //CreateMap<Job, JobDto>()
            //    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            //    .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Name));

            //// Department to DepartmentDto mapping
            //CreateMap<Department, DepartmentDto>()
            //    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            //    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
        }
    }
}
