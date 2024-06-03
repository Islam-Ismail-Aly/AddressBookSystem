using AddressBook.Application.Mapper.Profiles.AddressBookProfile;
using AddressBook.Application.Mapper.Profiles.DepartmentProfile;
using AddressBook.Application.Mapper.Profiles.JobProfile;
using Microsoft.Extensions.DependencyInjection;

namespace AddressBook.Application.Mapper.Configuration
{
    public static class AutoMapperConfiguration
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapperConfiguration),
                                   typeof(AddressBookMappingProfile),
                                   typeof(DepartmentMappingProfile),
                                   typeof(JobMappingProfile));
        }
    }
}
