using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HorizonControlCenterModels.Custom
{
    public class Custom
    {
        public static GlobalResponseModel<T> CreateSuccess<T>(
            object objectId,
            T obj,
            string message,
            string code = "200"
        )
        {
            return new GlobalResponseModel<T>(
                Status: code,
                ObjectId: objectId,
                Obj: obj,
                Results: new List<Result>
                {
                    new Result
                    {
                        Code = code,
                        Message = message,
                        Type = "SUCCESS",
                        TimeStamp = DateTime.UtcNow.ToString("o"),
                    },
                }
            );
        }

        public static GlobalResponseModel<T> CreateError<T>(
            string code,
            string message,
            string? details = null,
            string? path = null,
            string? fieldName = null,
            T obj = default,
            object? objectId = null
        )
        {
            return new GlobalResponseModel<T>(
                Status: code,
                ObjectId: objectId,
                Obj: obj,
                Results: new List<Result>
                {
                    new Result
                    {
                        Code = code,
                        Message = message,
                        Type = "ERROR",
                        Details = details,
                        Path = path,
                        FieldName = fieldName,
                        TimeStamp = DateTime.UtcNow.ToString("o"),
                    },
                }
            );
        }

        public static GlobalResponseModel<T> CreateWarning<T>(
            object objectId,
            T obj,
            List<string> warnings
        )
        {
            ///this is method is for creating warnings
            return new GlobalResponseModel<T>(
                Status: "SUCCESS",
                ObjectId: objectId,
                Obj: obj,
                Results: warnings
                    .Select(w => new Result
                    {
                        Message = w,
                        Type = "WARNING",
                        TimeStamp = DateTime.UtcNow.ToString("o"),
                    })
                    .ToList()
            );
        }
    }

}
