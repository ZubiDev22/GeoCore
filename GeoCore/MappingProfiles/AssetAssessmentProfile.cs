using AutoMapper;
using GeoCore.Entities;
using GeoCore.DTOs;

namespace GeoCore.MappingProfiles
{
    public class ManagementBudgetProfile : Profile
    {
        public ManagementBudgetProfile()
        {
            CreateMap<ManagementBudget, ManagementBudgetDto>().ReverseMap();
        }
    }
}
