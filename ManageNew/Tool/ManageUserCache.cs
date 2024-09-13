using IService;
using Microsoft.Extensions.Caching.Memory;
using Model;
using System.Security.Principal;
using static Model.LoginUserInfo;

namespace ManageNew.Tool
{
    /// <summary>
    /// 管理用户缓存
    /// </summary>
    public class ManageUserCache
    {
        private readonly IMemoryCache _cache;
        private readonly IHomePageService _userRole;
        /// <summary>
        /// 构造方法
        /// </summary>
        public ManageUserCache(IMemoryCache cache, IHomePageService user)
        {
            _cache = cache;
            _userRole = user;
        }
        /// <summary>
        /// 获取用户的下属部门职员
        /// </summary>
        /// <param name="deptId"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManagerDeptUser>> GetDeptUsers(int deptId, string account)
        {
            var userInfo = await GetUserInfo(account);
            if (deptId == 0)
                return userInfo.DeptUsers;
            return  userInfo.DeptUsers.Where(d => d.DeptID == deptId);
        }
        /// <summary>
        /// 获取下属部门
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManagerDept>> GetDept(string account)
        {
            var userInfo = await GetUserInfo(account);
            return userInfo.DeptName;
        }
        /// <summary>
        /// 根据用户Id获取菜单
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<UserMenuDto>> GetUserMenu(int userId)
        {
            string menuKey = "UserMenu-" + userId;
            if (!_cache.TryGetValue(menuKey, out IEnumerable<UserMenuDto> result))
            {
                result = await _userRole.GetUserMenu(userId);
                _cache.Set(menuKey, result);
            }
            return result;
        }
        private async Task<LoginUserInfo> GetUserInfo(string account)
        {

            string cacheKey = "userInfo-" + account;
            var userInfo = _cache.Get<LoginUserInfo>(cacheKey);
            if (userInfo == null) //缓存丢失
            {
                userInfo = await _userRole.GetUserInfo(account);
                _cache.Set(cacheKey, userInfo);
            }

            return userInfo;
        }
    }
}
