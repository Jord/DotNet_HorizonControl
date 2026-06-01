using HorizonControlCenterModels.Custom;
using Microsoft.AspNetCore.Mvc;

namespace HorizonControlCenterWebAPI
{
    public class ErrorHandler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objARM"></param>
        /// <returns></returns>
        public static ActionResult HandleErrorResponse(ActionResponseModel objARM)
        {
            if (objARM.Status == "FAILURE")
            {
                if (objARM.Errors != null && objARM.Errors.Any())
                {
                    var errorMessages = objARM.Errors.Select(e => e.ExceptionMessage).ToList();
                    return new BadRequestObjectResult(new { message = "Validation errors occurred.", errors = errorMessages });
                }
                else if (objARM.ReturnMessage.Contains("already exists"))
                {
                    return new ConflictObjectResult(new { message = objARM.ReturnMessage });
                }
                else if (objARM.ReturnMessage.Contains("Not Found"))
                {
                    return new NotFoundObjectResult(new { message = objARM.ReturnMessage });
                }
                //else if(objARM.ReturnMessage.Contains("VALIDATION ERROR"))
                //{
                //    return new BadRequestObjectResult(new{ message = objARM.Errors });
                //}
                else
                {
                    return new BadRequestObjectResult(new { message = objARM.ReturnMessage });
                }
            }

            return new OkObjectResult(objARM);
        }
    }
}
