using AutoMapper;
using HorizonControlCenterDAL.Entities;
using HorizonControlCenterModels;
using HorizonControlCenterModels.DTO;

public class GroupRoleProfile : Profile
{
    public GroupRoleProfile()
    {
        CreateMap<GroupRoleDTO, GroupRole>();
        CreateMap<GroupRole, GroupRoleModel>().ReverseMap();
    }
}
