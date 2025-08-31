using AutoMapper;
using GeoCore.Entities;
using GeoCore.DTOs;

namespace GeoCore.MappingProfiles
{
    public class OtherProfiles : Profile
    {
        public OtherProfiles()
        {
            CreateMap<CashFlow, CashFlowDto>().ReverseMap();
            CreateMap<MaintenanceEvent, MaintenanceEventDto>().ReverseMap();
            CreateMap<AssetAssessment, AssetAssessmentDto>().ReverseMap();
        }
    }
}
