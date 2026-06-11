using AutoMapper;
using HorizonControlCenterDAL.Entities;
using HorizonControlCenterModels;
using HorizonControlCenterModels.DTO;

public class UserGroupProfile : Profile
{
    public UserGroupProfile()
    {
        CreateMap<UserGroupDTO, UserGroup>();
        CreateMap<UserGroup, UserGroupModel>().ReverseMap();
    }
}
