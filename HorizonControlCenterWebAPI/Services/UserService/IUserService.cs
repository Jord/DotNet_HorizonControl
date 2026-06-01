
using HorizonControlCenterWebAPI.Security;

namespace HorizonControlCenterWebAPI.Services.UserService
{
    public interface IUserService
    {
        int GetGlobalUserID();
        string GetUserName();
        string GetUserFullName();
        string GetUserEmail();
        AuthUser GetUserInfo();

    }
}
