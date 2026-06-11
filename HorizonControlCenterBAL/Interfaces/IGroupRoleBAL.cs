using HorizonControlCenterModels;
using HorizonControlCenterModels.Custom;
using HorizonControlCenterModels.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HorizonControlCenterBAL.Interfaces
{
    public interface IGroupRoleBAL
    {
        Task<List<GroupRoleModel>> GetAllAsync();
        Task<GroupRoleModel?> GetByIdAsync(Guid id);
        Task<GlobalResponseModel<GroupRoleModel>> CreateAsync(GroupRoleDTO model, int userId);
        Task<GlobalResponseModel<GroupRoleModel>> UpdateAsync(Guid id, GroupRoleDTO model, int userId);
        Task<GlobalResponseModel<GroupRoleModel>> DeleteAsync(Guid id, int userId);
        Task<GlobalResponseModel<GroupRoleModel>> PermanentDeleteAsync(Guid id);
    }
}
