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
    public class UserGroupBAL : IUserGroupBAL
    {
        private readonly IUserGroupDAL _dal;
        private readonly string _className;

        public UserGroupBAL(IUserGroupDAL dal)
        {
            _dal = dal;
            _className = GetType().Name;
        }

        public async Task<List<UserGroupModel>> GetAllAsync()
        {
            return await _dal.GetAllAsync();
        }

        public async Task<UserGroupModel?> GetByIdAsync(Guid id)
        {
            return await _dal.GetByIdAsync(id);
        }

        public async Task<GlobalResponseModel<UserGroupModel>> CreateAsync(UserGroupDTO model, int userId)
        {
            const string methodName = nameof(CreateAsync);
            try
            {
                // Validate required properties
                if (model.UserId == null || model.UserId == Guid.Empty)
                {
                    return Custom.CreateError<UserGroupModel>("400", "UserGroup - create - UserId is required and cannot be null or empty", obj: null);
                }

                if (model.GroupId == null || model.GroupId == Guid.Empty)
                {
                    return Custom.CreateError<UserGroupModel>("400", "UserGroup - create - GroupId is required and cannot be null or empty", obj: null);
                }

                UserGroupModel? existing = await _dal.GetExistingUserGroupAsync(model);
                if (existing == null)
                {
                    var (result, message) = await _dal.CreateAsync(model, userId);
                    if (result == null)
                    {
                        return Custom.CreateError<UserGroupModel>("500", "UserGroup - create - no records were created", obj: null);
                    }
                    Log.ForContext("classname", _className)
                       .ForContext("method_name", methodName)
                       .Information("UserGroup created successfully at {Time}", DateTime.Now);
                    return Custom.CreateSuccess<UserGroupModel>(objectId: result.GuidId, obj: result, message: "UserGroup - create - successful");
                }
                else
                {
                    return Custom.CreateError<UserGroupModel>("409", "UserGroup - create - record already exists", obj: existing, objectId: existing.GuidId);
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

        public async Task<GlobalResponseModel<UserGroupModel>> UpdateAsync(Guid id, UserGroupDTO model, int userId)
        {
            const string methodName = nameof(UpdateAsync);
            try
            {
                // Validate required properties
                if (model.UserId == null || model.UserId == Guid.Empty)
                {
                    return Custom.CreateError<UserGroupModel>("400", "UserGroup - update - UserId is required and cannot be null or empty", obj: null);
                }

                if (model.GroupId == null || model.GroupId == Guid.Empty)
                {
                    return Custom.CreateError<UserGroupModel>("400", "UserGroup - update - GroupId is required and cannot be null or empty", obj: null);
                }

                var existing = await _dal.GetByIdAsync(id);
                if (existing == null)
                {
                    return Custom.CreateError<UserGroupModel>("404", "UserGroup - update - record not found", obj: null);
                }

                var result = await _dal.UpdateAsync(id, model, userId);
                if (result == null)
                {
                    return Custom.CreateError<UserGroupModel>("500", "UserGroup - update - failed to update record", obj: null);
                }

                Log.ForContext("classname", _className)
                   .ForContext("method_name", methodName)
                   .Information("UserGroup updated successfully at {Time}", DateTime.Now);
                return Custom.CreateSuccess<UserGroupModel>(objectId: result.GuidId, obj: result, message: "UserGroup - update - successful");
            }
            catch (Exception ex)
            {
                Log.ForContext("classname", _className)
                   .ForContext("method_name", methodName)
                   .Error(ex, "Error occurred in {MethodName}", methodName);
                throw;
            }
        }

        public async Task<GlobalResponseModel<UserGroupModel>> DeleteAsync(Guid id, int userId)
        {
            const string methodName = nameof(DeleteAsync);
            try
            {
                var existing = await _dal.GetByIdAsync(id);
                if (existing == null)
                {
                    return Custom.CreateError<UserGroupModel>("404", "UserGroup - delete - record not found", obj: null);
                }

                var result = await _dal.DeleteAsync(id, userId);
                if (result == 0)
                {
                    return Custom.CreateError<UserGroupModel>("500", "UserGroup - delete - failed to delete record", obj: null);
                }

                Log.ForContext("classname", _className)
                   .ForContext("method_name", methodName)
                   .Information("UserGroup deleted successfully at {Time}", DateTime.Now);
                return Custom.CreateSuccess<UserGroupModel>(objectId: id, obj: existing, message: "UserGroup - delete - successful");
            }
            catch (Exception ex)
            {
                Log.ForContext("classname", _className)
                   .ForContext("method_name", methodName)
                   .Error(ex, "Error occurred in {MethodName}", methodName);
                throw;
            }
        }

        public async Task<GlobalResponseModel<UserGroupModel>> PermanentDeleteAsync(Guid id)
        {
            const string methodName = nameof(PermanentDeleteAsync);
            try
            {
                var existing = await _dal.GetByIdAsync(id);
                if (existing == null)
                {
                    return Custom.CreateError<UserGroupModel>("404", "UserGroup - permanent delete - record not found", obj: null);
                }

                var result = await _dal.PermanentDeleteAsync(id);
                if (result == 0)
                {
                    return Custom.CreateError<UserGroupModel>("500", "UserGroup - permanent delete - failed to delete record", obj: null);
                }

                Log.ForContext("classname", _className)
                   .ForContext("method_name", methodName)
                   .Information("UserGroup permanently deleted successfully at {Time}", DateTime.Now);
                return Custom.CreateSuccess<UserGroupModel>(objectId: id, obj: existing, message: "UserGroup - permanent delete - successful");
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
