using HorizonControlCenterModels;
using HorizonControlCenterModels.Custom;
using HorizonControlCenterModels.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HorizonControlCenterBAL.Interfaces
{
    public interface IGroupBAL
    {
        //Task<List<GroupWithCategoryViewModel>> GetgrouprecordsAsync();
        //Task<GroupWithCategoryViewModel> GetgrouprecordsByIdAsync(Guid id);
        //Task<List<GroupWithCategoryViewModel>> GetGroupRecordsByGroupTypeAsync(string groupType);
        //Task<List<UserGroupSummaryViewModelDTO>> GetUserGroupRecordsByGroupNamesAsync(List<string> groupNames);
        //Task<List<GroupSummaryDTO>> GetGroupNamesByUserIdAsync(Guid userId);
        Task<List<GroupModel>> GetAllAsync();
        Task<GroupModel?> GetByIdAsync(Guid id);
        Task<GlobalResponseModel<GroupModel>> CreategroupAsync(GroupDTO groupDto, int userId);
        Task<GlobalResponseModel<GroupModel>> UpdategroupAsync(Guid id, GroupDTO groupDto, int userId);
        Task<GlobalResponseModel<GroupModel>> DeletegroupAsync(Guid id, int userId);
        Task<GlobalResponseModel<GroupModel>> PermanentDeletegroupAsync(Guid id);
    }
}