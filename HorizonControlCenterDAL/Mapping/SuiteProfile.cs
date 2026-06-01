using AutoMapper;
using HorizonControlCenterDAL.Entities;
using HorizonControlCenterModels;
using HorizonControlCenterModels.DTO;

public class SuiteProfile : Profile
{
    public SuiteProfile()
    {
        CreateMap<SuitesDTO, Suite>();
        CreateMap<Suite, SuiteModel>().ReverseMap();
    }
}