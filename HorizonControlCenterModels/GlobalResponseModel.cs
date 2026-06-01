namespace HorizonControlCenterModels
{
    public class GlobalResponseModel<T>
    {
        public string? Status { get; set; }
        public Object? ObjectId { get; set; }
        public T? Object { get; set; }
        public List<Result>? Results { get; set; }

        public GlobalResponseModel(string Status, List<Result>? Results = null, Object? ObjectId = null, T? Obj = default)
        {
            this.Status = Status;
            this.Results = Results;
            this.ObjectId = ObjectId;
            this.Object = Obj;
        }
        public GlobalResponseModel()
        {
        }
    }
}
