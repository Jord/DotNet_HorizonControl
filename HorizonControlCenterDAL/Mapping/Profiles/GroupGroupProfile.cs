using AutoMapper;
using HorizonControlCenterDAL.Entities;
using HorizonControlCenterModels;
using HorizonControlCenterModels.DTO;

public class GroupGroupProfile : Profile
{
    public GroupGroupProfile()
    {
        CreateMap<GroupGroupDTO, GroupGroup>();
        CreateMap<GroupGroup, GroupGroupModel>().ReverseMap();
    }
}
