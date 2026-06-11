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
    public interface ISuiteBAL
    {
        Task<List<SuiteModel>> GetAllAsync();
        Task<SuiteModel?> GetByIdAsync(Guid id);
        Task<GlobalResponseModel<SuiteModel>> CreateAsync(SuitesDTO model, int userId);
        Task<SuiteModel> PatchAsync(Guid id, JsonPatchDocument<Suite> patchDoc, int userId);
        Task<GlobalResponseModel<SuiteModel>> UpdateAsync(Guid id, SuitesDTO model, int userId);
        Task<GlobalResponseModel<SuiteModel>> DeleteAsync(Guid id, int userId);
        Task<GlobalResponseModel<SuiteModel>> PermanentDeleteAsync(Guid id);
    }
}