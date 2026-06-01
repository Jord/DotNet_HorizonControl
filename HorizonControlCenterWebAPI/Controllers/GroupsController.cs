using AutoMapper;
using HorizonControlCenterBAL;
using HorizonControlCenterModels;
using HorizonControlCenterModels.DTO;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace HorizonControlCenterWebAPI.Controllers
{
    [Route("api/groups")]
    [ApiController]
    [Tags("Groups")]
    public class GroupsController : Controller
    {
        private readonly GroupBAL _groupBAL;
        private string _controllerName = "";
        private string? _methodName = "";

        /// <summary>
        /// </summary>
        public GroupsController(IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _groupBAL = new GroupBAL(httpContextAccessor,mapper);
        }

        /// <summary>
        /// Fetch all group records.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<GroupWithCategoryViewModel>>> Get()
        {
            List<GroupWithCategoryViewModel>? records = null;

            try
            {
                records = await _groupBAL.GetgrouprecordsAsync();
                _controllerName = ControllerContext.ActionDescriptor.ControllerName;
                _methodName = ControllerContext?.RouteData?.Values["action"]?.ToString();
                Log.ForContext("method_name", _methodName)
                   .ForContext("controller_name", _controllerName)
                   .ForContext("thrown_by", "Application")
                   .Information("Run EndPoint Of Getappaccessgrouprecords Method: {TimeofDay}", DateTime.Now.TimeOfDay);
                return records;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        ///Fetch a specific group record by ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<List<GroupWithCategoryViewModel>>> GetById(Guid id)
        {
            GroupWithCategoryViewModel? record = null;
            try
            {
                if (id <= Guid.Empty)
                {
                    return BadRequest();
                }
                else
                {
                    record = await _groupBAL.GetgrouprecordsByIdAsync(id);
                    if (record == null)
                    {
                        return NotFound(new { message = "Record not found." });
                    }
                    _controllerName = ControllerContext.ActionDescriptor.ControllerName;
                    _methodName = ControllerContext?.RouteData?.Values["action"]?.ToString();
                    Log.ForContext("method_name", _methodName)
                       .ForContext("controller_name", _controllerName)
                       .ForContext("thrown_by", "Application")
                       .Information("Run EndPoint Of GetappaccessgrouprecordsById Method: {TimeofDay}", DateTime.Now.TimeOfDay);
                    if (record.Guid_Id == null)
                    {
                        return NotFound("Record Not Found");
                    }
                    return Ok(record);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Fetch a specific group record by GroupType.
        /// </summary>
        /// <param name="groupType"></param>
        /// <returns></returns>
        [HttpGet("by-grouptype/{groupType}")]
        public async Task<ActionResult<List<GroupWithCategoryViewModel>>> GetByGroupType(string groupType)
        {
            List<GroupWithCategoryViewModel>? records = null;

            try
            {
                if (string.IsNullOrWhiteSpace(groupType))
                {
                    return BadRequest(new { message = "GroupType is required." });
                }

                records = await _groupBAL.GetGroupRecordsByGroupTypeAsync(groupType);

                _controllerName = ControllerContext.ActionDescriptor.ControllerName;
                _methodName = ControllerContext?.RouteData?.Values["action"]?.ToString();

                Log.ForContext("method_name", _methodName)
                   .ForContext("controller_name", _controllerName)
                   .ForContext("thrown_by", "Application")
                   .Information("Run EndPoint Of GetGroupRecordsByGroupType Method: {TimeofDay}", DateTime.Now.TimeOfDay);

                return Ok(records);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        ///  Fetch user-group records by multiple group names
        /// </summary>
        /// <param name="groupNames"></param>
        /// <returns></returns>
        [HttpGet("by-group-names")]
        public async Task<ActionResult<List<UserGroupSummaryViewModelDTO>>> GetByGroupNames(string groupNames)
        {
            try
            {
                if (groupNames == null || !groupNames.Any())
                {
                    return BadRequest(new { message = "Group names are required." });
                }
                var groupList = groupNames.Split(',', StringSplitOptions.RemoveEmptyEntries)
                               .Select(x => x.Trim())
                               .ToList();
                var records = await _groupBAL.GetUserGroupRecordsByGroupNamesAsync(groupList);

                _controllerName = ControllerContext.ActionDescriptor.ControllerName;
                _methodName = ControllerContext?.RouteData?.Values["action"]?.ToString();

                Log.ForContext("method_name", _methodName)
                   .ForContext("controller_name", _controllerName)
                   .ForContext("thrown_by", "Application")
                   .Information("Run EndPoint Of GetGroupRecordsByGroupType Method: {TimeofDay}", DateTime.Now.TimeOfDay);

                return Ok(records);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Fetch group names by user id.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("by-user")]
        public async Task<ActionResult<List<GroupSummaryDTO>>> GetGroupNamesByUserId(Guid userId)
        {
            try
            {
                if (userId <= Guid.Empty)
                {
                    return BadRequest(new { message = "Valid userId is required." });
                }

                var records = await _groupBAL.GetGroupNamesByUserIdAsync(userId);

                _controllerName = ControllerContext.ActionDescriptor.ControllerName;
                _methodName = ControllerContext?.RouteData?.Values["action"]?.ToString();

                Log.ForContext("method_name", _methodName)
                   .ForContext("controller_name", _controllerName)
                   .ForContext("thrown_by", "Application")
                   .Information("Run EndPoint Of GetGroupNamesByUserId: {TimeofDay}", DateTime.Now.TimeOfDay);

                return Ok(records);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Create a new Group record 
        /// </summary>
        /// <param name="groupModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(GroupModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GroupModel>> Post(GroupModel groupModel)
        {
            try
            {
                if (groupModel == null)
                {
                    BadRequest();
                }
                else
                {
                    await _groupBAL.CreategroupAsync(groupModel);
                    _controllerName = ControllerContext.ActionDescriptor.ControllerName;
                    _methodName = ControllerContext?.RouteData?.Values["action"]?.ToString();
                    Log.ForContext("method_name", _methodName)
                       .ForContext("controller_name", _controllerName)
                       .ForContext("thrown_by", "Application")
                       .Information("Run EndPoint Of Postappaccessgroup Method in : {TimeofDay}", DateTime.Now.TimeOfDay);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return CreatedAtAction(nameof(Get), new { id = groupModel.GuidId }, groupModel);
        }

        /// <summary>
        /// Update an existing group record.
        /// </summary>
        /// <param name="groupModel"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)] // Indicates successful update
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // Indicates invalid input
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Indicates the record was not found
        public async Task<IActionResult> Put(GroupModel groupModel)
        {


            if (groupModel == null)
            {
                return BadRequest();
            }

            try
            {
                await _groupBAL.UpdategroupAsync(groupModel);
                _controllerName = ControllerContext.ActionDescriptor.ControllerName;
                _methodName = ControllerContext?.RouteData?.Values["action"]?.ToString();
                Log.ForContext("method_name", _methodName)
                   .ForContext("controller_name", _controllerName)
                   .ForContext("thrown_by", "Application")
                   .Information("Run EndPoint Of Putappaccessgroup Method in : {TimeofDay}", DateTime.Now.TimeOfDay);
                return Ok(groupModel);
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// Delete a group record by ID.
        /// </summary>
        /// <param name = "id" ></ param >
        /// < returns ></ returns >
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)] // Indicates successful deletion
        [ProducesResponseType(StatusCodes.Status404NotFound)] // Indicates the record was not found
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    return BadRequest("id not exists to delete the record");
                }
                else
                {
                    var response = await _groupBAL.DeletegroupAsync(id);
                    _controllerName = ControllerContext.ActionDescriptor.ControllerName;
                    _methodName = ControllerContext?.RouteData?.Values["action"]?.ToString();
                    Log.ForContext("method_name", _methodName)
                       .ForContext("controller_name", _controllerName)
                       .ForContext("thrown_by", "Application")
                       .Information("Run EndPoint Of Deleteappaccessgroup Method in : {TimeofDay}", DateTime.Now.TimeOfDay);
                    return Ok(response);
                }

            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
