using HorizonControlCenterDAL.Entities;
using HorizonControlCenterModels;
using HorizonControlCenterModels.Custom;
using HorizonControlCenterModels.DTO;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HorizonControlCenterBAL.Interfaces
{
    public interface ISuitesApplicationBAL
    {
        Task<List<SuitesApplicationModel>> GetAllAsync();
        Task<SuitesApplicationModel?> GetByIdAsync(Guid id);
        Task<GlobalResponseModel<SuitesApplicationModel>> CreateAsync(SuitesApplicationDTO model, int userId);
        Task<SuitesApplicationModel> PatchAsync(Guid id, JsonPatchDocument<SuiteApplication> patchDoc, int userId);
        Task<GlobalResponseModel<SuitesApplicationModel>> UpdateAsync(Guid id, SuitesApplicationDTO model, int userId);
        Task<GlobalResponseModel<SuitesApplicationModel>> DeleteAsync(Guid id, int userId);
        Task<GlobalResponseModel<SuitesApplicationModel>> PermanentDeleteAsync(Guid id);
        Task<List<string>> GetDistinctApplicationTypesAsync();
    }
}