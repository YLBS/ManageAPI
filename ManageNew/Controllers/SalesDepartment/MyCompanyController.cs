
using Commons.Tool;
using IService.SalesDepartment;
using ManageNew.Tool;
using Microsoft.AspNetCore.Mvc;
using Model;
using Model.enums;
using Model.SalesDepartment;
using static ServiceStack.Diagnostics.Events;

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
        private readonly CheckPermission _cacheMange;
        private readonly ManageUserCache _userCache;
        /// <summary>
        /// 构造方法
        /// </summary>
        public MyCompanyController(IMyCompany company,CheckPermission memoryCache, ManageUserCache manageUserCache)
        {
            _company= company;
            _cacheMange = memoryCache;
            _userCache = manageUserCache;
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
                return Unauthorized(ResultMode<string>.Failed("身份验证信息已经过期"));
            }
            filter += "and SalerUserID = "+ userId;
            var result = await _company.GetData(filter);
            return Ok(ResultMode<object>.Success(new { Data = result.item, Count = result.count }));
        }
        /// <summary>
        /// ViewCompany.aspx 检测用户是否有查看该企业的权限，Code=0 有权限
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> EnableViewCompany(int memId)
        {
            var account = User.Claims.FirstOrDefault(c => c.Type == "account")?.Value; 
            var userId = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(account))
            {
                return Unauthorized(ResultMode<string>.Failed("身份验证信息已经过期"));
            }

            var result1 = await _userCache.GetDeptUsers(0,account);
            var depUserId = result1.Select(d => d.ClerkID).ToArray();
            
            var result = await _company.EnableViewCompany(memId, Convert.ToInt32(userId), depUserId);
            if (result)
            {
                return Ok(ResultMode<bool>.Success(result));
            }
            return Ok(ResultMode<string>.Failed("无权限"));
        }
        /// <summary>
        /// SendMessage.aspx 給企业留言
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddSendMessage([FromBody] InputPubInformationDto informationDto)
        {
            var userName = User.Claims.FirstOrDefault(c => c.Type == "userName")?.Value;
            var userIdStr = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value; 
            if (userIdStr == null || userName == null)
            {
                return Unauthorized(ResultMode<string>.Failed("身份验证信息已经过期"));
            }

            informationDto.Issuer = userName;
            int.TryParse(userIdStr, out int userId);
            informationDto.SalerId = userId;
            await Task.Delay(1);
            var result= await _company.AddPubInformation(informationDto);
            if (result)
            {
                return Ok(ResultMode<string>.Success("发送成功", "发送成功"));
            }
            return Ok(ResultMode<string>.Failed("发送失败"));

        }

        /// <summary>
        /// 检查权限—企业信息查看,返回true为有权限
        /// 查看企业服务资料
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> CheckCompanyViewPermission()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            var result = await _cacheMange.CheckCompanyViewPermission(CompanyViewEnum.ViewCompanyServiceInfo, Convert.ToInt32(userId));
            return Ok(ResultMode<bool>.Success(result));
        }
        /// <summary>
        /// 获取企业信息，ViewCompany.aspx 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetCompanyById(int memId)
        {
            var result =await _company.GetCompanyById(memId);
            await Task.Delay(1);
            return Ok(ResultMode<object>.Success(result));
        }

        /// <summary>
        /// 获取企业服务信息，ViewCompany.aspx 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetCompanyServiceById(int memId)
        {
            var result = await _company.GetCompanyServiceById(memId);
            await Task.Delay(1);
            return Ok(ResultMode<object>.Success(result));
        }
        /// <summary>
        /// 检查权限—查看企业账号密码,返回true为有权限 ViewCompany.aspx 
        /// 查看企业服务资料
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> CheckViewCompanyUsername()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            var result = await _cacheMange.CheckCompanyViewPermission(CompanyViewEnum.ViewCompanyUsername, Convert.ToInt32(userId));
            return Ok(ResultMode<bool>.Success(result));
        }
        /// <summary>
        /// 获取企业的账号密码 ViewCompany.aspx 
        /// </summary>
        /// <param name="memId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetMemUserNameAndPassWord(int memId)
        {
            var userIdStr = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            var userName = User.Claims.FirstOrDefault(c => c.Type == "userName")?.Value;
            var ip = ManageIP.GetIP();
            var result = await _company.GetMemUserNameAndPassWord(memId, userIdStr, userName, ip);
            
            string str=result.userName+"|"+result.passWord;
            return Ok(ResultMode<string>.Success(str));
        }
        /// <summary>
        /// 获取企业的账号密码,返回json
        /// </summary>
        /// <param name="memId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetMemUserNameAndPassWord_N(int memId)
        {
            var userIdStr = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            var userName = User.Claims.FirstOrDefault(c => c.Type == "userName")?.Value;
            var ip = ManageIP.GetIP();
            var result = await _company.GetMemUserNameAndPassWord(memId, userIdStr, userName, ip);

            return Ok(ResultMode<object>.Success(new{ result.userName,result.passWord }));
        }

        /// <summary>
        /// ViewCompany.aspx  获取sid;
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetSid(int memId)
        {
            string keyString = "zhangxianbingoodjob";
            string sid = EnAndDecryption.Md5Decryption(memId + keyString + DateTime.Now.ToLongTimeString(), 32);
            return Ok(ResultMode<string>.Success(sid));
        }

        /// <summary>
        /// ViewCompany.aspx 发送邮件时调用,修改 Mem_BackPassword
        /// </summary>
        /// <param name="memId"></param>
        /// <param name="sid"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> SendPasswordLink(int memId,string sid)
        {
            var r=await _company.SendPasswordLink(memId,sid);
            return Ok(ResultMode<bool>.Success(r));
        }

        /// <summary>
        /// ViewCompany.aspx 查看联系电话时，添加日志
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult AddLog(int memId,string memName)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            var userName = User.Claims.FirstOrDefault(c => c.Type == "userName")?.Value;
            var ip = ManageIP.GetIP();
            
            //var forwardedFor = Request.Headers["X-Forwarded-For"];
            //string clientIp = forwardedFor.FirstOrDefault() ?? Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            string logInfo = "查看人：" + userId + "-" + userName + " 查看企业：" + memId + "-" + memName + " 请求IP：" + ip;
            LogConfig.TestSetConfig(logInfo, "查看联系方式");
            return Ok();
        }
        /// <summary>
        /// ViewCompany.aspx 审核通过调用，修改
        /// </summary>
        /// <param name="id">ImageID</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> CheckCompanyImage(int id)
        {
            return Ok();
        }
        /// <summary>
        /// ViewCompany.aspx 检测企业的联系方式是否可见
        /// </summary>
        /// <param name="memId"></param>
        /// <param name="salerUserId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> CheckCompanyContact(int memId,int salerUserId)
        {
            var userIdStr = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            int.TryParse(userIdStr, out int userId);
            if (userId == 1937 || userId == 1972)
            {
                return Ok(ResultMode<bool>.Success(true));
            }
            var collId = await _company.GetClient_Collaborative(memId, userId, salerUserId);
            if (collId == userId || collId > 0)
            {
                return Ok(ResultMode<bool>.Success(true));
            }
            return Ok(ResultMode<bool>.Success(false));
        }
        /// <summary>
        /// ViewCompany.aspx 检测是否可点击发送短信按钮
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult CheckSMSBut()
        {
            var userIdStr = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            int.TryParse(userIdStr, out int userId);
            if (userId == 1513 || userId == 458 || userId == 72 || userId == 568|| userId == 1566 || userId == 1133)
            {
                return Ok(ResultMode<bool>.Success(true));
            }
            return Ok(ResultMode<bool>.Success(false));
        }
        /// <summary>
        /// 返会营业执照路径
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetCompanyImageInfoList(int top, int memId)
        {
            var result = await _company.GetCompanyImageInfoList(top, memId);
            return Ok(ResultMode<object>.Success(result));
        }
    }
}
