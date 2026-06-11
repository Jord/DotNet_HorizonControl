using HorizonControlCenterModels;
using HorizonControlCenterModels.Custom;
using HorizonControlCenterModels.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HorizonControlCenterDAL.Interfaces
{
    public interface IGroupDAL
    {
       // Task<List<GroupWithCategoryViewModel>> GetgrouprecordsAsync();
       // Task<GroupWithCategoryViewModel> GetgrouprecordsByIdAsync(Guid id);
        //Task<List<GroupWithCategoryViewModel>> GetGroupRecordsByGroupTypeAsync(string groupType);
       // Task<List<UserGroupSummaryViewModelDTO>> GetUserGroupRecordsByGroupNamesAsync(List<string> groupNames);
       // Task<List<GroupSummaryDTO>> GetGroupNamesByUserIdAsync(Guid userId);
        Task<List<GroupModel>> GetAllAsync();
        Task<GroupModel?> GetGroupByIdAsync(Guid id);
        Task<GroupModel?> GetExistingGroupAsync(GroupDTO dto);
        Task<(GroupModel result, string message)> CreategroupAsync(GroupDTO groupDto, int userId);
        Task<GroupModel> UpdategroupAsync(Guid id, GroupDTO groupDto, int userId);
        Task<ActionResponseModel> DeletegroupAsync(Guid id, int userId);
        Task<ActionResponseModel> PermanentDeletegroupAsync(Guid id);
    }
}