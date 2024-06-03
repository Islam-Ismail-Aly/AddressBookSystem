using AddressBook.Application.DTOs.Jobs;
using AddressBook.Core.Entities;
using AutoMapper;

namespace AddressBook.Application.Mapper.Profiles.JobProfile
{
    public class JobMappingProfile : Profile
    {
        public JobMappingProfile()
        {
            CreateMap<Job, JobDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Name));
        }
    }
}
