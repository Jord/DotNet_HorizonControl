namespace HorizonControlCenterWebAPI.Security
{
    public class AuthUserContext : IAuthUserContext
    {
        public AuthUser CurrentUser { get; set; }
    }
}
