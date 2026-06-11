using HorizonControlCenterModels;
using HorizonControlCenterModels.Custom;
using HorizonControlCenterModels.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HorizonControlCenterBAL.Interfaces
{
    public interface IUserGroupBAL
    {
        Task<List<UserGroupModel>> GetAllAsync();
        Task<UserGroupModel?> GetByIdAsync(Guid id);
        Task<GlobalResponseModel<UserGroupModel>> CreateAsync(UserGroupDTO model, int userId);
        Task<GlobalResponseModel<UserGroupModel>> UpdateAsync(Guid id, UserGroupDTO model, int userId);
        Task<GlobalResponseModel<UserGroupModel>> DeleteAsync(Guid id, int userId);
        Task<GlobalResponseModel<UserGroupModel>> PermanentDeleteAsync(Guid id);
    }
}
