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
    public class SuiteBAL
    {
        private readonly SuiteDAL _dal;

        public SuiteBAL(IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _dal = new SuiteDAL(httpContextAccessor, mapper);
        }

        public async Task<List<SuiteModel>> GetAllAsync()
        {
            return await _dal.GetAllAsync();
        }

        public async Task<SuiteModel?> GetByIdAsync(Guid id)
        {
            return await _dal.GetByIdAsync(id);
        }

        public async Task<(SuiteModel result, string message)> CreateAsync(SuitesDTO model, int userId)
        {
            return await _dal.CreateAsync(model, userId);
        }
        public async Task<SuiteModel> PatchAsync(Guid id, JsonPatchDocument<Suite> patchDoc, int userId)
        {
            return await _dal.PatchAsync(id, patchDoc, userId);
        }
        public async Task<SuiteModel> UpdateAsync(Guid id, SuitesDTO model, int userId)
        {
            return await _dal.UpdateAsync(id, model, userId);
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            return await _dal.DeleteAsync(id);
        }
    }
}
