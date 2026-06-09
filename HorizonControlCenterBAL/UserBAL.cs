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
    public class UserBAL : IUserBAL
    {
        private readonly IUserDAL _dal;
        private readonly string _className;

        public UserBAL(IUserDAL dal)
        {
            _dal = dal;
            _className = GetType().Name;
        }

        public async Task<List<UserModel>> GetAllAsync()
        {
            return await _dal.GetAllAsync();
        }

        public async Task<UserModel?> GetByIdAsync(Guid id)
        {
            return await _dal.GetByIdAsync(id);
        }

        public async Task<GlobalResponseModel<UserModel>> CreateAsync(UserDTO model, int userId)
        {
            const string methodName = nameof(CreateAsync);
            try
            {
                UserModel? existing = await _dal.GetExistingUserAsync(model);
                if (existing == null)
                {
                    var (result, message) = await _dal.CreateAsync(model, userId);
                    if (result == null)
                    {
                        return Custom.CreateError<UserModel>("500", "User - create - no records were created", obj: null);
                    }
                    Log.ForContext("classname", _className)
                       .ForContext("method_name", methodName)
                       .Information("User created successfully at {Time}", DateTime.Now);
                    return Custom.CreateSuccess<UserModel>(objectId: result.GuidId, obj: result, message: "User - create - successful");
                }
                else
                {
                    return Custom.CreateError<UserModel>("409", "User - create - record already exists", obj: existing, objectId: existing.GuidId);
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

        public async Task<GlobalResponseModel<UserModel>> UpdateAsync(Guid id, UserDTO model, int userId)
        {
            const string methodName = nameof(UpdateAsync);
            try
            {
                var existing = await _dal.GetByIdAsync(id);
                if (existing == null)
                {
                    return Custom.CreateError<UserModel>("404", "User - update - record not found", obj: null);
                }

                var result = await _dal.UpdateAsync(id, model, userId);
                if (result == null)
                {
                    return Custom.CreateError<UserModel>("500", "User - update - failed to update record", obj: null);
                }

                Log.ForContext("classname", _className)
                   .ForContext("method_name", methodName)
                   .Information("User updated successfully at {Time}", DateTime.Now);
                return Custom.CreateSuccess<UserModel>(objectId: result.GuidId, obj: result, message: "User - update - successful");
            }
            catch (Exception ex)
            {
                Log.ForContext("classname", _className)
                   .ForContext("method_name", methodName)
                   .Error(ex, "Error occurred in {MethodName}", methodName);
                throw;
            }
        }

        public async Task<GlobalResponseModel<UserModel>> DeleteAsync(Guid id, int userId)
        {
            const string methodName = nameof(DeleteAsync);
            try
            {
                var existing = await _dal.GetByIdAsync(id);
                if (existing == null)
                {
                    return Custom.CreateError<UserModel>("404", "User - delete - record not found", obj: null);
                }

                var result = await _dal.DeleteAsync(id, userId);
                if (result == 0)
                {
                    return Custom.CreateError<UserModel>("500", "User - delete - failed to delete record", obj: null);
                }

                Log.ForContext("classname", _className)
                   .ForContext("method_name", methodName)
                   .Information("User deleted successfully at {Time}", DateTime.Now);
                return Custom.CreateSuccess<UserModel>(objectId: id, obj: existing, message: "User - delete - successful");
            }
            catch (Exception ex)
            {
                Log.ForContext("classname", _className)
                   .ForContext("method_name", methodName)
                   .Error(ex, "Error occurred in {MethodName}", methodName);
                throw;
            }
        }

        public async Task<GlobalResponseModel<UserModel>> PermanentDeleteAsync(Guid id)
        {
            const string methodName = nameof(PermanentDeleteAsync);
            try
            {
                var existing = await _dal.GetByIdAsync(id);
                if (existing == null)
                {
                    return Custom.CreateError<UserModel>("404", "User - permanent delete - record not found", obj: null);
                }

                var result = await _dal.PermanentDeleteAsync(id);
                if (result == 0)
                {
                    return Custom.CreateError<UserModel>("500", "User - permanent delete - failed to delete record", obj: null);
                }

                Log.ForContext("classname", _className)
                   .ForContext("method_name", methodName)
                   .Information("User permanently deleted successfully at {Time}", DateTime.Now);
                return Custom.CreateSuccess<UserModel>(objectId: id, obj: existing, message: "User - permanent delete - successful");
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
