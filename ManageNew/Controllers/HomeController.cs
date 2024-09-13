
using ManageNew.Tool;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace ManageNew.Controllers
{
    /// <summary>
    /// 主页的API
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "主页")]
    public class HomeController : ControllerBase
    {
        #region 依赖注入
        private readonly ManageUserCache _userCache;
        /// <summary>
        /// 构造方法
        /// </summary>
        public HomeController(ManageUserCache manageUserCache)
        {
            _userCache = manageUserCache;
        }
        
        #endregion
        


        /// <summary>
        /// 主页获取菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetHeaderMenu()
        {
            var userId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value);
            if (userId == 0)
            {
                return Unauthorized(ResultMode<string>.Failed("身份验证信息已经过期"));
            }
            var ss = await _userCache.GetUserMenu(userId);
            var s = ss.Where(u => u.Depth == 1);
            return Ok(ResultMode<Object>.Success(s));

            //return Ok(result.Where(u=>u.Depth==1));
        }
        /// <summary>
        /// 获取左侧菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetLeftMenu(int parentId)
        {
            var userId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value);
            if (userId == 0)
            {
                return Unauthorized(ResultMode<string>.Failed("身份验证信息已经过期"));
            }
            var ss = await _userCache.GetUserMenu(userId);
            var s = ss.Where(u => u.ParentID == parentId);
            return Ok(ResultMode<object>.Success(s));
        }
        /// <summary>
        /// 获取菜单名称
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetMenuName(int menuId)
        {
            var userId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value);
            if (userId == 0)
            {
                return Unauthorized(ResultMode<string>.Failed("身份验证信息已经过期"));
            }

            var ss = await _userCache.GetUserMenu(userId);
            var s = ss.Where(u => u.MenuID == menuId).SingleOrDefault();
            if (s != null)
            {
                return Ok(ResultMode<string>.Success(s.MenuName));
            }
            return Ok(ResultMode<string>.Failed("菜单不存在"));
        }
        /// <summary>
        /// 获取当前用户名
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetUserName()
        {
            var userName = User.Claims.FirstOrDefault(c => c.Type == "userName")?.Value;
            if (userName == null)
            {
                return Unauthorized(ResultMode<string>.Failed("身份验证信息已经过期"));
            }
            return Ok(ResultMode<string>.Success(userName));
        }
        /// <summary>
        /// 获取当前用户Id
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetUserId()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            if (userId == null)
            {
                return Unauthorized(ResultMode<string>.Failed("身份验证信息已经过期"));
            }
            return Ok(ResultMode<string>.Success(userId));
        }
    }
}
