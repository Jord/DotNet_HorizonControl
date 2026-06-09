using HorizonControlCenterModels;
using HorizonControlCenterModels.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HorizonControlCenterDAL.Interfaces
{
    public interface IGroupGroupDAL
    {
        Task<List<GroupGroupModel>> GetAllAsync();
        Task<GroupGroupModel?> GetByIdAsync(Guid id);
        Task<GroupGroupModel?> GetExistingGroupGroupAsync(GroupGroupDTO model);
        Task<(GroupGroupModel result, string message)> CreateAsync(GroupGroupDTO model, int userId);
        Task<GroupGroupModel> UpdateAsync(Guid id, GroupGroupDTO model, int userId);
        Task<int> DeleteAsync(Guid id);
        Task<int> PermanentDeleteAsync(Guid id);
    }
}
