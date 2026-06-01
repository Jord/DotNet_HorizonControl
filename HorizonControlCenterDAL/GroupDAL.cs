using AutoMapper;
using HorizonControlCenterDAL.Entities;
using HorizonControlCenterModels;
using HorizonControlCenterModels.Custom;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HorizonControlCenterModels.DTO;

namespace HorizonControlCenterDAL
{
    public class GroupDAL
    {
        private readonly horizoncontrolContext _context;
        private readonly IMapper _mapper;
        private readonly string _className;
        public GroupDAL(IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _context = new horizoncontrolContext(AppConfiguration.ngsqlConnectionOptions());
            _mapper = mapper;
            _className = GetType().Name;
        }

        public async Task<List<GroupWithCategoryViewModel>> GetgrouprecordsAsync()
        {
            // List<GroupWithCategoryViewModel>? records = null;
            try
            {
                Log.ForContext("class_name", GetType().Name).ForContext("method_name", "GetgrouprecordsAsync").ForContext("thrown_by", "Application")
               .Information("Execution of GetgrouprecordsAsync started: {TimeofDay}", DateTime.Now.TimeOfDay);

                var groupRecords = await _context.GroupWithCategoryViews.AsNoTracking().ToListAsync();
                return _mapper.Map<List<GroupWithCategoryViewModel>>(groupRecords);

            }
            catch (Exception)
            {
                Log.ForContext("class_name", GetType().Name).ForContext("method_name", "GetgrouprecordsAsync").ForContext("thrown_by", "Application")
              .Error("Error Occured in method  GetgrouprecordsAsync : {TimeofDay}", DateTime.Now.TimeOfDay);
                throw;//new Exception(ex.Message);
            }

        }

        public async Task<GroupWithCategoryViewModel> GetgrouprecordsByIdAsync(Guid id)
        {

            try
            {
                Log.ForContext("class_name", GetType().Name)
                 .ForContext("method_name", "GetgrouprecordsByIdAsync")
                 .ForContext("thrown_by", "Application")
                 .Information("Execution of GetgrouprecordsByIdAsync started at {TimeofDay}", DateTime.Now.TimeOfDay);

                var groupRecords = await _context.GroupWithCategoryViews.AsNoTracking().Where(x => x.GuidId == id).FirstOrDefaultAsync();

                return groupRecords == null
                    ? new GroupWithCategoryViewModel()
                    : _mapper.Map<GroupWithCategoryViewModel>(groupRecords);
            }
            catch (Exception)
            {
                Log.ForContext("class_name", GetType().Name)
                    .ForContext("method_name", "GetgrouprecordsByIdAsync")
                    .ForContext("thrown_by", "Application")
                    .Error("Error occurred while fetching  group record by ID at {TimeofDay}", DateTime.Now.TimeOfDay);
                throw;
            }

        }

        public async Task<List<GroupWithCategoryViewModel>> GetGroupRecordsByGroupTypeAsync(string groupType)
        {

            try
            {
                Log.ForContext("class_name", GetType().Name)
                 .ForContext("method_name", "GetGroupRecordsByGroupTypeAsync")
                 .ForContext("thrown_by", "Application")
                 .Information("Execution of GetGroupRecordsByGroupTypeAsync started at {TimeofDay}", DateTime.Now.TimeOfDay);

                var groupRecords = await _context.GroupWithCategoryViews.AsNoTracking().Where(x => x.GroupType == groupType).ToListAsync();

                return _mapper.Map<List<GroupWithCategoryViewModel>>(groupRecords);
            }
            catch (Exception)
            {
                Log.ForContext("class_name", GetType().Name)
                    .ForContext("method_name", "GetGroupRecordsByGroupTypeAsync")
                    .ForContext("thrown_by", "Application")
                    .Error("Error occurred while fetching  group record by ID at {TimeofDay}", DateTime.Now.TimeOfDay);
                throw;
            }

        }

        public async Task<List<UserGroupSummaryViewModelDTO>> GetUserGroupRecordsByGroupNamesAsync(List<string> groupNames)
        {
            try
            {
                Log.ForContext("class_name", GetType().Name)
                   .ForContext("method_name", "GetUserGroupRecordsByGroupNamesAsync")
                   .ForContext("thrown_by", "Application")
                 .Information("Execution of GetGroupRecordsByGroupTypeAsync started at {TimeofDay}", DateTime.Now.TimeOfDay);

                //Fetch all records for given group names
                var groupRecords = await _context.UserGroupViews.AsNoTracking()
                    .Where(x => groupNames.Contains(x.MapGroupName)).ToListAsync();

                //Direct Users (mapping_type = "user")
                var directUsers = groupRecords
                    .Where(x => x.MappingType.ToLower() == "user" && x.UserId.HasValue)
                    .Select(x => x).ToList();

                //Groups mapped to given group names
                var mappedGroupIds = groupRecords
                    .Where(x => x.MappingType.ToLower() == "group" && x.GroupId.HasValue)
                    .Select(x => x.GroupId!.Value).Distinct().ToList();

                //Indirect Users (users inside mapped groups)
                var indirectUsers = await _context.UserGroupViews
                    .AsNoTracking()
                    .Where(x => mappedGroupIds.Contains(x.MapToGroupId ?? Guid.Empty) && x.MappingType.ToLower() == "user" &&
                        x.UserId.HasValue).ToListAsync();

                //Combine + Remove duplicate users
                var finalRecords = directUsers.Concat(indirectUsers).GroupBy(x => x.UserId).Select(g => g.First()).ToList();

                return _mapper.Map<List<UserGroupSummaryViewModelDTO>>(finalRecords);
            }
            catch (Exception)
            {
                Log.ForContext("class_name", GetType().Name)
                    .ForContext("method_name", "GetUserGroupRecordsByGroupNamesAsync")
                    .Error("Error occurred while fetching records");

                throw;
            }
        }

        public async Task<List<GroupSummaryDTO>> GetGroupNamesByUserIdAsync(Guid userId)
        {
            try
            {
                Log.ForContext("class_name", GetType().Name)
                   .ForContext("method_name", "GetGroupNamesByUserIdAsync")
                   .ForContext("thrown_by", "Application")
                   .Information("Execution started at {TimeofDay}", DateTime.Now.TimeOfDay);

                // Fetch all mappings for this user
                var userMappings = await _context.UserGroupViews.AsNoTracking()
                    .Where(x => x.UserId == userId).ToListAsync();

                //Direct Groups (user → group)
                var directGroupNames = userMappings
                    .Where(x => x.MappingType.ToLower() == "user" && !string.IsNullOrEmpty(x.MapGroupName))
                     .Select(x => new GroupSummaryDTO { GuidId = x.GuidId, GroupName = x.MapGroupName!, GroupType = x.GroupType! })
                    .ToList();

                //Indirect Groups (user → group → parent group)
                var mappedGroupIds = userMappings.Where(x => x.MappingType.ToLower() == "user" && x.GroupId.HasValue)
                    .Select(x => x.GroupId!.Value).Distinct().ToList();

                var indirectGroupNames = await _context.UserGroupViews
                    .AsNoTracking().Where(x => mappedGroupIds.Contains(x.MapToGroupId ?? Guid.Empty) &&
                        x.MappingType.ToLower() == "group" && !string.IsNullOrEmpty(x.MapGroupName))
                     .Select(x => new GroupSummaryDTO { GuidId = x.GuidId, GroupName = x.MapGroupName!, GroupType = x.GroupType! })
                    .ToListAsync();

                // Combine + distinct
                return directGroupNames.Concat(indirectGroupNames).GroupBy(g => g.GroupName).Select(g => g.First()).ToList();
            }
            catch (Exception)
            {
                Log.ForContext("class_name", GetType().Name)
                   .ForContext("method_name", "GetGroupNamesByUserIdAsync")
                   .Error("Error occurred while fetching group names by userId");

                throw;
            }
        }

        public async Task<GroupModel> CreategroupAsync(GroupModel groupModel)
        {
            try
            {
                Log.ForContext("class_name", GetType().Name)
                   .ForContext("method_name", "CreategroupAsync")
                   .ForContext("thrown_by", "Application")
                   .Information("Execution of method CreategroupAsync started at {TimeofDay}", DateTime.Now.TimeOfDay);

                groupModel.CreationDate = DateTime.UtcNow;
                groupModel.LastUpdatedDate = DateTime.UtcNow;
                // _context.ApplicationAccessGroupInfos.Add(_mapper.Map<ApplicationAccessGroupInfo>(aagi));

                var entity = _mapper.Map<Group>(groupModel);
                if (entity != null)
                {
                    await _context.Groups.AddAsync(entity);
                    await _context.SaveChangesAsync();
                }
                //return aagi;
                //await _context.SaveChangesAsync();
                groupModel.GuidId = entity.GuidId;
            }
            catch (Exception) { throw; }
            return groupModel;
        }

        public async Task<GroupModel> UpdategroupAsync(GroupModel groupModel)
        {
            try
            {
                groupModel.LastUpdatedDate = DateTime.UtcNow;
                Log.ForContext("class_name", GetType().Name).ForContext("method_name", "UpdategroupAsync").ForContext("thrown_by", "Application")
                .Information("Execution of  method  UpdategroupAsync started: {TimeofDay}", DateTime.Now.TimeOfDay);
                _context.Entry(_mapper.Map<Group>(groupModel)).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex) { throw; }
            return groupModel;
        }

        public async Task<ActionResponseModel> DeletegroupAsync(Guid id)
        {
            try
            {
                Log.ForContext("class_name", GetType().Name).ForContext("method_name", "DeletegroupAsync").ForContext("thrown_by", "Application")
                .Information("Execution of  method  DeletegroupAsync : {TimeofDay}", DateTime.Now.TimeOfDay);
                var records = await _context.Groups.FindAsync(id);
                if (records == null)
                {
                    return new ActionResponseModel("FAILED", $"Group not found. with id:{id}", null, 0, null);
                }

                var mappingRecods = await _context.UserGroupViews.Where(x => x.MapToGroupId == id).ToListAsync();
                if (mappingRecods.Count > 0)
                {
                    return new ActionResponseModel("FAILED", $"Group cannot be deleted because it is linked to a user/group. with id:{id}", null, 0, null);
                }

                _context.Groups.Remove(records);
                await _context.SaveChangesAsync();

                return new ActionResponseModel("SUCCESS", $"Group deleted successfully. with id:{id}", null, 0, records);

            }
            catch (Exception ex) { throw; }

        }
    }
}
