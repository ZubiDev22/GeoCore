using AutoMapper;
using GeoCore.Entities;
using GeoCore.DTOs;

namespace GeoCore.MappingProfiles
{
    public class MaintenanceEventProfile : Profile
    {
        public MaintenanceEventProfile()
        {
            CreateMap<MaintenanceEvent, MaintenanceEventDto>().ReverseMap();
        }
    }
}
