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
    [Route("api/users")]
    [ApiController]
    [Tags("Users")]
    public class UsersController : ControllerBase
    {
        private AuthUser _authUser;
        string? _controllerName = "";
        string? _methodName = "";
        private readonly IUserService _userService;
        private readonly IUserBAL _userBal;

        public UsersController(IUserService userService, IUserBAL userBAL)
        {
            _userBal = userBAL;
            _userService = userService;
            _authUser = userService.GetUserInfo();
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Get all users",
            Description = "Retrieves a list of all user records from the system."
        )]
        public async Task<ActionResult<List<UserModel>>> GetAsync()
        {
            try
            {
                _controllerName = ControllerContext.ActionDescriptor.ControllerName;
                _methodName = ControllerContext?.RouteData?.Values["action"]?.ToString();
                Log.ForContext("method_name", _methodName)
                   .ForContext("controller_name", _controllerName)
                   .ForContext("thrown_by", "Application")
                   .Information("Run EndPoint Of Get Method: {TimeofDay}", DateTime.Now.TimeOfDay);

                return await _userBal.GetAllAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in GetAsync");
                throw;
            }
        }

        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Get a user by ID",
            Description = "Retrieves a specific user record identified by its GUID."
        )]
        public async Task<ActionResult<UserModel>> GetByIdAsync(Guid id)
        {
            try
            {
                var result = await _userBal.GetByIdAsync(id);
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
            Summary = "Create a new user",
            Description = "Creates a new user record in the system with the provided details."
        )]
        public async Task<ActionResult<GlobalResponseModel<UserModel>>> PostAsync(
            [SwaggerParameter(Description = "The user data to create.", Required = true)] UserDTO model)
        {
            try
            {
                var userId = _authUser?.UserId ?? 0;
                var userName = _authUser?.UserName ?? string.Empty;
                _controllerName = ControllerContext.ActionDescriptor.ControllerName;
                _methodName = ControllerContext?.RouteData?.Values["action"]?.ToString();

                var result = await _userBal.CreateAsync(model, userId);

                if (result == null)
                    return StatusCode(StatusCodes.Status500InternalServerError, Custom.CreateError<UserModel>("500", "Unexpected error"));

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
            Summary = "Update a user",
            Description = "Updates an existing user record identified by its GUID with the provided details."
        )]
        public async Task<ActionResult<GlobalResponseModel<UserModel>>> PutAsync(
            Guid id,
            [SwaggerParameter(Description = "The user data to update.", Required = true)] UserDTO model)
        {
            try
            {
                _controllerName = ControllerContext.ActionDescriptor.ControllerName;
                _methodName = ControllerContext?.RouteData?.Values["action"]?.ToString();
                var userId = _authUser?.UserId ?? 0;
                var userName = _authUser?.UserName ?? string.Empty;

                var result = await _userBal.UpdateAsync(id, model, userId);

                if (result == null)
                    return StatusCode(StatusCodes.Status500InternalServerError, Custom.CreateError<UserModel>("500", "Unexpected error"));

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
            Summary = "Soft delete a user",
            Description = "Soft deletes a user record by setting IsActive to false."
        )]
        public async Task<ActionResult<GlobalResponseModel<UserModel>>> DeleteAsync(
            [SwaggerParameter(Description = "The GUID of the user to soft delete.", Required = true)] Guid id)
        {
            try
            {
                _controllerName = ControllerContext.ActionDescriptor.ControllerName;
                _methodName = ControllerContext?.RouteData?.Values["action"]?.ToString();
                var userId = _authUser?.UserId ?? 0;

                var result = await _userBal.DeleteAsync(id, userId);

                if (result == null)
                    return StatusCode(StatusCodes.Status500InternalServerError, Custom.CreateError<UserModel>("500", "Unexpected error"));

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
            Summary = "Permanently delete a user",
            Description = "Permanently deletes a user record from the database."
        )]
        public async Task<ActionResult<GlobalResponseModel<UserModel>>> PermanentDeleteAsync(
            [SwaggerParameter(Description = "The GUID of the user to permanently delete.", Required = true)] Guid id)
        {
            try
            {
                _controllerName = ControllerContext.ActionDescriptor.ControllerName;
                _methodName = ControllerContext?.RouteData?.Values["action"]?.ToString();

                var result = await _userBal.PermanentDeleteAsync(id);

                if (result == null)
                    return StatusCode(StatusCodes.Status500InternalServerError, Custom.CreateError<UserModel>("500", "Unexpected error"));

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
