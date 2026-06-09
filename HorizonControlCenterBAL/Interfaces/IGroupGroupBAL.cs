using HorizonControlCenterModels;
using HorizonControlCenterModels.Custom;
using HorizonControlCenterModels.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HorizonControlCenterBAL.Interfaces
{
    public interface IGroupGroupBAL
    {
        Task<List<GroupGroupModel>> GetAllAsync();
        Task<GroupGroupModel?> GetByIdAsync(Guid id);
        Task<GlobalResponseModel<GroupGroupModel>> CreateAsync(GroupGroupDTO model, int userId);
        Task<GlobalResponseModel<GroupGroupModel>> UpdateAsync(Guid id, GroupGroupDTO model, int userId);
        Task<GlobalResponseModel<GroupGroupModel>> DeleteAsync(Guid id);
        Task<GlobalResponseModel<GroupGroupModel>> PermanentDeleteAsync(Guid id);
    }
}
