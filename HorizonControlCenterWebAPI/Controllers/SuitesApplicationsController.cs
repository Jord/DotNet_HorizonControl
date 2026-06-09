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
    [Route("api/suite-applications")]
    [ApiController]
    [Tags("Suites Applications")]
    public class SuitesApplicationsController : ControllerBase
    {
        private AuthUser _authUser;
        string? _controllerName = "";
        string? _methodName = "";
        private readonly IUserService _userService;
        private readonly ISuitesApplicationBAL _suitesApplicationBal;

        public SuitesApplicationsController(IUserService userService, ISuitesApplicationBAL suitesApplicationBAL)
        {
            _suitesApplicationBal = suitesApplicationBAL;
            _userService = userService;
            _authUser = userService.GetUserInfo();
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Get all suite applications",
            Description = "Retrieves a list of all suite application records from the system."
        )]
        public async Task<ActionResult<List<SuitesApplicationModel>>> Get()
        {
            try
            {
                _controllerName = ControllerContext.ActionDescriptor.ControllerName;
                _methodName = ControllerContext?.RouteData?.Values["action"]?.ToString();
                Log.ForContext("method_name", _methodName)
                   .ForContext("controller_name", _controllerName)
                   .ForContext("thrown_by", "Application")
                   .Information("Run EndPoint Of Get Method: {TimeofDay}", DateTime.Now.TimeOfDay);

                return await _suitesApplicationBal.GetAllAsync();
            }
            catch (Exception)
            {
                Log.ForContext("method_name", _methodName)
                     .ForContext("controller_name", _controllerName)
                     .ForContext("thrown_by", "Application")
                     .Error("An error occurred while processing the request: {Message}");
                throw;
            }
        }

        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Get a suite application by ID",
            Description = "Retrieves a specific suite application record identified by its GUID."
        )]
        public async Task<ActionResult<SuitesApplicationModel>> GetById(Guid id)
        {
            try
            {
                var result = await _suitesApplicationBal.GetByIdAsync(id);
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
            catch (Exception)
            {
                Log.ForContext("method_name", _methodName)
                    .ForContext("controller_name", _controllerName)
                    .ForContext("thrown_by", "Application")
                    .Error("An error occurred while processing the request: {Message}");
                throw;
            }
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Create a new suite application",
            Description = "Creates a new suite application record in the system with the provided details."
        )]
        public async Task<ActionResult<GlobalResponseModel<SuitesApplicationModel>>> Post(
            [SwaggerParameter(Description = "The suite application data to create.", Required = true)] SuitesApplicationDTO model)
        {
            try
            {
                var userId = _authUser?.UserId ?? 0;
                var userName = _authUser?.UserName ?? string.Empty;
                _controllerName = ControllerContext.ActionDescriptor.ControllerName;
                _methodName = ControllerContext?.RouteData?.Values["action"]?.ToString();

                var result = await _suitesApplicationBal.CreateAsync(model, userId);

                if (result == null)
                    return StatusCode(StatusCodes.Status500InternalServerError, Custom.CreateError<SuitesApplicationModel>("500", "Unexpected error"));

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
            catch (Exception)
            {
                Log.ForContext("method_name", _methodName)
                  .ForContext("controller_name", _controllerName)
                  .ForContext("thrown_by", "Application")
                  .Error("An error occurred while processing the request: {Message}");
                throw;
            }
        }

        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Update a suite application",
            Description = "Updates an existing suite application record identified by its GUID with the provided details."
        )]
        public async Task<ActionResult<GlobalResponseModel<SuitesApplicationModel>>> Put(
            Guid id,
            [SwaggerParameter(Description = "The suite application data to update.", Required = true)] SuitesApplicationDTO model)
        {
            try
            {
                _controllerName = ControllerContext.ActionDescriptor.ControllerName;
                _methodName = ControllerContext?.RouteData?.Values["action"]?.ToString();
                var userId = _authUser?.UserId ?? 0;
                var userName = _authUser?.UserName ?? string.Empty;

                var result = await _suitesApplicationBal.UpdateAsync(id, model, userId);

                if (result == null)
                    return StatusCode(StatusCodes.Status500InternalServerError, Custom.CreateError<SuitesApplicationModel>("500", "Unexpected error"));

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
            catch (Exception)
            {
                Log.ForContext("method_name", _methodName)
                  .ForContext("controller_name", _controllerName)
                  .ForContext("thrown_by", "Application")
                  .Error("An error occurred while processing the request: {Message}");
                throw;
            }
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Soft delete a suite application",
            Description = "Soft deletes a suite application record by setting IsActive to false."
        )]
        public async Task<ActionResult<GlobalResponseModel<SuitesApplicationModel>>> DeleteAsync(
            [SwaggerParameter(Description = "The GUID of the suite application to soft delete.", Required = true)] Guid id)
        {
            try
            {
                _controllerName = ControllerContext.ActionDescriptor.ControllerName;
                _methodName = ControllerContext?.RouteData?.Values["action"]?.ToString();
                var userId = _authUser?.UserId ?? 0;

                var result = await _suitesApplicationBal.DeleteAsync(id, userId);

                if (result == null)
                    return StatusCode(StatusCodes.Status500InternalServerError, Custom.CreateError<SuitesApplicationModel>("500", "Unexpected error"));

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
            catch (Exception)
            {
                Log.ForContext("method_name", _methodName)
                  .ForContext("controller_name", _controllerName)
                  .ForContext("thrown_by", "Application")
                  .Error("An error occurred while processing the request: {Message}");
                throw;
            }
        }

        [HttpDelete("permanent/{id}")]
        [SwaggerOperation(
            Summary = "Permanently delete a suite application",
            Description = "Permanently deletes a suite application record from the database."
        )]
        public async Task<ActionResult<GlobalResponseModel<SuitesApplicationModel>>> PermanentDeleteAsync(
            [SwaggerParameter(Description = "The GUID of the suite application to permanently delete.", Required = true)] Guid id)
        {
            try
            {
                _controllerName = ControllerContext.ActionDescriptor.ControllerName;
                _methodName = ControllerContext?.RouteData?.Values["action"]?.ToString();

                var result = await _suitesApplicationBal.PermanentDeleteAsync(id);

                if (result == null)
                    return StatusCode(StatusCodes.Status500InternalServerError, Custom.CreateError<SuitesApplicationModel>("500", "Unexpected error"));

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
            catch (Exception)
            {
                Log.ForContext("method_name", _methodName)
                  .ForContext("controller_name", _controllerName)
                  .ForContext("thrown_by", "Application")
                  .Error("An error occurred while processing the request: {Message}");
                throw;
            }
        }

        //[HttpPatch("{id}")]
        //public async Task<IActionResult> Patch(Guid id, [FromBody] JsonPatchDocument<SuiteApplication> patchDoc)
        //{
        //    try
        //    {
        //        if (patchDoc == null)
        //            return BadRequest();

        //        var userId = _authUser?.UserId ?? 0;
        //        _controllerName = ControllerContext.ActionDescriptor.ControllerName;
        //        _methodName = ControllerContext?.RouteData?.Values["action"]?.ToString();

        //        var result = await _suitesApplicationBal.PatchAsync(id, patchDoc, userId);

        //        Log.ForContext("method_name", _methodName)
        //           .ForContext("controller_name", _controllerName)
        //           .ForContext("thrown_by", "Application")
        //           .Information("Run EndPoint Of Patch Method: {TimeofDay}", DateTime.Now.TimeOfDay);

        //        return Ok(result);
        //    }
        //    catch (Exception)
        //    {
        //        Log.ForContext("method_name", _methodName)
        //          .ForContext("controller_name", _controllerName)
        //          .ForContext("thrown_by", "Application")
        //          .Error("An error occurred while processing the request: {Message}");
        //        throw;
        //    }
        //}

        [HttpGet("application-types")]
        [SwaggerOperation(
            Summary = "Get application types",
            Description = "Retrieves a list of all distinct application types."
        )]
        public async Task<ActionResult<List<string>>> GetApplicationTypes()
        {
            try
            {
                _controllerName = ControllerContext.ActionDescriptor.ControllerName;
                _methodName = ControllerContext?.RouteData?.Values["action"]?.ToString();

                Log.ForContext("method_name", _methodName)
                   .ForContext("controller_name", _controllerName)
                   .ForContext("thrown_by", "Application")
                   .Information("Run EndPoint Of GetApplicationTypes Method: {TimeofDay}", DateTime.Now.TimeOfDay);

                var applicationTypes = await _suitesApplicationBal.GetDistinctApplicationTypesAsync();

                return Ok(applicationTypes);
            }
            catch (Exception)
            {
                Log.ForContext("method_name", _methodName)
                  .ForContext("controller_name", _controllerName)
                  .ForContext("thrown_by", "Application")
                  .Error("An error occurred while processing the request: {Message}");
                throw;
            }
        }
    }
}
