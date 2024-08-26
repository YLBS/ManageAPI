
using IService;
using IService.SalesDepartment;
using ManageNew.Controllers.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Model;
using Model.SalesDepartment;

namespace ManageNew.Controllers.SalesDepartment
{
    /// <summary>
    /// 我的企业
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "销售部专用")]
    public class MyCompanyController : ControllerBase
    {
        private readonly IMyCompany _company;
        private readonly IMemoryCache _cache;
        private readonly IHomePageService _userRole;
        /// <summary>
        /// 构造方法
        /// </summary>
        public MyCompanyController(IMyCompany company, IMemoryCache cache, IHomePageService userRole)
        {
            _company= company;
            _cache= cache;
            _userRole= userRole;
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetData(string? filter)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            if (userId == null)
            {
                return Ok(ResultMode<string>.Failed("身份验证信息已经过期"));
            }
            filter += "and SalerUserID = "+ userId;
            var result = await _company.GetData(filter);
            return Ok(ResultMode<object>.Success(new { Data = result.item, Count = result.count }));
        }
        /// <summary>
        /// 检测用户是否有查看该企业的权限，Code=0 有权限
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> EnableViewCompany(int memId)
        {
            var account = User.Claims.FirstOrDefault(c => c.Type == "account")?.Value; 
            var userId = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(account))
            {
                return Ok(ResultMode<string>.Failed("身份验证信息已经过期"));
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

            var depUserId = userInfo.DeptUsers.Select(d => d.ClerkID).ToArray();
            
            var result = await _company.EnableViewCompany(memId, Convert.ToInt32(userId), depUserId);
            if (result)
            {
                return Ok(ResultMode<bool>.Success(result));
            }
            return Ok(ResultMode<string>.Failed("无权限"));
        }
        /// <summary>
        /// 給企业留言
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddSendMessage([FromBody] InputPubInformationDto informationDto)
        {
            var userName = User.Claims.FirstOrDefault(c => c.Type == "userName")?.Value;
            var userIdStr = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value; 
            if (userIdStr == null || userName == null)
            {
                return Ok(ResultMode<string>.Failed("身份验证信息已经过期"));
            }

            informationDto.Issuer = userName;
            int.TryParse(userIdStr, out int userId);
            informationDto.SalerId = userId;
            await Task.Delay(1);
            var result= await _company.AddPubInformation(informationDto);
            if (result)
            {
                return Ok(ResultMode<string>.Success("发送成功"));
            }
            return Ok(ResultMode<string>.Failed("发送失败"));

        }
        
        /// <summary>
        /// 检查权限—企业信息查看
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> CheckCompanyViewPermission()
        {
            return Ok();
        }
    }
}
