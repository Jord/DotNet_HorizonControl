using HorizonControlCenterModels;
using HorizonControlCenterModels.Custom;
using HorizonControlCenterModels.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HorizonControlCenterBAL.Interfaces
{
    public interface IUserBAL
    {
        Task<List<UserModel>> GetAllAsync();
        Task<UserModel?> GetByIdAsync(Guid id);
        Task<GlobalResponseModel<UserModel>> CreateAsync(UserDTO model, int userId);
        Task<GlobalResponseModel<UserModel>> UpdateAsync(Guid id, UserDTO model, int userId);
        Task<GlobalResponseModel<UserModel>> DeleteAsync(Guid id, int userId);
        Task<GlobalResponseModel<UserModel>> PermanentDeleteAsync(Guid id);
    }
}
