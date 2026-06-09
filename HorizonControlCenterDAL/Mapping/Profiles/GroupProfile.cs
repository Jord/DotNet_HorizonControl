using HorizonControlCenterModels.DTO;
using HorizonControlCenterModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using HorizonControlCenterDAL.Entities;

namespace HorizonControlCenterDAL.Mapping.Profiles
{
    public class GroupProfile : Profile
    {
        public GroupProfile()
        {
            CreateMap<GroupDTO, Group>().ReverseMap();
            CreateMap<Group, GroupModel>().ReverseMap();

            //CreateMap<GroupWithCategoryView, GroupWithCategoryViewModel>().ReverseMap();
            //CreateMap<UserGroupView, UserGroupSummaryViewModelDTO>().ReverseMap();
        }
    }
}
