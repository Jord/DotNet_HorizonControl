using AutoMapper;
using HorizonControlCenterDAL;
using HorizonControlCenterModels;
using HorizonControlCenterModels.Custom;
using HorizonControlCenterModels.DTO;
using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorizonControlCenterBAL
{
    public class GroupBAL
    {
        private readonly GroupDAL _groupDAL;
        public GroupBAL(IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _groupDAL = new GroupDAL(httpContextAccessor, mapper);

        }

        public async Task<List<GroupWithCategoryViewModel>> GetgrouprecordsAsync()
        {
            try
            {
                Log.ForContext("class_name", GetType().Name).ForContext("method_name", "GetgrouprecordsAsync").ForContext("thrown_by", "Application")
              .Information("Execution of GetgrouprecordsAsync started: {TimeofDay}", DateTime.Now.TimeOfDay);
                return await _groupDAL.GetgrouprecordsAsync();
            }
            catch (Exception)
            {
                throw; //new Exception("An error occurred while fetching access group records.", ex);
            }
        }

        public async Task<GroupWithCategoryViewModel> GetgrouprecordsByIdAsync(Guid id)
        {
            try
            {
                Log.ForContext("class_name", GetType().Name).ForContext("method_name", "GetgrouprecordsByIdAsync").ForContext("thrown_by", "Application")
                 .Information("Execution of GetgrouprecordsByIdAsync started: {TimeofDay}", DateTime.Now.TimeOfDay);
                return await _groupDAL.GetgrouprecordsByIdAsync(id);
            }

            catch (Exception)
            {
                throw;
            }

        }
        public async Task<List<GroupWithCategoryViewModel>> GetGroupRecordsByGroupTypeAsync(string groupType)
        {
            try
            {
                Log.ForContext("class_name", GetType().Name).ForContext("method_name", "GetGroupRecordsByGroupTypeAsync").ForContext("thrown_by", "Application")
                 .Information("Execution of GetGroupRecordsByGroupTypeAsync started: {TimeofDay}", DateTime.Now.TimeOfDay);
                return await _groupDAL.GetGroupRecordsByGroupTypeAsync(groupType);
            }

            catch (Exception)
            {
                throw;
            }

        }

        public async Task<List<UserGroupSummaryViewModelDTO>> GetUserGroupRecordsByGroupNamesAsync(List<string> groupNames)
        {
            try
            {
                Log.ForContext("class_name", GetType().Name).ForContext("method_name", "GetUserGroupRecordsByGroupNamesAsync")
                   .Information("Execution of GetGroupRecordsByGroupTypeAsync started: {TimeofDay}", DateTime.Now.TimeOfDay);

                return await _groupDAL.GetUserGroupRecordsByGroupNamesAsync(groupNames);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<GroupSummaryDTO>> GetGroupNamesByUserIdAsync(Guid userId)
        {
            try
            {
                Log.ForContext("class_name", GetType().Name)
                   .ForContext("method_name", "GetGroupNamesByUserIdAsync")
                   .Information("Execution started at {TimeofDay}", DateTime.Now.TimeOfDay);

                return await _groupDAL.GetGroupNamesByUserIdAsync(userId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<GroupModel> CreategroupAsync(GroupModel groupModel)
        {
            try
            {
                Log.ForContext("class_name", GetType().Name).ForContext("method_name", "CreategroupAsync").ForContext("thrown_by", "Application")
               .Information("Execution of CreategroupAsync started: {TimeofDay}", DateTime.Now.TimeOfDay);
                await _groupDAL.CreategroupAsync(groupModel);
                return groupModel;
            }
            catch (Exception)
            {
                throw; //new Exception("Error while creating access group", ex);
            }
        }

        public async Task<GroupModel> UpdategroupAsync(GroupModel groupModel)
        {
            try
            {
                Log.ForContext("class_name", GetType().Name).ForContext("method_name", "UpdategroupAsync").ForContext("thrown_by", "Application")
                .Information("Execution of UpdategroupAsync started: {TimeofDay}", DateTime.Now.TimeOfDay);
                await _groupDAL.UpdategroupAsync(groupModel);
                return groupModel;
            }
            catch (Exception) { throw; } //new Exception("Error while updating  group", ex); }
        }

        public async Task<ActionResponseModel> DeletegroupAsync(Guid id)
        {
            try
            {
                Log.ForContext("class_name", GetType().Name).ForContext("method_name", "DeletegroupAsync").ForContext("thrown_by", "Application")
                 .Information("Execution of DeletegroupAsync started: {TimeofDay}", DateTime.Now.TimeOfDay);
                return await _groupDAL.DeletegroupAsync(id);
                //return 0;
            }
            catch (Exception)
            {
                throw;// new Exception($"An error occurred while deleting access group with ID {id}.", ex);
            }
        }
    }
}
