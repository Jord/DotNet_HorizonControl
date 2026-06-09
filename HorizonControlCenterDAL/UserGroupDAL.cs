using AutoMapper;
using HorizonControlCenterDAL.Entities;
using HorizonControlCenterDAL.Interfaces;
using HorizonControlCenterModels;
using HorizonControlCenterModels.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HorizonControlCenterDAL
{
    public class UserGroupDAL : IUserGroupDAL
    {
        private readonly horizoncontrolContext _context;
        private readonly IMapper _mapper;
        private readonly string _className;

        public UserGroupDAL(IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _context = new horizoncontrolContext(AppConfiguration.ngsqlConnectionOptions());
            _mapper = mapper;
            _className = GetType().Name;
        }

        public async Task<List<UserGroupModel>> GetAllAsync()
        {
            const string methodName = nameof(GetAllAsync);
            try
            {
                var data = await _context.UserGroups.AsNoTracking().ToListAsync();

                Log.ForContext("classname", _className)
                    .ForContext("method_name", methodName)
                    .ForContext("thrown_by", "Application")
                    .Information("Executed {MethodName} at {Time}", methodName, DateTime.Now);

                return _mapper.Map<List<UserGroupModel>>(data);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred in {MethodName}", methodName);
                throw;
            }
        }

        public async Task<UserGroupModel?> GetByIdAsync(Guid id)
        {
            const string methodName = nameof(GetByIdAsync);
            try
            {
                var entity = await _context.UserGroups
                            .FirstOrDefaultAsync(j => j.GuidId == id);

                Log.ForContext("classname", _className)
                   .ForContext("method_name", methodName)
                   .ForContext("thrown_by", "Application")
                   .Information("Executed {MethodName} at {Time}", methodName, DateTime.Now);

                return entity == null ? null : _mapper.Map<UserGroupModel>(entity);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred in {MethodName}", methodName);
                throw;
            }
        }

        public async Task<UserGroupModel?> GetExistingUserGroupAsync(UserGroupDTO model)
        {
            const string methodName = nameof(GetExistingUserGroupAsync);
            try
            {
                var existingEntity = await _context.UserGroups
                    .AsNoTracking()
                    .FirstOrDefaultAsync(s => s.UserId == model.UserId && s.GroupId == model.GroupId);

                if (existingEntity != null)
                {
                    Log.ForContext("classname", _className)
                       .ForContext("method_name", methodName)
                       .Information("Existing UserGroup found at {Time}", DateTime.Now);
                }

                return existingEntity == null ? null : _mapper.Map<UserGroupModel>(existingEntity);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred in {MethodName}", methodName);
                throw;
            }
        }

        public async Task<(UserGroupModel result, string message)> CreateAsync(UserGroupDTO model, int userId)
        {
            const string methodName = nameof(CreateAsync);

            try
            {
                var exists = await _context.UserGroups.AnyAsync(s => s.UserId == model.UserId && s.GroupId == model.GroupId);

                if (exists)
                {
                    string message = "UserGroup mapping already exists";

                    Log.ForContext("classname", _className)
                       .ForContext("method_name", methodName)
                       .ForContext("user_id", userId)
                       .Warning("Duplicate detected: {Message}", message);

                    return (null, message);
                }

                var entity = _mapper.Map<UserGroup>(model);

                entity.CreationDate = DateTime.UtcNow;
                entity.LastUpdatedDate = DateTime.UtcNow;
                entity.CreatedByUserId = userId;
                entity.LastUpdatedByUserId = userId;
                entity.IsActive = true;
                await _context.UserGroups.AddAsync(entity);
                await _context.SaveChangesAsync();

                return (_mapper.Map<UserGroupModel>(entity), "Created successfully");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in {MethodName}", methodName);
                throw;
            }
        }

        public async Task<UserGroupModel> UpdateAsync(Guid id, UserGroupDTO model, int userId)
        {
            const string methodName = nameof(UpdateAsync);
            try
            {
                var entity = await _context.UserGroups
                    .FirstOrDefaultAsync(j => j.GuidId == id);

                if (entity == null)
                    throw new Exception("Record not found");

                if (model != null)
                {
                    _mapper.Map(model, entity);
                }

                entity.LastUpdatedDate = DateTime.UtcNow;
                entity.LastUpdatedByUserId = userId;

                await _context.SaveChangesAsync();

                return _mapper.Map<UserGroupModel>(entity);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in {MethodName}", methodName);
                throw;
            }
        }

        public async Task<int> DeleteAsync(Guid id, int userId)
        {
            const string methodName = nameof(DeleteAsync);

            try
            {
                var entity = await _context.UserGroups
                    .Where(j => j.GuidId == id)
                    .FirstOrDefaultAsync();

                if (entity == null)
                {
                    return 0; // Not found
                }

                // ? Soft delete
                entity.IsActive = false;
                entity.LastUpdatedByUserId = userId;
                entity.LastUpdatedDate = DateTime.UtcNow;

                _context.UserGroups.Update(entity);
                await _context.SaveChangesAsync();

                Log.ForContext("classname", _className)
                   .ForContext("method_name", methodName)
                   .ForContext("thrown_by", "Application")
                   .Information("Soft deleted record with GuidId {Id} at {Time}", id, DateTime.Now);

                return 1; // Success
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in {MethodName}", methodName);
                throw;
            }
        }

        public async Task<int> PermanentDeleteAsync(Guid id)
        {
            const string methodName = nameof(PermanentDeleteAsync);

            try
            {
                var entity = await _context.UserGroups
                    .Where(j => j.GuidId == id)
                    .FirstOrDefaultAsync();

                if (entity == null)
                {
                    return 0; // Not found
                }

                // ? Permanent delete
                _context.UserGroups.Remove(entity);
                await _context.SaveChangesAsync();

                Log.ForContext("classname", _className)
                   .ForContext("method_name", methodName)
                   .ForContext("thrown_by", "Application")
                   .Information("Permanently deleted record with GuidId {Id} at {Time}", id, DateTime.Now);

                return 1; // Success
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in {MethodName}", methodName);
                throw;
            }
        }
    }
}
