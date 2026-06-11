using AutoMapper;
using HorizonControlCenterBAL.Interfaces;
using HorizonControlCenterDAL.Entities;
using HorizonControlCenterDAL.Interfaces;
using HorizonControlCenterModels;
using HorizonControlCenterModels.Custom;
using HorizonControlCenterModels.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorizonControlCenterBAL
{
    public class SuitesApplicationBAL : ISuitesApplicationBAL
    {
        private readonly ISuitesApplicationDAL _dal;
        private readonly ISuiteDAL _suiteDAL;
        private readonly string _className;

        public SuitesApplicationBAL(ISuitesApplicationDAL dal, ISuiteDAL suiteDAL)
        {
            _dal = dal;
            _suiteDAL = suiteDAL;
            _className = GetType().Name;
        }

        public async Task<List<SuitesApplicationModel>> GetAllAsync()
        {
            return await _dal.GetAllAsync();
        }

        public async Task<SuitesApplicationModel?> GetByIdAsync(Guid id)
        {
            return await _dal.GetByIdAsync(id);
        }

        public async Task<GlobalResponseModel<SuitesApplicationModel>> CreateAsync(SuitesApplicationDTO model, int userId)
        {
            const string methodName = nameof(CreateAsync);
            try
            {
                // Validate SuiteId exists
                if (model.SuiteId.HasValue)
                {
                    var suite = await _suiteDAL.GetByIdAsync(model.SuiteId.Value);
                    if (suite == null)
                    {
                        Log.ForContext("classname", _className)
                           .ForContext("method_name", methodName)
                           .Warning("Invalid SuiteId: {SuiteId} does not exist", model.SuiteId.Value);
                        return Custom.CreateError<SuitesApplicationModel>("400", "Suite Application - create - SuiteId is invalid", obj: null);
                    }
                }

                SuitesApplicationModel? existing = await _dal.GetExistingApplicationAsync(model);
                if (existing == null)
                {
                    var (result, message) = await _dal.CreateAsync(model, userId);
                    if (result == null)
                    {
                        return Custom.CreateError<SuitesApplicationModel>("500", "Suite Application - create - no records were created", obj: null);
                    }
                    Log.ForContext("classname", _className)
                       .ForContext("method_name", methodName)
                       .Information("Suite Application created successfully at {Time}", DateTime.Now);
                    return Custom.CreateSuccess<SuitesApplicationModel>(objectId: result.GuidId, obj: result, message: "Suite Application - create - successful");
                }
                else
                {
                    return Custom.CreateError<SuitesApplicationModel>("409", "Suite Application - create - record already exists", obj: existing, objectId: existing.GuidId);
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

        public async Task<SuitesApplicationModel> PatchAsync(Guid id, JsonPatchDocument<SuiteApplication> patchDoc, int userId)
        {
            return await _dal.PatchAsync(id, patchDoc, userId);
        }

        public async Task<GlobalResponseModel<SuitesApplicationModel>> UpdateAsync(Guid id, SuitesApplicationDTO model, int userId)
        {
            const string methodName = nameof(UpdateAsync);
            try
            {
                var existing = await _dal.GetByIdAsync(id);
                if (existing == null)
                {
                    return Custom.CreateError<SuitesApplicationModel>("404", "Suite Application - update - record not found", obj: null);
                }

                // Validate SuiteId exists if provided
                if (model.SuiteId.HasValue)
                {
                    var suite = await _suiteDAL.GetByIdAsync(model.SuiteId.Value);
                    if (suite == null)
                    {
                        Log.ForContext("classname", _className)
                           .ForContext("method_name", methodName)
                           .Warning("Invalid SuiteId: {SuiteId} does not exist", model.SuiteId.Value);
                        return Custom.CreateError<SuitesApplicationModel>("400", "Suite Application - update - SuiteId is invalid", obj: null);
                    }
                }

                var result = await _dal.UpdateAsync(id, model, userId);
                if (result == null)
                {
                    return Custom.CreateError<SuitesApplicationModel>("500", "Suite Application - update - failed to update record", obj: null);
                }

                Log.ForContext("classname", _className)
                   .ForContext("method_name", methodName)
                   .Information("Suite Application updated successfully at {Time}", DateTime.Now);
                return Custom.CreateSuccess<SuitesApplicationModel>(objectId: result.GuidId, obj: result, message: "Suite Application - update - successful");
            }
            catch (Exception ex)
            {
                Log.ForContext("classname", _className)
                   .ForContext("method_name", methodName)
                   .Error(ex, "Error occurred in {MethodName}", methodName);
                throw;
            }
        }

        public async Task<GlobalResponseModel<SuitesApplicationModel>> DeleteAsync(Guid id, int userId)
        {
            const string methodName = nameof(DeleteAsync);
            try
            {
                var existing = await _dal.GetByIdAsync(id);
                if (existing == null)
                {
                    return Custom.CreateError<SuitesApplicationModel>("404", "Suite Application - delete - record not found", obj: null);
                }

                var result = await _dal.DeleteAsync(id, userId);
                if (result == 0)
                {
                    return Custom.CreateError<SuitesApplicationModel>("500", "Suite Application - delete - failed to delete record", obj: null);
                }

                Log.ForContext("classname", _className)
                   .ForContext("method_name", methodName)
                   .Information("Suite Application deleted successfully at {Time}", DateTime.Now);
                return Custom.CreateSuccess<SuitesApplicationModel>(objectId: id, obj: existing, message: "Suite Application - delete - successful");
            }
            catch (Exception ex)
            {
                Log.ForContext("classname", _className)
                   .ForContext("method_name", methodName)
                   .Error(ex, "Error occurred in {MethodName}", methodName);
                throw;
            }
        }

        public async Task<GlobalResponseModel<SuitesApplicationModel>> PermanentDeleteAsync(Guid id)
        {
            const string methodName = nameof(PermanentDeleteAsync);
            try
            {
                var existing = await _dal.GetByIdAsync(id);
                if (existing == null)
                {
                    return Custom.CreateError<SuitesApplicationModel>("404", "Suite Application - permanent delete - record not found", obj: null);
                }

                var result = await _dal.PermanentDeleteAsync(id);
                if (result == 0)
                {
                    return Custom.CreateError<SuitesApplicationModel>("500", "Suite Application - permanent delete - failed to delete record", obj: null);
                }

                Log.ForContext("classname", _className)
                   .ForContext("method_name", methodName)
                   .Information("Suite Application permanently deleted successfully at {Time}", DateTime.Now);
                return Custom.CreateSuccess<SuitesApplicationModel>(objectId: id, obj: existing, message: "Suite Application - permanent delete - successful");
            }
            catch (Exception ex)
            {
                Log.ForContext("classname", _className)
                   .ForContext("method_name", methodName)
                   .Error(ex, "Error occurred in {MethodName}", methodName);
                throw;
            }
        }

        public async Task<List<string>> GetDistinctApplicationTypesAsync()
        {
            return await _dal.GetDistinctApplicationTypesAsync();
        }
    }
}
