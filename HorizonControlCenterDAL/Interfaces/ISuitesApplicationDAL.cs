using HorizonControlCenterDAL.Entities;
using HorizonControlCenterModels;
using HorizonControlCenterModels.DTO;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HorizonControlCenterDAL.Interfaces
{
    public interface ISuitesApplicationDAL
    {
        Task<List<SuitesApplicationModel>> GetAllAsync();
        Task<SuitesApplicationModel?> GetByIdAsync(Guid id);
        Task<SuitesApplicationModel?> GetExistingApplicationAsync(SuitesApplicationDTO model);
        Task<(SuitesApplicationModel result, string message)> CreateAsync(SuitesApplicationDTO model, int userId);
        Task<SuitesApplicationModel> UpdateAsync(Guid id, SuitesApplicationDTO model, int userId);
        Task<SuitesApplicationModel> PatchAsync(Guid id, JsonPatchDocument<SuiteApplication> patchDoc, int userId);
        Task<int> DeleteAsync(Guid id, int userId);
        Task<int> PermanentDeleteAsync(Guid id);
        Task<List<string>> GetDistinctApplicationTypesAsync();
    }
}