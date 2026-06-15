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

        public UserGroupDAL(horizoncontrolContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _className = GetType().Name;
        }

        public async Task<List<UserGroupModel>> GetAllAsync()
        {
            const string methodName = nameof(GetAllAsync);
            try
            {
                var data = await (from ug in _context.UserGroups
                                  join u in _context.Users on ug.UserId equals u.GuidId into userJoin
                                  from u in userJoin.DefaultIfEmpty()
                                  join g in _context.Groups on ug.GroupId equals g.GuidId into groupJoin
                                  from g in groupJoin.DefaultIfEmpty()
                                  select new UserGroupModel
                                  {
                                      GuidId = ug.GuidId,
                                      UserId = ug.UserId,
                                      GroupId = ug.GroupId,
                                      CreatedByUserId = ug.CreatedByUserId,
                                      CreationDate = ug.CreationDate,
                                      LastUpdatedByUserId = ug.LastUpdatedByUserId,
                                      LastUpdatedDate = ug.LastUpdatedDate,
                                      UserName = u != null ? u.UserFullName : null,
                                      GroupName = g != null ? g.Name : null
                                  }).AsNoTracking().ToListAsync();

                Log.ForContext("classname", _className)
                    .ForContext("method_name", methodName)
                    .ForContext("thrown_by", "Application")
                    .Information("Executed {MethodName} at {Time}", methodName, DateTime.Now);

                return data;
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
                var entity = await (from ug in _context.UserGroups
                                    join u in _context.Users on ug.UserId equals u.GuidId into userJoin
                                    from u in userJoin.DefaultIfEmpty()
                                    join g in _context.Groups on ug.GroupId equals g.GuidId into groupJoin
                                    from g in groupJoin.DefaultIfEmpty()
                                    where ug.GuidId == id
                                    select new UserGroupModel
                                    {
                                        GuidId = ug.GuidId,
                                        UserId = ug.UserId,
                                        GroupId = ug.GroupId,
                                        CreatedByUserId = ug.CreatedByUserId,
                                        CreationDate = ug.CreationDate,
                                        LastUpdatedByUserId = ug.LastUpdatedByUserId,
                                        LastUpdatedDate = ug.LastUpdatedDate,
                                        UserName = u != null ? u.UserFullName : null,
                                        GroupName = g != null ? g.Name : null
                                    }).FirstOrDefaultAsync();

                Log.ForContext("classname", _className)
                   .ForContext("method_name", methodName)
                   .ForContext("thrown_by", "Application")
                   .Information("Executed {MethodName} at {Time}", methodName, DateTime.Now);

                return entity;
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
                var existingEntity = await (from ug in _context.UserGroups
                                             join u in _context.Users on ug.UserId equals u.GuidId into userJoin
                                             from u in userJoin.DefaultIfEmpty()
                                             join g in _context.Groups on ug.GroupId equals g.GuidId into groupJoin
                                             from g in groupJoin.DefaultIfEmpty()
                                             where ug.UserId == model.UserId && ug.GroupId == model.GroupId
                                             select new UserGroupModel
                                             {
                                                 GuidId = ug.GuidId,
                                                 UserId = ug.UserId,
                                                 GroupId = ug.GroupId,
                                                 CreatedByUserId = ug.CreatedByUserId,
                                                 CreationDate = ug.CreationDate,
                                                 LastUpdatedByUserId = ug.LastUpdatedByUserId,
                                                 LastUpdatedDate = ug.LastUpdatedDate,
                                                 UserName = u != null ? u.UserFullName : null,
                                                 GroupName = g != null ? g.Name : null
                                             }).AsNoTracking().FirstOrDefaultAsync();

                if (existingEntity != null)
                {
                    Log.ForContext("classname", _className)
                       .ForContext("method_name", methodName)
                       .Information("Existing UserGroup found at {Time}", DateTime.Now);
                }

                return existingEntity;
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

                // Fetch the created entity with joined data
                var createdEntity = await (from ug in _context.UserGroups
                                           join u in _context.Users on ug.UserId equals u.GuidId into userJoin
                                           from u in userJoin.DefaultIfEmpty()
                                           join g in _context.Groups on ug.GroupId equals g.GuidId into groupJoin
                                           from g in groupJoin.DefaultIfEmpty()
                                           where ug.GuidId == entity.GuidId
                                           select new UserGroupModel
                                           {
                                               GuidId = ug.GuidId,
                                               UserId = ug.UserId,
                                               GroupId = ug.GroupId,
                                               CreatedByUserId = ug.CreatedByUserId,
                                               CreationDate = ug.CreationDate,
                                               LastUpdatedByUserId = ug.LastUpdatedByUserId,
                                               LastUpdatedDate = ug.LastUpdatedDate,
                                               UserName = u != null ? u.UserFullName : null,
                                               GroupName = g != null ? g.Name : null
                                           }).FirstOrDefaultAsync();

                return (createdEntity, "Created successfully");
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

                // Fetch the updated entity with joined data
                var updatedEntity = await (from ug in _context.UserGroups
                                           join u in _context.Users on ug.UserId equals u.GuidId into userJoin
                                           from u in userJoin.DefaultIfEmpty()
                                           join g in _context.Groups on ug.GroupId equals g.GuidId into groupJoin
                                           from g in groupJoin.DefaultIfEmpty()
                                           where ug.GuidId == id
                                           select new UserGroupModel
                                           {
                                               GuidId = ug.GuidId,
                                               UserId = ug.UserId,
                                               GroupId = ug.GroupId,
                                               CreatedByUserId = ug.CreatedByUserId,
                                               CreationDate = ug.CreationDate,
                                               LastUpdatedByUserId = ug.LastUpdatedByUserId,
                                               LastUpdatedDate = ug.LastUpdatedDate,
                                               UserName = u != null ? u.UserFullName : null,
                                               GroupName = g != null ? g.Name : null
                                           }).FirstOrDefaultAsync();

                return updatedEntity;
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
