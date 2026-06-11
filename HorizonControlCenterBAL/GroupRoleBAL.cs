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
    public class GroupRoleBAL : IGroupRoleBAL
    {
        private readonly IGroupRoleDAL _dal;
        private readonly string _className;

        public GroupRoleBAL(IGroupRoleDAL dal)
        {
            _dal = dal;
            _className = GetType().Name;
        }

        public async Task<List<GroupRoleModel>> GetAllAsync()
        {
            return await _dal.GetAllAsync();
        }

        public async Task<GroupRoleModel?> GetByIdAsync(Guid id)
        {
            return await _dal.GetByIdAsync(id);
        }

        public async Task<GlobalResponseModel<GroupRoleModel>> CreateAsync(GroupRoleDTO model, int userId)
        {
            const string methodName = nameof(CreateAsync);
            try
            {
                GroupRoleModel? existing = await _dal.GetExistingGroupRoleAsync(model);
                if (existing == null)
                {
                    var (result, message) = await _dal.CreateAsync(model, userId);
                    if (result == null)
                    {
                        return Custom.CreateError<GroupRoleModel>("500", "GroupRole - create - no records were created", obj: null);
                    }
                    Log.ForContext("classname", _className)
                       .ForContext("method_name", methodName)
                       .Information("GroupRole created successfully at {Time}", DateTime.Now);
                    return Custom.CreateSuccess<GroupRoleModel>(objectId: result.GuidD, obj: result, message: "GroupRole - create - successful");
                }
                else
                {
                    return Custom.CreateError<GroupRoleModel>("409", "GroupRole - create - record already exists", obj: existing, objectId: existing.GuidD);
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

        public async Task<GlobalResponseModel<GroupRoleModel>> UpdateAsync(Guid id, GroupRoleDTO model, int userId)
        {
            const string methodName = nameof(UpdateAsync);
            try
            {
                var existing = await _dal.GetByIdAsync(id);
                if (existing == null)
                {
                    return Custom.CreateError<GroupRoleModel>("404", "GroupRole - update - record not found", obj: null);
                }

                var result = await _dal.UpdateAsync(id, model, userId);
                if (result == null)
                {
                    return Custom.CreateError<GroupRoleModel>("500", "GroupRole - update - failed to update record", obj: null);
                }

                Log.ForContext("classname", _className)
                   .ForContext("method_name", methodName)
                   .Information("GroupRole updated successfully at {Time}", DateTime.Now);
                return Custom.CreateSuccess<GroupRoleModel>(objectId: result.GuidD, obj: result, message: "GroupRole - update - successful");
            }
            catch (Exception ex)
            {
                Log.ForContext("classname", _className)
                   .ForContext("method_name", methodName)
                   .Error(ex, "Error occurred in {MethodName}", methodName);
                throw;
            }
        }

        public async Task<GlobalResponseModel<GroupRoleModel>> DeleteAsync(Guid id, int userId)
        {
            const string methodName = nameof(DeleteAsync);
            try
            {
                var existing = await _dal.GetByIdAsync(id);
                if (existing == null)
                {
                    return Custom.CreateError<GroupRoleModel>("404", "GroupRole - delete - record not found", obj: null);
                }

                var result = await _dal.DeleteAsync(id, userId);
                if (result == 0)
                {
                    return Custom.CreateError<GroupRoleModel>("500", "GroupRole - delete - failed to delete record", obj: null);
                }

                Log.ForContext("classname", _className)
                   .ForContext("method_name", methodName)
                   .Information("GroupRole deleted successfully at {Time}", DateTime.Now);
                return Custom.CreateSuccess<GroupRoleModel>(objectId: id, obj: existing, message: "GroupRole - delete - successful");
            }
            catch (Exception ex)
            {
                Log.ForContext("classname", _className)
                   .ForContext("method_name", methodName)
                   .Error(ex, "Error occurred in {MethodName}", methodName);
                throw;
            }
        }

        public async Task<GlobalResponseModel<GroupRoleModel>> PermanentDeleteAsync(Guid id)
        {
            const string methodName = nameof(PermanentDeleteAsync);
            try
            {
                var existing = await _dal.GetByIdAsync(id);
                if (existing == null)
                {
                    return Custom.CreateError<GroupRoleModel>("404", "GroupRole - permanent delete - record not found", obj: null);
                }

                var result = await _dal.PermanentDeleteAsync(id);
                if (result == 0)
                {
                    return Custom.CreateError<GroupRoleModel>("500", "GroupRole - permanent delete - failed to delete record", obj: null);
                }

                Log.ForContext("classname", _className)
                   .ForContext("method_name", methodName)
                   .Information("GroupRole permanently deleted successfully at {Time}", DateTime.Now);
                return Custom.CreateSuccess<GroupRoleModel>(objectId: id, obj: existing, message: "GroupRole - permanent delete - successful");
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
