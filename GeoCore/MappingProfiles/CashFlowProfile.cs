using AutoMapper;
using GeoCore.Entities;
using GeoCore.DTOs;

namespace GeoCore.MappingProfiles
{
    public class CashFlowProfile : Profile
    {
        public CashFlowProfile()
        {
            CreateMap<CashFlow, CashFlowDto>().ReverseMap();
        }
    }
}
