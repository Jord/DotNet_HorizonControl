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
    [Route("api/suites")]
    [ApiController]
    [Tags("Suites")]
    public class SuitesController : ControllerBase
    {
        private AuthUser _authUser;
        string? _controllerName = ""; string? _methodName = "";
        private readonly IUserService _userService;
        private readonly SuiteBAL _suiteBal;
        public SuitesController(IUserService userService, SuiteBAL suiteBAL)
        {
            _suiteBal = suiteBAL;
            _userService = userService;
            _authUser = userService.GetUserInfo();
        }

        [HttpGet]
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
        public async Task<ActionResult<SuiteModel>> PostAsync(SuitesDTO model)
        {
            try
            {
                var userId = _authUser?.UserId ?? 0;
                var userName = _authUser?.UserName ?? string.Empty;
                _controllerName = ControllerContext.ActionDescriptor.ControllerName;
                _methodName = ControllerContext?.RouteData?.Values["action"]?.ToString();

                var (result, message) = await _suiteBal.CreateAsync(model, userId);

                Log.ForContext("method_name", _methodName)
                   .ForContext("controller_name", _controllerName)
                   .ForContext("thrown_by", "Application")
                   .Information("Run EndPoint Of GetById Method: {TimeofDay}", DateTime.Now.TimeOfDay);
                if (result == null)
                {
                    return Conflict(new { message }); //  exact message from DAL
                }
                return CreatedAtAction(nameof(GetByIdAsync), new { id = result.GuidId }, result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in PostAsync");
                throw;
            }

        }
        [HttpPut]
        public async Task<IActionResult> PutAsync(Guid id, SuitesDTO model)
        {
            try
            {
                _controllerName = ControllerContext.ActionDescriptor.ControllerName;
                _methodName = ControllerContext?.RouteData?.Values["action"]?.ToString();
                var userId = _authUser?.UserId ?? 0;
                var userName = _authUser?.UserName ?? string.Empty;

                await _suiteBal.UpdateAsync(id, model, userId);

                Log.ForContext("method_name", _methodName)
                   .ForContext("controller_name", _controllerName)
                   .ForContext("thrown_by", "Application")
                   .Information("Run EndPoint Of Put Method: {TimeofDay}", DateTime.Now.TimeOfDay);

                return Ok(model);
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
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            try
            {
                _controllerName = ControllerContext.ActionDescriptor.ControllerName;
                _methodName = ControllerContext?.RouteData?.Values["action"]?.ToString();
                var result = await _suiteBal.DeleteAsync(id);
                if (result == 0)
                    return NotFound();
                Log.ForContext("method_name", _methodName)
                   .ForContext("controller_name", _controllerName)
                   .ForContext("thrown_by", "Application")
                   .Information("Run EndPoint Of Delete Method: {TimeofDay}", DateTime.Now.TimeOfDay);
                return NoContent();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in DeleteAsync");
                throw;
            }
        }
    }
}