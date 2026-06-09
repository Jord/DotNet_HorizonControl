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
    public class GroupRoleDAL : IGroupRoleDAL
    {
        private readonly horizoncontrolContext _context;
        private readonly IMapper _mapper;
        private readonly string _className;

        public GroupRoleDAL(IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _context = new horizoncontrolContext(AppConfiguration.ngsqlConnectionOptions());
            _mapper = mapper;
            _className = GetType().Name;
        }

        public async Task<List<GroupRoleModel>> GetAllAsync()
        {
            const string methodName = nameof(GetAllAsync);
            try
            {
                var data = await _context.GroupRoles.AsNoTracking().ToListAsync();

                Log.ForContext("classname", _className)
                    .ForContext("method_name", methodName)
                    .ForContext("thrown_by", "Application")
                    .Information("Executed {MethodName} at {Time}", methodName, DateTime.Now);

                return _mapper.Map<List<GroupRoleModel>>(data);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred in {MethodName}", methodName);
                throw;
            }
        }

        public async Task<GroupRoleModel?> GetByIdAsync(Guid id)
        {
            const string methodName = nameof(GetByIdAsync);
            try
            {
                var entity = await _context.GroupRoles
                            .FirstOrDefaultAsync(j => j.GuidD == id);

                Log.ForContext("classname", _className)
                   .ForContext("method_name", methodName)
                   .ForContext("thrown_by", "Application")
                   .Information("Executed {MethodName} at {Time}", methodName, DateTime.Now);

                return entity == null ? null : _mapper.Map<GroupRoleModel>(entity);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred in {MethodName}", methodName);
                throw;
            }
        }

        public async Task<GroupRoleModel?> GetExistingGroupRoleAsync(GroupRoleDTO model)
        {
            const string methodName = nameof(GetExistingGroupRoleAsync);
            try
            {
                var existingEntity = await _context.GroupRoles
                    .AsNoTracking()
                    .FirstOrDefaultAsync(s => model.GroupName != null && s.GroupName == model.GroupName);

                if (existingEntity != null)
                {
                    Log.ForContext("classname", _className)
                       .ForContext("method_name", methodName)
                       .Information("Existing GroupRole found at {Time}", DateTime.Now);
                }

                return existingEntity == null ? null : _mapper.Map<GroupRoleModel>(existingEntity);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred in {MethodName}", methodName);
                throw;
            }
        }

        public async Task<(GroupRoleModel result, string message)> CreateAsync(GroupRoleDTO model, int userId)
        {
            const string methodName = nameof(CreateAsync);

            try
            {
                var nameExists = model.GroupName != null &&
                    await _context.GroupRoles.AnyAsync(s => s.GroupName == model.GroupName);

                if (nameExists)
                {
                    string message = "Group Name already exists";

                    Log.ForContext("classname", _className)
                       .ForContext("method_name", methodName)
                       .ForContext("user_id", userId)
                       .Warning("Duplicate detected: {Message}", message);

                    return (null, message);
                }

                var entity = _mapper.Map<GroupRole>(model);

                entity.CreationDate = DateTime.UtcNow;
                entity.LastUpdatedDate = DateTime.UtcNow;
                entity.IsActive = true;
                entity.CreateByUserId = userId;
                entity.LastUpdatedByUserId = userId;

                await _context.GroupRoles.AddAsync(entity);
                await _context.SaveChangesAsync();

                return (_mapper.Map<GroupRoleModel>(entity), "Created successfully");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in {MethodName}", methodName);
                throw;
            }
        }

        public async Task<GroupRoleModel> UpdateAsync(Guid id, GroupRoleDTO model, int userId)
        {
            const string methodName = nameof(UpdateAsync);
            try
            {
                var entity = await _context.GroupRoles
                    .FirstOrDefaultAsync(j => j.GuidD == id);

                if (entity == null)
                    throw new Exception("Record not found");

                if (model != null)
                {
                    _mapper.Map(model, entity);
                }

                entity.LastUpdatedDate = DateTime.UtcNow;
                entity.LastUpdatedByUserId = userId;

                await _context.SaveChangesAsync();

                return _mapper.Map<GroupRoleModel>(entity);
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
                var entity = await _context.GroupRoles
                    .Where(j => j.GuidD == id)
                    .FirstOrDefaultAsync();

                if (entity == null)
                {
                    return 0; // Not found
                }

                // ? Soft delete
                entity.IsActive = false;
                entity.LastUpdatedByUserId = userId;
                entity.LastUpdatedDate = DateTime.UtcNow;

                _context.GroupRoles.Update(entity);
                await _context.SaveChangesAsync();

                Log.ForContext("classname", _className)
                   .ForContext("method_name", methodName)
                   .ForContext("thrown_by", "Application")
                   .Information("Soft deleted record with GuidD {Id} at {Time}", id, DateTime.Now);

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
                var entity = await _context.GroupRoles
                    .Where(j => j.GuidD == id)
                    .FirstOrDefaultAsync();

                if (entity == null)
                {
                    return 0; // Not found
                }

                // ? Permanent delete
                _context.GroupRoles.Remove(entity);
                await _context.SaveChangesAsync();

                Log.ForContext("classname", _className)
                   .ForContext("method_name", methodName)
                   .ForContext("thrown_by", "Application")
                   .Information("Permanently deleted record with GuidD {Id} at {Time}", id, DateTime.Now);

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
