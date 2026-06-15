namespace HorizonControlCenterWebAPI.Security
{
    public interface IAuthUserContext
    {
        AuthUser CurrentUser { get; set; }
    }
}
