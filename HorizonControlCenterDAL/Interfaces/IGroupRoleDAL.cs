using HorizonControlCenterModels;
using HorizonControlCenterModels.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HorizonControlCenterDAL.Interfaces
{
    public interface IGroupRoleDAL
    {
        Task<List<GroupRoleModel>> GetAllAsync();
        Task<GroupRoleModel?> GetByIdAsync(Guid id);
        Task<GroupRoleModel?> GetExistingGroupRoleAsync(GroupRoleDTO model);
        Task<(GroupRoleModel result, string message)> CreateAsync(GroupRoleDTO model, int userId);
        Task<GroupRoleModel> UpdateAsync(Guid id, GroupRoleDTO model, int userId);
        Task<int> DeleteAsync(Guid id, int userId);
        Task<int> PermanentDeleteAsync(Guid id);
    }
}
