using AutoMapper;
using GeoCore.Entities;
using GeoCore.DTOs;

namespace GeoCore.MappingProfiles
{
    public class AssetAssessmentProfile : Profile
    {
        public AssetAssessmentProfile()
        {
            CreateMap<AssetAssessment, AssetAssessmentDto>().ReverseMap();
        }
    }
}
