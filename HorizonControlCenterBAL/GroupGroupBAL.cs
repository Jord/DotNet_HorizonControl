using HorizonControlCenterBAL.Interfaces;
using HorizonControlCenterDAL.Interfaces;
using HorizonControlCenterModels;
using HorizonControlCenterModels.Custom;
using HorizonControlCenterModels.DTO;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HorizonControlCenterBAL
{
    public class GroupGroupBAL : IGroupGroupBAL
    {
        private readonly IGroupGroupDAL _dal;
        private readonly string _className;

        public GroupGroupBAL(IGroupGroupDAL dal)
        {
            _dal = dal;
            _className = GetType().Name;
        }

        public async Task<List<GroupGroupModel>> GetAllAsync()
        {
            return await _dal.GetAllAsync();
        }

        public async Task<GroupGroupModel?> GetByIdAsync(Guid id)
        {
            return await _dal.GetByIdAsync(id);
        }

        public async Task<GlobalResponseModel<GroupGroupModel>> CreateAsync(GroupGroupDTO model, int userId)
        {
            const string methodName = nameof(CreateAsync);
            try
            {
                // Validate required properties
                if (model.GroupId == Guid.Empty)
                {
                    return Custom.CreateError<GroupGroupModel>("400", "GroupGroup - create - GroupId is required and cannot be empty", obj: null);
                }

                if (model.MapToGroupId == Guid.Empty)
                {
                    return Custom.CreateError<GroupGroupModel>("400", "GroupGroup - create - MapToGroupId is required and cannot be empty", obj: null);
                }

                GroupGroupModel? existing = await _dal.GetExistingGroupGroupAsync(model);
                if (existing == null)
                {
                    var (result, message) = await _dal.CreateAsync(model, userId);
                    if (result == null)
                    {
                        return Custom.CreateError<GroupGroupModel>("500", "GroupGroup - create - no records were created", obj: null);
                    }
                    Log.ForContext("classname", _className)
                       .ForContext("method_name", methodName)
                       .Information("GroupGroup created successfully at {Time}", DateTime.Now);
                    return Custom.CreateSuccess<GroupGroupModel>(objectId: result.GuidId, obj: result, message: "GroupGroup - create - successful");
                }
                else
                {
                    return Custom.CreateError<GroupGroupModel>("409", "GroupGroup - create - record already exists", obj: existing, objectId: existing.GuidId);
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

        public async Task<GlobalResponseModel<GroupGroupModel>> UpdateAsync(Guid id, GroupGroupDTO model, int userId)
        {
            const string methodName = nameof(UpdateAsync);
            try
            {
                // Validate required properties
                if (model.GroupId == Guid.Empty)
                {
                    return Custom.CreateError<GroupGroupModel>("400", "GroupGroup - update - GroupId is required and cannot be empty", obj: null);
                }

                if (model.MapToGroupId == Guid.Empty)
                {
                    return Custom.CreateError<GroupGroupModel>("400", "GroupGroup - update - MapToGroupId is required and cannot be empty", obj: null);
                }

                var existing = await _dal.GetByIdAsync(id);
                if (existing == null)
                {
                    return Custom.CreateError<GroupGroupModel>("404", "GroupGroup - update - record not found", obj: null);
                }

                var result = await _dal.UpdateAsync(id, model, userId);
                if (result == null)
                {
                    return Custom.CreateError<GroupGroupModel>("500", "GroupGroup - update - failed to update record", obj: null);
                }

                Log.ForContext("classname", _className)
                   .ForContext("method_name", methodName)
                   .Information("GroupGroup updated successfully at {Time}", DateTime.Now);
                return Custom.CreateSuccess<GroupGroupModel>(objectId: result.GuidId, obj: result, message: "GroupGroup - update - successful");
            }
            catch (Exception ex)
            {
                Log.ForContext("classname", _className)
                   .ForContext("method_name", methodName)
                   .Error(ex, "Error occurred in {MethodName}", methodName);
                throw;
            }
        }

        public async Task<GlobalResponseModel<GroupGroupModel>> DeleteAsync(Guid id)
        {
            const string methodName = nameof(DeleteAsync);
            try
            {
                var existing = await _dal.GetByIdAsync(id);
                if (existing == null)
                {
                    return Custom.CreateError<GroupGroupModel>("404", "GroupGroup - delete - record not found", obj: null);
                }

                var result = await _dal.DeleteAsync(id);
                if (result == 0)
                {
                    return Custom.CreateError<GroupGroupModel>("500", "GroupGroup - delete - failed to delete record", obj: null);
                }

                Log.ForContext("classname", _className)
                   .ForContext("method_name", methodName)
                   .Information("GroupGroup deleted successfully at {Time}", DateTime.Now);
                return Custom.CreateSuccess<GroupGroupModel>(objectId: id, obj: existing, message: "GroupGroup - delete - successful");
            }
            catch (Exception ex)
            {
                Log.ForContext("classname", _className)
                   .ForContext("method_name", methodName)
                   .Error(ex, "Error occurred in {MethodName}", methodName);
                throw;
            }
        }

        public async Task<GlobalResponseModel<GroupGroupModel>> PermanentDeleteAsync(Guid id)
        {
            const string methodName = nameof(PermanentDeleteAsync);
            try
            {
                var existing = await _dal.GetByIdAsync(id);
                if (existing == null)
                {
                    return Custom.CreateError<GroupGroupModel>("404", "GroupGroup - permanent delete - record not found", obj: null);
                }

                var result = await _dal.PermanentDeleteAsync(id);
                if (result == 0)
                {
                    return Custom.CreateError<GroupGroupModel>("500", "GroupGroup - permanent delete - failed to delete record", obj: null);
                }

                Log.ForContext("classname", _className)
                   .ForContext("method_name", methodName)
                   .Information("GroupGroup permanently deleted successfully at {Time}", DateTime.Now);
                return Custom.CreateSuccess<GroupGroupModel>(objectId: id, obj: existing, message: "GroupGroup - permanent delete - successful");
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
