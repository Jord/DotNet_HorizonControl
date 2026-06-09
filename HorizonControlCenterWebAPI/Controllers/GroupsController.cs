using AutoMapper;
using HorizonControlCenterBAL;
using HorizonControlCenterBAL.Interfaces;
using HorizonControlCenterModels;
using HorizonControlCenterModels.Custom;
using HorizonControlCenterModels.DTO;
using HorizonControlCenterWebAPI.Security;
using HorizonControlCenterWebAPI.Services.UserService;
using Microsoft.AspNetCore.Mvc;
using SecurityAuthorization;
using Serilog;
using Swashbuckle.AspNetCore.Annotations;

namespace HorizonControlCenterWebAPI.Controllers
{
    [Authorize]
    [Route("api/groups")]
    [ApiController]
    [Tags("Groups")]
    public class GroupsController : Controller
    {
        private readonly IGroupBAL _groupBAL;
        private readonly IUserService _userService;
        private AuthUser _authUser;
        private string _controllerName = "";
        private string? _methodName = "";

        /// <summary>
        /// </summary>
        public GroupsController(IGroupBAL groupBAL, IUserService userService)
        {
            _groupBAL = groupBAL;
            _userService = userService;
            _authUser = userService.GetUserInfo();
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Get all groups",
            Description = "Retrieves a list of all group records from the system."
        )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GroupModel>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<GroupModel>>> GetAsync()
        {
            try
            {
                _controllerName = ControllerContext.ActionDescriptor.ControllerName;
                _methodName = ControllerContext?.RouteData?.Values["action"]?.ToString();
                Log.ForContext("method_name", _methodName)
                   .ForContext("controller_name", _controllerName)
                   .ForContext("thrown_by", "Application")
                   .Information("Run EndPoint Of Get Method: {TimeofDay}", DateTime.Now.TimeOfDay);

                return await _groupBAL.GetAllAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in GetAsync");
                throw;
            }
        }

        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Get a group by ID",
            Description = "Retrieves a specific group record identified by its GUID."
        )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GroupModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GroupModel>> GetByIdAsync(Guid id)
        {
            try
            {
                var result = await _groupBAL.GetByIdAsync(id);
                if (result == null)
                    return NotFound();

                _controllerName = ControllerContext.ActionDescriptor.ControllerName;
                _methodName = ControllerContext?.RouteData?.Values["action"]?.ToString();
                Log.ForContext("method_name", _methodName)
                   .ForContext("controller_name", _controllerName)
                   .ForContext("thrown_by", "Application")
                   .Information("Run EndPoint Of GetById Method: {TimeofDay}", DateTime.Now.TimeOfDay);
                return result;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in GetByIdAsync");
                throw;
            }
        }

        ///// <summary>
        ///// Fetch all group records.
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet]
        //public async Task<ActionResult<List<GroupWithCategoryViewModel>>> Get()
        //{
        //    List<GroupWithCategoryViewModel>? records = null;

        //    try
        //    {
        //        records = await _groupBAL.GetgrouprecordsAsync();
        //        _controllerName = ControllerContext.ActionDescriptor.ControllerName;
        //        _methodName = ControllerContext?.RouteData?.Values["action"]?.ToString();
        //        Log.ForContext("method_name", _methodName)
        //           .ForContext("controller_name", _controllerName)
        //           .ForContext("thrown_by", "Application")
        //           .Information("Run EndPoint Of Getappaccessgrouprecords Method: {TimeofDay}", DateTime.Now.TimeOfDay);
        //        return records;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        ///// <summary>
        /////Fetch a specific group record by ID.
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //[HttpGet("{id}")]
        //public async Task<ActionResult<List<GroupWithCategoryViewModel>>> GetById(Guid id)
        //{
        //    GroupWithCategoryViewModel? record = null;
        //    try
        //    {
        //        if (id <= Guid.Empty)
        //        {
        //            return BadRequest();
        //        }
        //        else
        //        {
        //            record = await _groupBAL.GetgrouprecordsByIdAsync(id);
        //            if (record == null)
        //            {
        //                return NotFound(new { message = "Record not found." });
        //            }
        //            _controllerName = ControllerContext.ActionDescriptor.ControllerName;
        //            _methodName = ControllerContext?.RouteData?.Values["action"]?.ToString();
        //            Log.ForContext("method_name", _methodName)
        //               .ForContext("controller_name", _controllerName)
        //               .ForContext("thrown_by", "Application")
        //               .Information("Run EndPoint Of GetappaccessgrouprecordsById Method: {TimeofDay}", DateTime.Now.TimeOfDay);
        //            if (record.Guid_Id == null)
        //            {
        //                return NotFound("Record Not Found");
        //            }
        //            return Ok(record);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        ///// <summary>
        ///// Fetch a specific group record by GroupType.
        ///// </summary>
        ///// <param name="groupType"></param>
        ///// <returns></returns>
        //[HttpGet("by-grouptype/{groupType}")]
        //public async Task<ActionResult<List<GroupWithCategoryViewModel>>> GetByGroupType(string groupType)
        //{
        //    List<GroupWithCategoryViewModel>? records = null;

        //    try
        //    {
        //        if (string.IsNullOrWhiteSpace(groupType))
        //        {
        //            return BadRequest(new { message = "GroupType is required." });
        //        }

        //        records = await _groupBAL.GetGroupRecordsByGroupTypeAsync(groupType);

        //        _controllerName = ControllerContext.ActionDescriptor.ControllerName;
        //        _methodName = ControllerContext?.RouteData?.Values["action"]?.ToString();

        //        Log.ForContext("method_name", _methodName)
        //           .ForContext("controller_name", _controllerName)
        //           .ForContext("thrown_by", "Application")
        //           .Information("Run EndPoint Of GetGroupRecordsByGroupType Method: {TimeofDay}", DateTime.Now.TimeOfDay);

        //        return Ok(records);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        ///// <summary>
        /////  Fetch user-group records by multiple group names
        ///// </summary>
        ///// <param name="groupNames"></param>
        ///// <returns></returns>
        //[HttpGet("by-group-names")]
        //public async Task<ActionResult<List<UserGroupSummaryViewModelDTO>>> GetByGroupNames(string groupNames)
        //{
        //    try
        //    {
        //        if (groupNames == null || !groupNames.Any())
        //        {
        //            return BadRequest(new { message = "Group names are required." });
        //        }
        //        var groupList = groupNames.Split(',', StringSplitOptions.RemoveEmptyEntries)
        //                       .Select(x => x.Trim())
        //                       .ToList();
        //        var records = await _groupBAL.GetUserGroupRecordsByGroupNamesAsync(groupList);

        //        _controllerName = ControllerContext.ActionDescriptor.ControllerName;
        //        _methodName = ControllerContext?.RouteData?.Values["action"]?.ToString();

        //        Log.ForContext("method_name", _methodName)
        //           .ForContext("controller_name", _controllerName)
        //           .ForContext("thrown_by", "Application")
        //           .Information("Run EndPoint Of GetGroupRecordsByGroupType Method: {TimeofDay}", DateTime.Now.TimeOfDay);

        //        return Ok(records);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        ///// <summary>
        ///// Fetch group names by user id.
        ///// </summary>
        ///// <param name="userId"></param>
        ///// <returns></returns>
        //[HttpGet("by-user")]
        //public async Task<ActionResult<List<GroupSummaryDTO>>> GetGroupNamesByUserId(Guid userId)
        //{
        //    try
        //    {
        //        if (userId <= Guid.Empty)
        //        {
        //            return BadRequest(new { message = "Valid userId is required." });
        //        }

        //        var records = await _groupBAL.GetGroupNamesByUserIdAsync(userId);

        //        _controllerName = ControllerContext.ActionDescriptor.ControllerName;
        //        _methodName = ControllerContext?.RouteData?.Values["action"]?.ToString();

        //        Log.ForContext("method_name", _methodName)
        //           .ForContext("controller_name", _controllerName)
        //           .ForContext("thrown_by", "Application")
        //           .Information("Run EndPoint Of GetGroupNamesByUserId: {TimeofDay}", DateTime.Now.TimeOfDay);

        //        return Ok(records);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        /// <summary>
        /// Create a new Group record 
        /// </summary>
        /// <param name="groupModel"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerOperation(
            Summary = "Create a new group",
            Description = "Creates a new group record in the system with the provided details."
        )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GlobalResponseModel<GroupModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GlobalResponseModel<GroupModel>>> Post(
            [SwaggerParameter(Description = "The group data to create.", Required = true)] GroupDTO groupDto)
        {
            try
            {
                if (groupDto == null)
                {
                    return BadRequest(Custom.CreateError<GroupModel>("400", "Group data cannot be null"));
                }

                var result = await _groupBAL.CreategroupAsync(groupDto, _authUser.UserId);

                if (result == null)
                    return StatusCode(StatusCodes.Status500InternalServerError, Custom.CreateError<GroupModel>("500", "Unexpected error"));

                if (result.Status == "409")
                    return Conflict(result);

                if (result.Status == "500")
                    return StatusCode(StatusCodes.Status500InternalServerError, result);

                _controllerName = ControllerContext.ActionDescriptor.ControllerName;
                _methodName = ControllerContext?.RouteData?.Values["action"]?.ToString();
                Log.ForContext("method_name", _methodName)
                   .ForContext("controller_name", _controllerName)
                   .ForContext("thrown_by", "Application")
                   .Information("Run EndPoint Of Post Method in : {TimeofDay}", DateTime.Now.TimeOfDay);

                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in Post");
                throw;
            }
        }

        /// <summary>
        /// Update an existing group record.
        /// </summary>
        /// <param name="groupModel"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Update a group",
            Description = "Updates an existing group record with the provided details."
        )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GlobalResponseModel<GroupModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GlobalResponseModel<GroupModel>>> Put(
            [SwaggerParameter(Description = "The ID of the group to update.", Required = true)] Guid id,
            [SwaggerParameter(Description = "The group data to update.", Required = true)] GroupDTO groupDto)
        {
            if (groupDto == null)
            {
                return BadRequest(Custom.CreateError<GroupModel>("400", "Group data cannot be null"));
            }

            try
            {
                var result = await _groupBAL.UpdategroupAsync(id, groupDto, _authUser.UserId);

                if (result == null)
                    return StatusCode(StatusCodes.Status500InternalServerError, Custom.CreateError<GroupModel>("500", "Unexpected error"));

                if (result.Status == "404")
                    return NotFound(result);

                if (result.Status == "500")
                    return StatusCode(StatusCodes.Status500InternalServerError, result);

                _controllerName = ControllerContext.ActionDescriptor.ControllerName;
                _methodName = ControllerContext?.RouteData?.Values["action"]?.ToString();
                Log.ForContext("method_name", _methodName)
                   .ForContext("controller_name", _controllerName)
                   .ForContext("thrown_by", "Application")
                   .Information("Run EndPoint Of Put Method in : {TimeofDay}", DateTime.Now.TimeOfDay);

                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in Put");
                throw;
            }
        }

        /// <summary>
        /// Soft delete a group record by ID.
        /// </summary>
        /// <param name = "id" ></ param >
        /// < returns ></ returns >
        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Soft delete a group",
            Description = "Soft deletes a group record by setting IsActive to false."
        )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GlobalResponseModel<GroupModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GlobalResponseModel<GroupModel>>> Delete(
            [SwaggerParameter(Description = "The GUID of the group to soft delete.", Required = true)] Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    return BadRequest(Custom.CreateError<GroupModel>("400", "Id not exists to delete the record"));
                }

                var userId = _authUser?.UserId ?? 0;
                var result = await _groupBAL.DeletegroupAsync(id, userId);

                if (result == null)
                    return StatusCode(StatusCodes.Status500InternalServerError, Custom.CreateError<GroupModel>("500", "Unexpected error"));

                if (result.Status == "404")
                    return NotFound(result);

                if (result.Status == "500")
                    return StatusCode(StatusCodes.Status500InternalServerError, result);

                _controllerName = ControllerContext.ActionDescriptor.ControllerName;
                _methodName = ControllerContext?.RouteData?.Values["action"]?.ToString();
                Log.ForContext("method_name", _methodName)
                   .ForContext("controller_name", _controllerName)
                   .ForContext("thrown_by", "Application")
                   .Information("Run EndPoint Of Delete Method in : {TimeofDay}", DateTime.Now.TimeOfDay);

                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in Delete");
                throw;
            }
        }

        /// <summary>
        /// Permanently delete a group record by ID.
        /// </summary>
        /// <param name = "id" ></ param >
        /// < returns ></ returns >
        [HttpDelete("permanent/{id}")]
        [SwaggerOperation(
            Summary = "Permanently delete a group",
            Description = "Permanently deletes a group record from the database."
        )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GlobalResponseModel<GroupModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GlobalResponseModel<GroupModel>>> PermanentDelete(
            [SwaggerParameter(Description = "The GUID of the group to permanently delete.", Required = true)] Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    return BadRequest(Custom.CreateError<GroupModel>("400", "Id not exists to delete the record"));
                }

                var result = await _groupBAL.PermanentDeletegroupAsync(id);

                if (result == null)
                    return StatusCode(StatusCodes.Status500InternalServerError, Custom.CreateError<GroupModel>("500", "Unexpected error"));

                if (result.Status == "404")
                    return NotFound(result);

                if (result.Status == "500")
                    return StatusCode(StatusCodes.Status500InternalServerError, result);

                _controllerName = ControllerContext.ActionDescriptor.ControllerName;
                _methodName = ControllerContext?.RouteData?.Values["action"]?.ToString();
                Log.ForContext("method_name", _methodName)
                   .ForContext("controller_name", _controllerName)
                   .ForContext("thrown_by", "Application")
                   .Information("Run EndPoint Of Permanent Delete Method in : {TimeofDay}", DateTime.Now.TimeOfDay);

                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in PermanentDelete");
                throw;
            }
        }
    }
}
