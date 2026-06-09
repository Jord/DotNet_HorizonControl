using HorizonControlCenterModels;
using HorizonControlCenterModels.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HorizonControlCenterDAL.Interfaces
{
    public interface IUserGroupDAL
    {
        Task<List<UserGroupModel>> GetAllAsync();
        Task<UserGroupModel?> GetByIdAsync(Guid id);
        Task<UserGroupModel?> GetExistingUserGroupAsync(UserGroupDTO model);
        Task<(UserGroupModel result, string message)> CreateAsync(UserGroupDTO model, int userId);
        Task<UserGroupModel> UpdateAsync(Guid id, UserGroupDTO model, int userId);
        Task<int> DeleteAsync(Guid id, int userId);
        Task<int> PermanentDeleteAsync(Guid id);
    }
}
