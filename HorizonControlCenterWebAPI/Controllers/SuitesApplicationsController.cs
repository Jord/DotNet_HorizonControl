using CentralLogModels;
using HorizonControlCenterBAL;
using HorizonControlCenterDAL.Entities;
using HorizonControlCenterModels;
using HorizonControlCenterModels.DTO;
using HorizonControlCenterWebAPI.Security;
using HorizonControlCenterWebAPI.Services.UserService;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Swashbuckle.AspNetCore.Annotations;

namespace HorizonControlCenterWebAPI.Controllers
{
    //[Authorize]
    [Route("api/suites-applications")]
    [ApiController]
    [Tags("Suites Applications")]
    public class SuitesApplicationsController : ControllerBase
    {
        private AuthUser _authUser;
        string? _controllerName = ""; 
        string? _methodName = "";
        private readonly IUserService _userService;
        private readonly SuitesApplicationBAL _suitesApplicationBal;

        public SuitesApplicationsController(IUserService userService, SuitesApplicationBAL suitesApplicationBAL)
        {
            _suitesApplicationBal = suitesApplicationBAL;
            _userService = userService;
            _authUser = userService.GetUserInfo();
        }

        [HttpGet]
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
        public async Task<ActionResult<SuitesApplicationModel>> Post(SuitesApplicationDTO model)
        {
            try
            {
                var userId = _authUser?.UserId ?? 0;
                var userName = _authUser?.UserName ?? string.Empty;
                _controllerName = ControllerContext.ActionDescriptor.ControllerName;
                _methodName = ControllerContext?.RouteData?.Values["action"]?.ToString();

                var (result, message) = await _suitesApplicationBal.CreateAsync(model, userId);

                Log.ForContext("method_name", _methodName)
                   .ForContext("controller_name", _controllerName)
                   .ForContext("thrown_by", "Application")
                   .Information("Run EndPoint Of Post Method: {TimeofDay}", DateTime.Now.TimeOfDay);

                if (result == null)
                {
                    return Conflict(new { message });
                }
                return CreatedAtAction(nameof(GetById), new { id = result.GuidId }, result);
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
        public async Task<IActionResult> Put(Guid id, SuitesApplicationDTO model)
        {
            try
            {
                _controllerName = ControllerContext.ActionDescriptor.ControllerName;
                _methodName = ControllerContext?.RouteData?.Values["action"]?.ToString();
                var userId = _authUser?.UserId ?? 0;
                var userName = _authUser?.UserName ?? string.Empty;

                var result = await _suitesApplicationBal.UpdateAsync(id, model, userId);

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
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            try
            {
                _controllerName = ControllerContext.ActionDescriptor.ControllerName;
                _methodName = ControllerContext?.RouteData?.Values["action"]?.ToString();

                var result = await _suitesApplicationBal.DeleteAsync(id);

                Log.ForContext("method_name", _methodName)
                   .ForContext("controller_name", _controllerName)
                   .ForContext("thrown_by", "Application")
                   .Information("Run EndPoint Of Delete Method: {TimeofDay}", DateTime.Now.TimeOfDay);

                if (result == 0)
                    return NotFound(new { message = "Record not found" });

                if (result == -1)
                    return Conflict(new { message = "Record already deleted" });

                return Ok(new { message = "Record deleted successfully" });
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
        //public async Task<IActionResult> Patch(Guid id, [FromBody] JsonPatchDocument<SuitesApplication> patchDoc)
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
