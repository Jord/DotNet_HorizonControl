namespace HorizonControlCenterModels.Custom
{
    public class ActionResponseModel
    {
        public string? Status { get; set; }

        public string? SuccessMessage { get; set; }

        public List<ErrorModel>? Errors { get; set; }
        // exception handling in entity framework core

        public string? ReturnMessage { get; set; }

        public int ReturnID { get; set; }

        public Object? ReturnObject { get; set; }

        public string? StatusCode { get; set; }

        public ActionResponseModel(string Status, string ReturnMessage = "", List<ErrorModel>? Errors = null, int ReturnID = 0, Object? ReturnObject = null, string StatusCode = "")
        {
            this.Status = Status;
            this.ReturnMessage = ReturnMessage;
            this.Errors = Errors;
            this.ReturnID = ReturnID;
            this.ReturnObject = ReturnObject;
            this.StatusCode = StatusCode;
        }
        public ActionResponseModel()
        {
        }
    }
}
