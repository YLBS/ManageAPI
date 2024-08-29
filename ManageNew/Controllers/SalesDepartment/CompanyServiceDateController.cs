using Common.Cache;
using IService;
using IService.SalesDepartment;
using ManageNew.Authentication.JWT;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
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

        private readonly IMemoryCache _cache;
        private readonly IHomePageService _userRole;
        private readonly ICompanyServiceDate _companyService;
        /// <summary>
        /// 构造方法
        /// </summary>

        public CompanyServiceDateController(IMemoryCache cache, IHomePageService userRole, ICompanyServiceDate company)
        {
            _cache = cache;
            _userRole = userRole;
            _companyService = company;
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
                return Ok(ResultMode<string>.Failed("身份验证信息已过期"));
            }
            string cacheKey = "userInfo-"+ account;
            var userInfo = _cache.Get<LoginUserInfo>(cacheKey);
            if (userInfo == null) //缓存丢失
            {
                //重新获取
                userInfo = await _userRole.GetUserInfo(account);
                if (userInfo == null)
                {
                    return Ok(ResultMode<string>.NotFound("用户不存在"));
                }
                _cache.Set(cacheKey, userInfo);
            }
            return Ok(ResultMode<object>.Success(userInfo.DeptName));
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
                return Ok(ResultMode<string>.Failed("身份验证信息已过期"));
            }
            string cacheKey = "userInfo-" + account;
            var userInfo = _cache.Get<LoginUserInfo>(cacheKey);
            if (userInfo == null) //缓存丢失
            {
                //重新获取
                userInfo = await _userRole.GetUserInfo(account);
                if (userInfo == null)
                {
                    return Ok(ResultMode<string>.NotFound("用户不存在"));
                }
                _cache.Set(cacheKey, userInfo);
            }

            if (deptId == 0)
            {
                return Ok(ResultMode<object>.Success(userInfo.DeptUsers));
            }

            var s = userInfo.DeptUsers.Where(d => d.DeptID == deptId);
            return Ok(ResultMode<object>.Success(userInfo.DeptUsers.Where(d=>d.DeptID == deptId)));
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
