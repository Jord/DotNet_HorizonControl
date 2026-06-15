using HorizonControlCenterBAL.Interfaces;
using HorizonControlCenterModels;
using HorizonControlCenterModels.Custom;
using HorizonControlCenterModels.DTO;
using HorizonControlCenterWebAPI.Security;
using HorizonControlCenterWebAPI.Services.UserService;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Swashbuckle.AspNetCore.Annotations;

namespace HorizonControlCenterWebAPI.Controllers
{
    [Route("api/group-groups")]
    [ApiController]
    [Tags("Group Groups")]
    public class GroupGroupsController : ControllerBase
    {
        private AuthUser _authUser;
        string? _controllerName = "";
        string? _methodName = "";
        private readonly IUserService _userService;
        private readonly IGroupGroupBAL _groupGroupBal;

        public GroupGroupsController(IUserService userService, IGroupGroupBAL groupGroupBAL)
        {
            _groupGroupBal = groupGroupBAL;
            _userService = userService;
            _authUser = userService.GetUserInfo();
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Get all group-group mappings",
            Description = "Retrieves a list of all group-group mapping records from the system. Each record includes GroupName and MapToGroupName for easier identification."
        )]
        public async Task<ActionResult<List<GroupGroupModel>>> GetAsync()
        {
            try
            {
                _controllerName = ControllerContext.ActionDescriptor.ControllerName;
                _methodName = ControllerContext?.RouteData?.Values["action"]?.ToString();
                Log.ForContext("method_name", _methodName)
                   .ForContext("controller_name", _controllerName)
                   .ForContext("thrown_by", "Application")
                   .Information("Run EndPoint Of Get Method: {TimeofDay}", DateTime.Now.TimeOfDay);

                return await _groupGroupBal.GetAllAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in GetAsync");
                throw;
            }
        }

        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Get a group-group mapping by ID",
            Description = "Retrieves a specific group-group mapping record identified by its GUID. The response includes GroupName and MapToGroupName for easier identification."
        )]
        public async Task<ActionResult<GroupGroupModel>> GetByIdAsync(Guid id)
        {
            try
            {
                var result = await _groupGroupBal.GetByIdAsync(id);
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

        [HttpPost]
        [SwaggerOperation(
            Summary = "Create a new group-group mapping",
            Description = "Creates a new group-group mapping record in the system with the provided details. The response includes GroupName and MapToGroupName for easier identification."
        )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GlobalResponseModel<GroupGroupModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GlobalResponseModel<GroupGroupModel>>> PostAsync(
            [SwaggerParameter(Description = "The group-group mapping data to create.", Required = true)] GroupGroupDTO model)
        {
            try
            {
                var userId = _authUser?.UserId ?? 0;
                var userName = _authUser?.UserName ?? string.Empty;
                _controllerName = ControllerContext.ActionDescriptor.ControllerName;
                _methodName = ControllerContext?.RouteData?.Values["action"]?.ToString();

                var result = await _groupGroupBal.CreateAsync(model, userId);

                if (result == null)
                    return StatusCode(StatusCodes.Status500InternalServerError, Custom.CreateError<GroupGroupModel>("500", "Unexpected error"));

                if (result.Status == "400")
                    return BadRequest(result);

                if (result.Status == "409")
                    return Conflict(result);

                if (result.Status == "500")
                    return StatusCode(StatusCodes.Status500InternalServerError, result);

                Log.ForContext("method_name", _methodName)
                   .ForContext("controller_name", _controllerName)
                   .ForContext("thrown_by", "Application")
                   .Information("Run EndPoint Of Post Method: {TimeofDay}", DateTime.Now.TimeOfDay);

                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in PostAsync");
                throw;
            }
        }

        [HttpPut]
        [SwaggerOperation(
            Summary = "Update a group-group mapping",
            Description = "Updates an existing group-group mapping record identified by its GUID with the provided details. The response includes GroupName and MapToGroupName for easier identification."
        )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GlobalResponseModel<GroupGroupModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GlobalResponseModel<GroupGroupModel>>> PutAsync(
            Guid id,
            [SwaggerParameter(Description = "The group-group mapping data to update.", Required = true)] GroupGroupDTO model)
        {
            try
            {
                _controllerName = ControllerContext.ActionDescriptor.ControllerName;
                _methodName = ControllerContext?.RouteData?.Values["action"]?.ToString();
                var userId = _authUser?.UserId ?? 0;
                var userName = _authUser?.UserName ?? string.Empty;

                var result = await _groupGroupBal.UpdateAsync(id, model, userId);

                if (result == null)
                    return StatusCode(StatusCodes.Status500InternalServerError, Custom.CreateError<GroupGroupModel>("500", "Unexpected error"));

                if (result.Status == "400")
                    return BadRequest(result);

                if (result.Status == "404")
                    return NotFound(result);

                if (result.Status == "500")
                    return StatusCode(StatusCodes.Status500InternalServerError, result);

                Log.ForContext("method_name", _methodName)
                   .ForContext("controller_name", _controllerName)
                   .ForContext("thrown_by", "Application")
                   .Information("Run EndPoint Of Put Method: {TimeofDay}", DateTime.Now.TimeOfDay);

                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in PutAsync");
                throw;
            }
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Delete a group-group mapping",
            Description = "Deletes a group-group mapping record from the database."
        )]
        public async Task<ActionResult<GlobalResponseModel<GroupGroupModel>>> DeleteAsync(
            [SwaggerParameter(Description = "The GUID of the group-group mapping to delete.", Required = true)] Guid id)
        {
            try
            {
                _controllerName = ControllerContext.ActionDescriptor.ControllerName;
                _methodName = ControllerContext?.RouteData?.Values["action"]?.ToString();

                var result = await _groupGroupBal.DeleteAsync(id);

                if (result == null)
                    return StatusCode(StatusCodes.Status500InternalServerError, Custom.CreateError<GroupGroupModel>("500", "Unexpected error"));

                if (result.Status == "404")
                    return NotFound(result);

                if (result.Status == "500")
                    return StatusCode(StatusCodes.Status500InternalServerError, result);

                Log.ForContext("method_name", _methodName)
                   .ForContext("controller_name", _controllerName)
                   .ForContext("thrown_by", "Application")
                   .Information("Run EndPoint Of Delete Method: {TimeofDay}", DateTime.Now.TimeOfDay);

                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in DeleteAsync");
                throw;
            }
        }

        [HttpDelete("permanent/{id}")]
        [SwaggerOperation(
            Summary = "Permanently delete a group-group mapping",
            Description = "Permanently deletes a group-group mapping record from the database."
        )]
        public async Task<ActionResult<GlobalResponseModel<GroupGroupModel>>> PermanentDeleteAsync(
            [SwaggerParameter(Description = "The GUID of the group-group mapping to permanently delete.", Required = true)] Guid id)
        {
            try
            {
                _controllerName = ControllerContext.ActionDescriptor.ControllerName;
                _methodName = ControllerContext?.RouteData?.Values["action"]?.ToString();

                var result = await _groupGroupBal.PermanentDeleteAsync(id);

                if (result == null)
                    return StatusCode(StatusCodes.Status500InternalServerError, Custom.CreateError<GroupGroupModel>("500", "Unexpected error"));

                if (result.Status == "404")
                    return NotFound(result);

                if (result.Status == "500")
                    return StatusCode(StatusCodes.Status500InternalServerError, result);

                Log.ForContext("method_name", _methodName)
                   .ForContext("controller_name", _controllerName)
                   .ForContext("thrown_by", "Application")
                   .Information("Run EndPoint Of Permanent Delete Method: {TimeofDay}", DateTime.Now.TimeOfDay);

                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in PermanentDeleteAsync");
                throw;
            }
        }
    }
}
