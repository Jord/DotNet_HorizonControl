using HorizonControlCenterDAL.Entities;
using HorizonControlCenterModels;
using HorizonControlCenterModels.DTO;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HorizonControlCenterDAL.Interfaces
{
    public interface ISuiteDAL
    {
        Task<List<SuiteModel>> GetAllAsync();
        Task<SuiteModel?> GetByIdAsync(Guid id);
        Task<SuiteModel?> GetExistingSuiteAsync(SuitesDTO model);
        Task<(SuiteModel result, string message)> CreateAsync(SuitesDTO model, int userId);
        Task<SuiteModel> PatchAsync(Guid id, JsonPatchDocument<Suite> patchDoc, int userId);
        Task<SuiteModel> UpdateAsync(Guid id, SuitesDTO model, int userId);
        Task<int> DeleteAsync(Guid id, int userId);
        Task<int> PermanentDeleteAsync(Guid id);
    }
}