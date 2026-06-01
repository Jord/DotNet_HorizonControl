using AutoMapper;
using HorizonControlCenterDAL;
using HorizonControlCenterDAL.Entities;
using HorizonControlCenterModels;
using HorizonControlCenterModels.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorizonControlCenterBAL
{
    public class SuitesApplicationBAL
    {
        private readonly SuitesApplicationDAL _dal;

        public SuitesApplicationBAL(IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _dal = new SuitesApplicationDAL(httpContextAccessor, mapper);
        }

        public async Task<List<SuitesApplicationModel>> GetAllAsync()
        {
            return await _dal.GetAllAsync();
        }

        public async Task<SuitesApplicationModel?> GetByIdAsync(Guid id)
        {
            return await _dal.GetByIdAsync(id);
        }

        public async Task<(SuitesApplicationModel result, string message)> CreateAsync(SuitesApplicationDTO model, int userId)
        {
            return await _dal.CreateAsync(model, userId);
        }

        public async Task<SuitesApplicationModel> PatchAsync(Guid id, JsonPatchDocument<SuitesApplication> patchDoc, int userId)
        {
            return await _dal.PatchAsync(id, patchDoc, userId);
        }

        public async Task<SuitesApplicationModel> UpdateAsync(Guid id, SuitesApplicationDTO model, int userId)
        {
            return await _dal.UpdateAsync(id, model, userId);
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            return await _dal.DeleteAsync(id);
        }

        public async Task<List<string>> GetDistinctApplicationTypesAsync()
        {
            return await _dal.GetDistinctApplicationTypesAsync();
        }
    }
}
