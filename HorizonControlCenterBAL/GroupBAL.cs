using AutoMapper;
using HorizonControlCenterBAL.Interfaces;
using HorizonControlCenterDAL.Interfaces;
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
    public class GroupBAL : IGroupBAL
    {
        private readonly IGroupDAL _groupDAL;
        public GroupBAL(IGroupDAL groupDAL)
        {
            _groupDAL = groupDAL;

        }

        //public async Task<List<GroupWithCategoryViewModel>> GetgrouprecordsAsync()
        //{
        //    try
        //    {
        //        Log.ForContext("class_name", GetType().Name).ForContext("method_name", "GetgrouprecordsAsync").ForContext("thrown_by", "Application")
        //      .Information("Execution of GetgrouprecordsAsync started: {TimeofDay}", DateTime.Now.TimeOfDay);
        //        return await _groupDAL.GetgrouprecordsAsync();
        //    }
        //    catch (Exception)
        //    {
        //        throw; //new Exception("An error occurred while fetching access group records.", ex);
        //    }
        //}

        //public async Task<GroupWithCategoryViewModel> GetgrouprecordsByIdAsync(Guid id)
        //{
        //    try
        //    {
        //        Log.ForContext("class_name", GetType().Name).ForContext("method_name", "GetgrouprecordsByIdAsync").ForContext("thrown_by", "Application")
        //         .Information("Execution of GetgrouprecordsByIdAsync started: {TimeofDay}", DateTime.Now.TimeOfDay);
        //        return await _groupDAL.GetgrouprecordsByIdAsync(id);
        //    }

        //    catch (Exception)
        //    {
        //        throw;
        //    }

        //}
        //public async Task<List<GroupWithCategoryViewModel>> GetGroupRecordsByGroupTypeAsync(string groupType)
        //{
        //    try
        //    {
        //        Log.ForContext("class_name", GetType().Name).ForContext("method_name", "GetGroupRecordsByGroupTypeAsync").ForContext("thrown_by", "Application")
        //         .Information("Execution of GetGroupRecordsByGroupTypeAsync started: {TimeofDay}", DateTime.Now.TimeOfDay);
        //        return await _groupDAL.GetGroupRecordsByGroupTypeAsync(groupType);
        //    }

        //    catch (Exception)
        //    {
        //        throw;
        //    }

        //}

        //public async Task<List<UserGroupSummaryViewModelDTO>> GetUserGroupRecordsByGroupNamesAsync(List<string> groupNames)
        //{
        //    try
        //    {
        //        Log.ForContext("class_name", GetType().Name).ForContext("method_name", "GetUserGroupRecordsByGroupNamesAsync")
        //           .Information("Execution of GetGroupRecordsByGroupTypeAsync started: {TimeofDay}", DateTime.Now.TimeOfDay);

        //        return await _groupDAL.GetUserGroupRecordsByGroupNamesAsync(groupNames);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        //public async Task<List<GroupSummaryDTO>> GetGroupNamesByUserIdAsync(Guid userId)
        //{
        //    try
        //    {
        //        Log.ForContext("class_name", GetType().Name)
        //           .ForContext("method_name", "GetGroupNamesByUserIdAsync")
        //           .Information("Execution started at {TimeofDay}", DateTime.Now.TimeOfDay);

        //        return await _groupDAL.GetGroupNamesByUserIdAsync(userId);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        public async Task<List<GroupModel>> GetAllAsync()
        {
            const string methodName = nameof(GetAllAsync);
            try
            {
                Log.ForContext("class_name", GetType().Name)
                   .ForContext("method_name", methodName)
                   .ForContext("thrown_by", "Application")
                   .Information("Execution of GetAllAsync started: {TimeofDay}", DateTime.Now.TimeOfDay);

                return await _groupDAL.GetAllAsync();
            }
            catch (Exception ex)
            {
                Log.ForContext("class_name", GetType().Name)
                   .ForContext("method_name", methodName)
                   .Error(ex, "Error occurred in {MethodName}", methodName);
                throw;
            }
        }

        public async Task<GroupModel?> GetByIdAsync(Guid id)
        {
            const string methodName = nameof(GetByIdAsync);
            try
            {
                Log.ForContext("class_name", GetType().Name)
                   .ForContext("method_name", methodName)
                   .ForContext("thrown_by", "Application")
                   .Information("Execution of GetByIdAsync started: {TimeofDay}", DateTime.Now.TimeOfDay);

                return await _groupDAL.GetGroupByIdAsync(id);
            }
            catch (Exception ex)
            {
                Log.ForContext("class_name", GetType().Name)
                   .ForContext("method_name", methodName)
                   .Error(ex, "Error occurred in {MethodName}", methodName);
                throw;
            }
        }

        public async Task<GlobalResponseModel<GroupModel>> CreategroupAsync(GroupDTO groupDto, int userId)
        {
            const string methodName = nameof(CreategroupAsync);
            try
            {
                Log.ForContext("class_name", GetType().Name)
                   .ForContext("method_name", methodName)
                   .Information("Execution of CreategroupAsync started: {TimeofDay}", DateTime.Now.TimeOfDay);

                GroupModel? existing = await _groupDAL.GetExistingGroupAsync(groupDto);
                if (existing != null)
                {
                    return Custom.CreateError<GroupModel>("409", "Group - create - record already exists with the same Name and GroupType", obj: existing, objectId: existing.GuidId);
                }

                var (result, message) = await _groupDAL.CreategroupAsync(groupDto, userId);
                if (result == null)
                {
                    return Custom.CreateError<GroupModel>("500", "Group - create - no records were created", obj: null);
                }

                return Custom.CreateSuccess<GroupModel>(objectId: result.GuidId, obj: result, message: "Group - create - successful");
            }
            catch (Exception ex)
            {
                Log.ForContext("class_name", GetType().Name)
                   .ForContext("method_name", methodName)
                   .Error(ex, "Error occurred in {MethodName}", methodName);
                throw;
            }
        }

        public async Task<GlobalResponseModel<GroupModel>> UpdategroupAsync(Guid id, GroupDTO groupDto, int userId)
        {
            const string methodName = nameof(UpdategroupAsync);
            try
            {
                Log.ForContext("class_name", GetType().Name)
                   .ForContext("method_name", methodName)
                   .ForContext("thrown_by", "Application")
                   .Information("Execution of UpdategroupAsync started: {TimeofDay}", DateTime.Now.TimeOfDay);

                var existing = await _groupDAL.GetGroupByIdAsync(id);
                if (existing == null)
                {
                    return Custom.CreateError<GroupModel>("404", "Group - update - record not found", obj: null);
                }

                var result = await _groupDAL.UpdategroupAsync(id, groupDto, userId);
                if (result == null)
                {
                    return Custom.CreateError<GroupModel>("500", "Group - update - failed to update record", obj: null);
                }

                return Custom.CreateSuccess<GroupModel>(objectId: result.GuidId, obj: result, message: "Group - update - successful");
            }
            catch (Exception ex)
            {
                Log.ForContext("class_name", GetType().Name)
                   .ForContext("method_name", methodName)
                   .Error(ex, "Error occurred in {MethodName}", methodName);
                throw;
            }
        }

        public async Task<GlobalResponseModel<GroupModel>> DeletegroupAsync(Guid id, int userId)
        {
            const string methodName = nameof(DeletegroupAsync);
            try
            {
                Log.ForContext("class_name", GetType().Name)
                   .ForContext("method_name", methodName)
                   .ForContext("thrown_by", "Application")
                   .Information("Execution of DeletegroupAsync started: {TimeofDay}", DateTime.Now.TimeOfDay);

                var existing = await _groupDAL.GetGroupByIdAsync(id);
                if (existing == null)
                {
                    return Custom.CreateError<GroupModel>("404", "Group - delete - record not found", obj: null);
                }

                var actionResponse = await _groupDAL.DeletegroupAsync(id, userId);
                if (actionResponse.Status == "FAILED")
                {
                    return Custom.CreateError<GroupModel>("500", actionResponse.ReturnMessage ?? "Failed to delete group", obj: null);
                }

                return Custom.CreateSuccess<GroupModel>(objectId: id, obj: existing, message: "Group - delete - successful");
            }
            catch (Exception ex)
            {
                Log.ForContext("class_name", GetType().Name)
                   .ForContext("method_name", methodName)
                   .Error(ex, "Error occurred in {MethodName}", methodName);
                throw;
            }
        }

        public async Task<GlobalResponseModel<GroupModel>> PermanentDeletegroupAsync(Guid id)
        {
            const string methodName = nameof(PermanentDeletegroupAsync);
            try
            {
                Log.ForContext("class_name", GetType().Name)
                   .ForContext("method_name", methodName)
                   .ForContext("thrown_by", "Application")
                   .Information("Execution of PermanentDeletegroupAsync started: {TimeofDay}", DateTime.Now.TimeOfDay);

                var existing = await _groupDAL.GetGroupByIdAsync(id);
                if (existing == null)
                {
                    return Custom.CreateError<GroupModel>("404", "Group - permanent delete - record not found", obj: null);
                }

                var actionResponse = await _groupDAL.PermanentDeletegroupAsync(id);
                if (actionResponse.Status == "FAILED")
                {
                    return Custom.CreateError<GroupModel>("500", actionResponse.ReturnMessage ?? "Failed to permanently delete group", obj: null);
                }

                return Custom.CreateSuccess<GroupModel>(objectId: id, obj: existing, message: "Group - permanent delete - successful");
            }
            catch (Exception ex)
            {
                Log.ForContext("class_name", GetType().Name)
                   .ForContext("method_name", methodName)
                   .Error(ex, "Error occurred in {MethodName}", methodName);
                throw;
            }
        }
    }
}
