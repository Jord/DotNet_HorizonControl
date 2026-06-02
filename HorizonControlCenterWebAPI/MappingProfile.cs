using AutoMapper;
using HorizonControlCenterDAL.Entities;
using HorizonControlCenterModels;

namespace HorizonControlCenterWebAPI
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Suite, SuiteModel>().ReverseMap();
        }
    }
}
