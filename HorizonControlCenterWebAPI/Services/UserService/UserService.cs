using HorizonControlCenterWebAPI.Security;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace HorizonControlCenterWebAPI.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMemoryCache _cache;
        private readonly IAuthUserContext _authUserContext;

        public string[]? Roles { get; set; }

        public UserService(
             IHttpContextAccessor httpContextAccessor,
             IMemoryCache cache,
             IAuthUserContext authUserContext
         )
        {
            this._httpContextAccessor = httpContextAccessor;
            _cache = cache;
            _authUserContext = authUserContext;
        }
        public int GetGlobalUserID()
        {
            var result = string.Empty;
            if (_httpContextAccessor.HttpContext != null)
            {
                result = _httpContextAccessor.HttpContext.User.FindFirstValue("GlobalUserID");
            }
            if (result == null) { return 0; }
            else { return int.Parse(result); }
        }

        public string GetUserEmail()
        {
            var result = string.Empty;
            if (_httpContextAccessor.HttpContext != null)
            {
                result = CryptoUtill.DecryptStringAES(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email));
            }
            return result;
        }

        public string GetUserFullName()
        {
            var result = string.Empty;
            if (_httpContextAccessor.HttpContext != null)
            {
                result = CryptoUtill.DecryptStringAES(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name));
            }
            return result;
        }

        public AuthUser GetUserInfo()
        {
            if (_httpContextAccessor.HttpContext != null)
            {
                return new AuthUser
                {
                    UserName = GetUserName(),
                    UserFullName = GetUserFullName(),
                    UserEmail = GetUserEmail(),
                    UserId = GetGlobalUserID(),
                    Roles = GetRoles(),
                };
            }
            else
                return _authUserContext.CurrentUser;
        }

        public string GetUserName()
        {
            var result = string.Empty;
            if (_httpContextAccessor.HttpContext != null)
            {
                result = CryptoUtill.DecryptStringAES(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            }
            return result;
        }

        public string[]? GetRoles()
        {
            if (_httpContextAccessor.HttpContext != null)
            {
                var rolesClaimValues = _httpContextAccessor.HttpContext.User.FindAll(ClaimTypes.Role);
                if (rolesClaimValues != null && rolesClaimValues.Any())
                {
                    return rolesClaimValues.Select(c => c.Value).ToArray();
                }
            }
            return null;
        }
    }
}
