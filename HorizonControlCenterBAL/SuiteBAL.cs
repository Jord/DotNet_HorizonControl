using HorizonControlCenterBAL.Interfaces;
using HorizonControlCenterDAL.Entities;
using HorizonControlCenterDAL.Interfaces;
using HorizonControlCenterModels;
using HorizonControlCenterModels.Custom;
using HorizonControlCenterModels.DTO;
using Microsoft.AspNetCore.JsonPatch;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorizonControlCenterBAL
{
    public class SuiteBAL : ISuiteBAL
    {
        private readonly ISuiteDAL _dal;
        private readonly string _className;

        public SuiteBAL(ISuiteDAL dal)
        {
            _dal = dal;
            _className = GetType().Name;
        }

        public async Task<List<SuiteModel>> GetAllAsync()
        {
            return await _dal.GetAllAsync();
        }

        public async Task<SuiteModel?> GetByIdAsync(Guid id)
        {
            return await _dal.GetByIdAsync(id);
        }

        public async Task<GlobalResponseModel<SuiteModel>> CreateAsync(SuitesDTO model, int userId)
        {
            const string methodName = nameof(CreateAsync);
            try
            {
                SuiteModel? existing = await _dal.GetExistingSuiteAsync(model);
                if (existing == null)
                {
                    var (result, message) = await _dal.CreateAsync(model, userId);
                    if (result == null)
                    {
                        return Custom.CreateError<SuiteModel>("500", "Suite - create - no records were created", obj: null);
                    }
                    Log.ForContext("classname", _className)
                       .ForContext("method_name", methodName)
                       .Information("Suite created successfully at {Time}", DateTime.Now);
                    return Custom.CreateSuccess<SuiteModel>(objectId: result.GuidId, obj: result, message: "Suite - create - successful");
                }
                else
                {
                    return Custom.CreateError<SuiteModel>("409", "Suite - create - record already exists", obj: existing, objectId: existing.GuidId);
                }
            }
            catch (Exception ex)
            {
                Log.ForContext("classname", _className)
                   .ForContext("method_name", methodName)
                   .Error(ex, "Error occurred in {MethodName}", methodName);
                throw;
            }
        }

        public async Task<SuiteModel> PatchAsync(Guid id, JsonPatchDocument<Suite> patchDoc, int userId)
        {
            return await _dal.PatchAsync(id, patchDoc, userId);
        }

        public async Task<GlobalResponseModel<SuiteModel>> UpdateAsync(Guid id, SuitesDTO model, int userId)
        {
            const string methodName = nameof(UpdateAsync);
            try
            {
                var existing = await _dal.GetByIdAsync(id);
                if (existing == null)
                {
                    return Custom.CreateError<SuiteModel>("404", "Suite - update - record not found", obj: null);
                }

                var result = await _dal.UpdateAsync(id, model, userId);
                if (result == null)
                {
                    return Custom.CreateError<SuiteModel>("500", "Suite - update - failed to update record", obj: null);
                }

                Log.ForContext("classname", _className)
                   .ForContext("method_name", methodName)
                   .Information("Suite updated successfully at {Time}", DateTime.Now);
                return Custom.CreateSuccess<SuiteModel>(objectId: result.GuidId, obj: result, message: "Suite - update - successful");
            }
            catch (Exception ex)
            {
                Log.ForContext("classname", _className)
                   .ForContext("method_name", methodName)
                   .Error(ex, "Error occurred in {MethodName}", methodName);
                throw;
            }
        }

        public async Task<GlobalResponseModel<SuiteModel>> DeleteAsync(Guid id, int userId)
        {
            const string methodName = nameof(DeleteAsync);
            try
            {
                var existing = await _dal.GetByIdAsync(id);
                if (existing == null)
                {
                    return Custom.CreateError<SuiteModel>("404", "Suite - delete - record not found", obj: null);
                }

                var result = await _dal.DeleteAsync(id, userId);
                if (result == 0)
                {
                    return Custom.CreateError<SuiteModel>("500", "Suite - delete - failed to delete record", obj: null);
                }

                Log.ForContext("classname", _className)
                   .ForContext("method_name", methodName)
                   .Information("Suite deleted successfully at {Time}", DateTime.Now);
                return Custom.CreateSuccess<SuiteModel>(objectId: id, obj: existing, message: "Suite - delete - successful");
            }
            catch (Exception ex)
            {
                Log.ForContext("classname", _className)
                   .ForContext("method_name", methodName)
                   .Error(ex, "Error occurred in {MethodName}", methodName);
                throw;
            }
        }

        public async Task<GlobalResponseModel<SuiteModel>> PermanentDeleteAsync(Guid id)
        {
            const string methodName = nameof(PermanentDeleteAsync);
            try
            {
                var existing = await _dal.GetByIdAsync(id);
                if (existing == null)
                {
                    return Custom.CreateError<SuiteModel>("404", "Suite - permanent delete - record not found", obj: null);
                }

                var result = await _dal.PermanentDeleteAsync(id);
                if (result == 0)
                {
                    return Custom.CreateError<SuiteModel>("500", "Suite - permanent delete - failed to delete record", obj: null);
                }

                Log.ForContext("classname", _className)
                   .ForContext("method_name", methodName)
                   .Information("Suite permanently deleted successfully at {Time}", DateTime.Now);
                return Custom.CreateSuccess<SuiteModel>(objectId: id, obj: existing, message: "Suite - permanent delete - successful");
            }
            catch (Exception ex)
            {
                Log.ForContext("classname", _className)
                   .ForContext("method_name", methodName)
                   .Error(ex, "Error occurred in {MethodName}", methodName);
                throw;
            }
        }
    }
}
