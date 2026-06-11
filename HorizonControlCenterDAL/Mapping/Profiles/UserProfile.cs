using AutoMapper;
using HorizonControlCenterDAL.Entities;
using HorizonControlCenterModels;
using HorizonControlCenterModels.DTO;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserDTO, User>();
        CreateMap<User, UserModel>().ReverseMap();
    }
}
