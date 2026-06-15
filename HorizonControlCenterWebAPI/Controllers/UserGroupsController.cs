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
    [Route("api/user-groups")]
    [ApiController]
    [Tags("User Groups")]
    public class UserGroupsController : ControllerBase
    {
        private AuthUser _authUser;
        string? _controllerName = "";
        string? _methodName = "";
        private readonly IUserService _userService;
        private readonly IUserGroupBAL _userGroupBal;

        public UserGroupsController(IUserService userService, IUserGroupBAL userGroupBAL)
        {
            _userGroupBal = userGroupBAL;
            _userService = userService;
            _authUser = userService.GetUserInfo();
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Get all user-group mappings",
            Description = "Retrieves a list of all user-group mapping records from the system. Each record includes UserName and GroupName for easier identification."
        )]
        public async Task<ActionResult<List<UserGroupModel>>> GetAsync()
        {
            try
            {
                _controllerName = ControllerContext.ActionDescriptor.ControllerName;
                _methodName = ControllerContext?.RouteData?.Values["action"]?.ToString();
                Log.ForContext("method_name", _methodName)
                   .ForContext("controller_name", _controllerName)
                   .ForContext("thrown_by", "Application")
                   .Information("Run EndPoint Of Get Method: {TimeofDay}", DateTime.Now.TimeOfDay);

                return await _userGroupBal.GetAllAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in GetAsync");
                throw;
            }
        }

        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Get a user-group mapping by ID",
            Description = "Retrieves a specific user-group mapping record identified by its GUID. The response includes UserName and GroupName for easier identification."
        )]
        public async Task<ActionResult<UserGroupModel>> GetByIdAsync(Guid id)
        {
            try
            {
                var result = await _userGroupBal.GetByIdAsync(id);
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
            Summary = "Create a new user-group mapping",
            Description = "Creates a new user-group mapping record in the system with the provided details. The response includes UserName and GroupName for easier identification."
        )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GlobalResponseModel<UserGroupModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GlobalResponseModel<UserGroupModel>>> PostAsync(
            [SwaggerParameter(Description = "The user-group mapping data to create.", Required = true)] UserGroupDTO model)
        {
            try
            {
                var userId = _authUser?.UserId ?? 0;
                var userName = _authUser?.UserName ?? string.Empty;
                _controllerName = ControllerContext.ActionDescriptor.ControllerName;
                _methodName = ControllerContext?.RouteData?.Values["action"]?.ToString();

                var result = await _userGroupBal.CreateAsync(model, userId);

                if (result == null)
                    return StatusCode(StatusCodes.Status500InternalServerError, Custom.CreateError<UserGroupModel>("500", "Unexpected error"));

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
            Summary = "Update a user-group mapping",
            Description = "Updates an existing user-group mapping record identified by its GUID with the provided details. The response includes UserName and GroupName for easier identification."
        )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GlobalResponseModel<UserGroupModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GlobalResponseModel<UserGroupModel>>> PutAsync(
            Guid id,
            [SwaggerParameter(Description = "The user-group mapping data to update.", Required = true)] UserGroupDTO model)
        {
            try
            {
                _controllerName = ControllerContext.ActionDescriptor.ControllerName;
                _methodName = ControllerContext?.RouteData?.Values["action"]?.ToString();
                var userId = _authUser?.UserId ?? 0;
                var userName = _authUser?.UserName ?? string.Empty;

                var result = await _userGroupBal.UpdateAsync(id, model, userId);

                if (result == null)
                    return StatusCode(StatusCodes.Status500InternalServerError, Custom.CreateError<UserGroupModel>("500", "Unexpected error"));

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
            Summary = "Soft delete a user-group mapping",
            Description = "Soft deletes a user-group mapping record by setting IsActive to false."
        )]
        public async Task<ActionResult<GlobalResponseModel<UserGroupModel>>> DeleteAsync(
            [SwaggerParameter(Description = "The GUID of the user-group mapping to soft delete.", Required = true)] Guid id)
        {
            try
            {
                _controllerName = ControllerContext.ActionDescriptor.ControllerName;
                _methodName = ControllerContext?.RouteData?.Values["action"]?.ToString();
                var userId = _authUser?.UserId ?? 0;

                var result = await _userGroupBal.DeleteAsync(id, userId);

                if (result == null)
                    return StatusCode(StatusCodes.Status500InternalServerError, Custom.CreateError<UserGroupModel>("500", "Unexpected error"));

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
            Summary = "Permanently delete a user-group mapping",
            Description = "Permanently deletes a user-group mapping record from the database."
        )]
        public async Task<ActionResult<GlobalResponseModel<UserGroupModel>>> PermanentDeleteAsync(
            [SwaggerParameter(Description = "The GUID of the user-group mapping to permanently delete.", Required = true)] Guid id)
        {
            try
            {
                _controllerName = ControllerContext.ActionDescriptor.ControllerName;
                _methodName = ControllerContext?.RouteData?.Values["action"]?.ToString();

                var result = await _userGroupBal.PermanentDeleteAsync(id);

                if (result == null)
                    return StatusCode(StatusCodes.Status500InternalServerError, Custom.CreateError<UserGroupModel>("500", "Unexpected error"));

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
