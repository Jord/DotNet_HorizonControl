using AutoMapper;
using HorizonControlCenterDAL.Entities;
using HorizonControlCenterDAL.Interfaces;
using HorizonControlCenterModels;
using HorizonControlCenterModels.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HorizonControlCenterDAL
{
    public class GroupGroupDAL : IGroupGroupDAL
    {
        private readonly horizoncontrolContext _context;
        private readonly IMapper _mapper;
        private readonly string _className;

        public GroupGroupDAL(horizoncontrolContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _className = GetType().Name;
        }

        public async Task<List<GroupGroupModel>> GetAllAsync()
        {
            const string methodName = nameof(GetAllAsync);
            try
            {
                var data = await (from gg in _context.GroupGroups
                                  join g1 in _context.Groups on gg.GroupId equals g1.GuidId into group1Join
                                  from g1 in group1Join.DefaultIfEmpty()
                                  join g2 in _context.Groups on gg.MapToGroupId equals g2.GuidId into group2Join
                                  from g2 in group2Join.DefaultIfEmpty()
                                  select new GroupGroupModel
                                  {
                                      GuidId = gg.GuidId,
                                      GroupId = gg.GroupId,
                                      MapToGroupId = gg.MapToGroupId,
                                      CreatedByUserId = gg.CreatedByUserId,
                                      CreationDate = gg.CreationDate,
                                      LastUpdatedByUserId = gg.LastUpdatedByUserId,
                                      LastUpdatedDate = gg.LastUpdatedDate,
                                      GroupName = g1 != null ? g1.Name : null,
                                      MapToGroupName = g2 != null ? g2.Name : null
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

        public async Task<GroupGroupModel?> GetByIdAsync(Guid id)
        {
            const string methodName = nameof(GetByIdAsync);
            try
            {
                var entity = await (from gg in _context.GroupGroups
                                    join g1 in _context.Groups on gg.GroupId equals g1.GuidId into group1Join
                                    from g1 in group1Join.DefaultIfEmpty()
                                    join g2 in _context.Groups on gg.MapToGroupId equals g2.GuidId into group2Join
                                    from g2 in group2Join.DefaultIfEmpty()
                                    where gg.GuidId == id
                                    select new GroupGroupModel
                                    {
                                        GuidId = gg.GuidId,
                                        GroupId = gg.GroupId,
                                        MapToGroupId = gg.MapToGroupId,
                                        CreatedByUserId = gg.CreatedByUserId,
                                        CreationDate = gg.CreationDate,
                                        LastUpdatedByUserId = gg.LastUpdatedByUserId,
                                        LastUpdatedDate = gg.LastUpdatedDate,
                                        GroupName = g1 != null ? g1.Name : null,
                                        MapToGroupName = g2 != null ? g2.Name : null
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

        public async Task<GroupGroupModel?> GetExistingGroupGroupAsync(GroupGroupDTO model)
        {
            const string methodName = nameof(GetExistingGroupGroupAsync);
            try
            {
                var existingEntity = await (from gg in _context.GroupGroups
                                             join g1 in _context.Groups on gg.GroupId equals g1.GuidId into group1Join
                                             from g1 in group1Join.DefaultIfEmpty()
                                             join g2 in _context.Groups on gg.MapToGroupId equals g2.GuidId into group2Join
                                             from g2 in group2Join.DefaultIfEmpty()
                                             where gg.GroupId == model.GroupId && gg.MapToGroupId == model.MapToGroupId
                                             select new GroupGroupModel
                                             {
                                                 GuidId = gg.GuidId,
                                                 GroupId = gg.GroupId,
                                                 MapToGroupId = gg.MapToGroupId,
                                                 CreatedByUserId = gg.CreatedByUserId,
                                                 CreationDate = gg.CreationDate,
                                                 LastUpdatedByUserId = gg.LastUpdatedByUserId,
                                                 LastUpdatedDate = gg.LastUpdatedDate,
                                                 GroupName = g1 != null ? g1.Name : null,
                                                 MapToGroupName = g2 != null ? g2.Name : null
                                             }).AsNoTracking().FirstOrDefaultAsync();

                if (existingEntity != null)
                {
                    Log.ForContext("classname", _className)
                       .ForContext("method_name", methodName)
                       .Information("Existing GroupGroup found at {Time}", DateTime.Now);
                }

                return existingEntity;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred in {MethodName}", methodName);
                throw;
            }
        }

        public async Task<(GroupGroupModel result, string message)> CreateAsync(GroupGroupDTO model, int userId)
        {
            const string methodName = nameof(CreateAsync);

            try
            {
                var exists = await _context.GroupGroups.AnyAsync(s => s.GroupId == model.GroupId && s.MapToGroupId == model.MapToGroupId);

                if (exists)
                {
                    string message = "GroupGroup mapping already exists";

                    Log.ForContext("classname", _className)
                       .ForContext("method_name", methodName)
                       .ForContext("user_id", userId)
                       .Warning("Duplicate detected: {Message}", message);

                    return (null, message);
                }

                var entity = _mapper.Map<GroupGroup>(model);

                entity.CreationDate = DateTime.UtcNow;
                entity.LastUpdatedDate = DateTime.UtcNow;
                entity.CreatedByUserId = userId;
                entity.LastUpdatedByUserId = userId;

                await _context.GroupGroups.AddAsync(entity);
                await _context.SaveChangesAsync();

                // Fetch the created entity with joined data
                var createdEntity = await (from gg in _context.GroupGroups
                                           join g1 in _context.Groups on gg.GroupId equals g1.GuidId into group1Join
                                           from g1 in group1Join.DefaultIfEmpty()
                                           join g2 in _context.Groups on gg.MapToGroupId equals g2.GuidId into group2Join
                                           from g2 in group2Join.DefaultIfEmpty()
                                           where gg.GuidId == entity.GuidId
                                           select new GroupGroupModel
                                           {
                                               GuidId = gg.GuidId,
                                               GroupId = gg.GroupId,
                                               MapToGroupId = gg.MapToGroupId,
                                               CreatedByUserId = gg.CreatedByUserId,
                                               CreationDate = gg.CreationDate,
                                               LastUpdatedByUserId = gg.LastUpdatedByUserId,
                                               LastUpdatedDate = gg.LastUpdatedDate,
                                               GroupName = g1 != null ? g1.Name : null,
                                               MapToGroupName = g2 != null ? g2.Name : null
                                           }).FirstOrDefaultAsync();

                return (createdEntity, "Created successfully");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in {MethodName}", methodName);
                throw;
            }
        }

        public async Task<GroupGroupModel> UpdateAsync(Guid id, GroupGroupDTO model, int userId)
        {
            const string methodName = nameof(UpdateAsync);
            try
            {
                var entity = await _context.GroupGroups
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
                var updatedEntity = await (from gg in _context.GroupGroups
                                           join g1 in _context.Groups on gg.GroupId equals g1.GuidId into group1Join
                                           from g1 in group1Join.DefaultIfEmpty()
                                           join g2 in _context.Groups on gg.MapToGroupId equals g2.GuidId into group2Join
                                           from g2 in group2Join.DefaultIfEmpty()
                                           where gg.GuidId == id
                                           select new GroupGroupModel
                                           {
                                               GuidId = gg.GuidId,
                                               GroupId = gg.GroupId,
                                               MapToGroupId = gg.MapToGroupId,
                                               CreatedByUserId = gg.CreatedByUserId,
                                               CreationDate = gg.CreationDate,
                                               LastUpdatedByUserId = gg.LastUpdatedByUserId,
                                               LastUpdatedDate = gg.LastUpdatedDate,
                                               GroupName = g1 != null ? g1.Name : null,
                                               MapToGroupName = g2 != null ? g2.Name : null
                                           }).FirstOrDefaultAsync();

                return updatedEntity;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in {MethodName}", methodName);
                throw;
            }
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            const string methodName = nameof(DeleteAsync);

            try
            {
                var entity = await _context.GroupGroups
                    .Where(j => j.GuidId == id)
                    .FirstOrDefaultAsync();

                if (entity == null)
                {
                    return 0; // Not found
                }

                _context.GroupGroups.Remove(entity);
                await _context.SaveChangesAsync();

                Log.ForContext("classname", _className)
                   .ForContext("method_name", methodName)
                   .ForContext("thrown_by", "Application")
                   .Information("Deleted record with GuidId {Id} at {Time}", id, DateTime.Now);

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
                var entity = await _context.GroupGroups
                    .Where(j => j.GuidId == id)
                    .FirstOrDefaultAsync();

                if (entity == null)
                {
                    return 0; // Not found
                }

                _context.GroupGroups.Remove(entity);
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
