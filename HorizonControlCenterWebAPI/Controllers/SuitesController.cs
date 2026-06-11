using CentralLogModels;
using HorizonControlCenterBAL;
using HorizonControlCenterBAL.Interfaces;
using HorizonControlCenterDAL.Entities;
using HorizonControlCenterModels;
using HorizonControlCenterModels.Custom;
using HorizonControlCenterModels.DTO;
using HorizonControlCenterWebAPI.Security;
using HorizonControlCenterWebAPI.Services.UserService;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SecurityAuthorization;
using Serilog;
using Swashbuckle.AspNetCore.Annotations;

namespace HorizonControlCenterWebAPI.Controllers
{
    [Authorize]
    [Route("api/suites")]
    [ApiController]
    [Tags("Suites")]
    public class SuitesController : ControllerBase
    {
        private AuthUser _authUser;
        string? _controllerName = ""; string? _methodName = "";
        private readonly IUserService _userService;
        private readonly ISuiteBAL _suiteBal;
        public SuitesController(IUserService userService, ISuiteBAL suiteBAL)
        {
            _suiteBal = suiteBAL;
            _userService = userService;
            _authUser = userService.GetUserInfo();
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Get all suites",
            Description = "Retrieves a list of all suite records from the system."
        )]
        public async Task<ActionResult<List<SuiteModel>>> GetAsync()
        {
            try
            {
                _controllerName = ControllerContext.ActionDescriptor.ControllerName;
                _methodName = ControllerContext?.RouteData?.Values["action"]?.ToString();
                Log.ForContext("method_name", _methodName)
                   .ForContext("controller_name", _controllerName)
                   .ForContext("thrown_by", "Application")
                   .Information("Run EndPoint Of Get Method: {TimeofDay}", DateTime.Now.TimeOfDay);

                return await _suiteBal.GetAllAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in GetAsync");
                throw;
            }

        }

        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Get a suite by ID",
            Description = "Retrieves a specific suite record identified by its GUID."
        )]
        public async Task<ActionResult<SuiteModel>> GetByIdAsync(Guid id)
        {
            try
            {
                var result = await _suiteBal.GetByIdAsync(id);
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
            Summary = "Create a new suite",
            Description = "Creates a new suite record in the system with the provided details."
        )]
        public async Task<ActionResult<GlobalResponseModel<SuiteModel>>> PostAsync(
            [SwaggerParameter(Description = "The suite data to create.", Required = true)] SuitesDTO model)
        {
            try
            {
                var userId = _authUser?.UserId ?? 0;
                var userName = _authUser?.UserName ?? string.Empty;
                _controllerName = ControllerContext.ActionDescriptor.ControllerName;
                _methodName = ControllerContext?.RouteData?.Values["action"]?.ToString();

                var result = await _suiteBal.CreateAsync(model, userId);

                if (result == null)
                    return StatusCode(StatusCodes.Status500InternalServerError, Custom.CreateError<SuiteModel>("500", "Unexpected error"));

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
            Summary = "Update a suite",
            Description = "Updates an existing suite record identified by its GUID with the provided details."
        )]
        public async Task<ActionResult<GlobalResponseModel<SuiteModel>>> PutAsync(
            Guid id,
            [SwaggerParameter(Description = "The suite data to update.", Required = true)] SuitesDTO model)
        {
            try
            {
                _controllerName = ControllerContext.ActionDescriptor.ControllerName;
                _methodName = ControllerContext?.RouteData?.Values["action"]?.ToString();
                var userId = _authUser?.UserId ?? 0;
                var userName = _authUser?.UserName ?? string.Empty;

                var result = await _suiteBal.UpdateAsync(id, model, userId);

                if (result == null)
                    return StatusCode(StatusCodes.Status500InternalServerError, Custom.CreateError<SuiteModel>("500", "Unexpected error"));

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
        //[HttpPatch("{id}")]
        //public async Task<IActionResult> Patch(Guid id, [FromBody] JsonPatchDocument<Suite> patchDoc)
        //{
        //    try
        //    {
        //        if (patchDoc == null)
        //            return BadRequest();

        //        var userId = _authUser?.UserId ?? 0;

        //        var result = await _suiteBal.PatchAsync(id, patchDoc, userId);

        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex, "Error in Patch");
        //        throw;
        //    }
        //}

        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Soft delete a suite",
            Description = "Soft deletes a suite record by setting IsActive to false."
        )]
        public async Task<ActionResult<GlobalResponseModel<SuiteModel>>> DeleteAsync(
            [SwaggerParameter(Description = "The GUID of the suite to soft delete.", Required = true)] Guid id)
        {
            try
            {
                _controllerName = ControllerContext.ActionDescriptor.ControllerName;
                _methodName = ControllerContext?.RouteData?.Values["action"]?.ToString();
                var userId = _authUser?.UserId ?? 0;
                var userName = _authUser?.UserName ?? string.Empty;
                var result = await _suiteBal.DeleteAsync(id, userId);

                if (result == null)
                    return StatusCode(StatusCodes.Status500InternalServerError, Custom.CreateError<SuiteModel>("500", "Unexpected error"));

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
            Summary = "Permanently delete a suite",
            Description = "Permanently deletes a suite record from the database."
        )]
        public async Task<ActionResult<GlobalResponseModel<SuiteModel>>> PermanentDeleteAsync(
            [SwaggerParameter(Description = "The GUID of the suite to permanently delete.", Required = true)] Guid id)
        {
            try
            {
                _controllerName = ControllerContext.ActionDescriptor.ControllerName;
                _methodName = ControllerContext?.RouteData?.Values["action"]?.ToString();

                var result = await _suiteBal.PermanentDeleteAsync(id);

                if (result == null)
                    return StatusCode(StatusCodes.Status500InternalServerError, Custom.CreateError<SuiteModel>("500", "Unexpected error"));

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