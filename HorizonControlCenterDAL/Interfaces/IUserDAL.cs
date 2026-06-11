using HorizonControlCenterModels;
using HorizonControlCenterModels.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HorizonControlCenterDAL.Interfaces
{
    public interface IUserDAL
    {
        Task<List<UserModel>> GetAllAsync();
        Task<UserModel?> GetByIdAsync(Guid id);
        Task<UserModel?> GetExistingUserAsync(UserDTO model);
        Task<(UserModel result, string message)> CreateAsync(UserDTO model, int userId);
        Task<UserModel> UpdateAsync(Guid id, UserDTO model, int userId);
        Task<int> DeleteAsync(Guid id, int userId);
        Task<int> PermanentDeleteAsync(Guid id);
    }
}
