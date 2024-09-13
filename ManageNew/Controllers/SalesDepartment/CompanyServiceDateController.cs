
using IService.SalesDepartment;
using ManageNew.Tool;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace ManageNew.Controllers.SalesDepartment
{
    /// <summary>
    /// 企业服务期统计
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "销售部专用")]
    public class CompanyServiceDateController : ControllerBase
    {
        private readonly ICompanyServiceDate _companyService;
        private readonly ManageUserCache _userCache;
        /// <summary>
        /// 构造方法
        /// </summary>

        public CompanyServiceDateController(ICompanyServiceDate company,ManageUserCache manageUserCache)
        {
            _companyService = company;
            _userCache = manageUserCache;
        }

        /// <summary>
        /// 从缓存中获取部门信息，
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetDept()
        {
            var account =  User.Claims.FirstOrDefault(c => c.Type == "account")?.Value;
            if (string.IsNullOrEmpty(account)) //token过期
            {
                return Unauthorized(ResultMode<string>.Failed("身份验证信息已过期"));
            }
            var result = await _userCache.GetDept(account);
            return Ok(ResultMode<object>.Success(result));

            //string cacheKey = "userInfo-"+ account;
            //var userInfo = _cache.Get<LoginUserInfo>(cacheKey);
            //if (userInfo == null) //缓存丢失
            //{
            //    //重新获取
            //    userInfo = await _userRole.GetUserInfo(account);
            //    if (userInfo == null)
            //    {
            //        return Ok(ResultMode<string>.NotFound("用户不存在"));
            //    }
            //    _cache.Set(cacheKey, userInfo);
            //}
            //return Ok(ResultMode<object>.Success(userInfo.DeptName));
        }
        /// <summary>
        /// 从缓存中获取部门职员
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetDeptUser(int deptId)
        {
            var account = User.Claims.FirstOrDefault(c => c.Type == "account")?.Value;
            if (string.IsNullOrEmpty(account)) //token过期
            {
                return Unauthorized(ResultMode<string>.Failed("身份验证信息已过期"));
            }
            var result = await _userCache.GetDeptUsers(deptId, account);
            return Ok(ResultMode<object>.Success(result));

            //string cacheKey = "userInfo-" + account;
            //var userInfo = _cache.Get<LoginUserInfo>(cacheKey);
            //if (userInfo == null) //缓存丢失
            //{
            //    //重新获取
            //    userInfo = await _userRole.GetUserInfo(account);
            //    if (userInfo == null)
            //    {
            //        return Ok(ResultMode<string>.NotFound("用户不存在"));
            //    }
            //    _cache.Set(cacheKey, userInfo);
            //}

            //if (deptId == 0)
            //{
            //    return Ok(ResultMode<object>.Success(userInfo.DeptUsers));
            //}
            //var s = userInfo.DeptUsers.Where(d => d.DeptID == deptId);
            //return Ok(ResultMode<object>.Success(userInfo.DeptUsers.Where(d=>d.DeptID == deptId)));
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetData(string? filter)
        {
            var result = await _companyService.GetCompanyServiceDateCollection(filter);
            return Ok(ResultMode<object>.Success(new{Data=result.item,Count=result.count}));
        }
    }
}
