using IService;
using ManageNew.Authentication.JWT;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Model;
using System.Net;
using Commons.Tool;

namespace ManageNew.Controllers
{
    /// <summary>
    /// 测试
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "测试用")]
    public class TestController : ControllerBase
    {
        private readonly IMemoryCache _cache;
        private readonly IHomePageService _userRole;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;
        /// <summary>
        /// 构造函数
        /// </summary>
        public TestController(IHomePageService userRole, ITokenService tokenService, IMemoryCache cache, IConfiguration configuration)
        {
            _userRole = userRole;
            _tokenService = tokenService;
            _cache = cache;
            _configuration = configuration;
        }

        /// <summary>
        /// 登录验证
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyLogin([FromBody] LoginInfo login)
        {
            DateTime loginTime = DateTime.MinValue;
            //string dbValidCode = _userRole.GetValidCode(userName, out loginTime);
            //if (string.IsNullOrEmpty(dbValidCode))
            //{
            //    return Unauthorized(ResultMode<string>.NotFound("身份验证失败"));
            //}

            //if (code != EnAndDecryption.Md5Decryption(dbValidCode))
            //{
            //    return Unauthorized(ResultMode<string>.Failed("您的身份验证信息无效"));
            //}
            //
            //TimeSpan timeSpan = DateTime.Now - loginTime;
            //if (timeSpan.Minutes > 60)
            //{
            //    return Unauthorized(ResultMode<string>.Failed("您的身份验证信息已经过期"));
            //}
            LoginUserInfo loginUserInfo = new LoginUserInfo();
            loginUserInfo.Username = login.UserName;
            loginUserInfo.Roles = new[] { login.Code, "角色" };
            loginUserInfo.trueName = "123";
            loginUserInfo.UserId = 123;
            var s = await _tokenService.IssueTokenAsync(loginUserInfo);
            return Ok(ResultMode<Token>.Success(s));
        }
        
        /// <summary>
        /// 测试权限，只有携带token才能访问
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            await Task.Delay(12);
            return Ok(123);
        }
        /// <summary>
        /// 测试权限,拥有角色1的用户才能访问
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = "Role1")]
        public async Task<IActionResult> Get1()
        {
            await Task.Delay(1);
            //var dt = await t.GetMemInfo();
            //return Ok(new{ dt });
            return Ok(1);
        }
        /// <summary>
        /// 测试权限,拥有角色2的用户才能访问
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = "Role2")]
        public async Task<IActionResult> Get2()
        {
            await Task.Delay(1);
            return Ok(32);
            //var dt = await t.GetOutJiuYeStationList();
            //return Ok(new { dt });
        }
        /// <summary>
        /// 测试权限,拥有角色1或者角色2的用户才能访问
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = "Role3")]
        public async Task<IActionResult> Get3()
        {
            await Task.Delay(1);
            return Ok(32);
            //var dt = await t.GetOutJiuYeStationList();
            //return Ok(new { dt });
        }
        /// <summary>
        /// 测试http响应缓存
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ResponseCache(Duration = 1200, Location = ResponseCacheLocation.Client)]
        public async Task<IActionResult> TestHttpCache()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "HrSendEmailToCompany.htm");
            await Task.Delay(1500);
            if (!System.IO.File.Exists(path))
            {
                return NotFound();
            }
            return Unauthorized(1);
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Set1")]
        public async Task<IActionResult> Set1(string cacheKey, string cacheValue)
        {
            await Task.Delay(1);
            var userName = User.Claims.FirstOrDefault(c => c.Type == "Name")?.Value;

            // 设置缓存的选项，如过期时间
            var options = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(10));

            _cache.Set(cacheKey, cacheValue, options);
            return Ok(2);
        }
        /// <summary>
        /// 获取缓存的值
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get2(string cacheKey)
        {
            await Task.Delay(1);
            if (_cache.TryGetValue(cacheKey, out string cachedData))
            {
                return Ok(cachedData);
            }
            return Ok("无");
        }
        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Del(string cacheKey)
        {
            await Task.Delay(1);
            _cache.Remove(cacheKey);
            return Ok(2);
            //var dt = await t.GetOutJiuYeStationList();
            //return Ok(new { dt });
        }
        /// <summary>
        /// 获取本地IP
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetIp()
        {
            string ip = string.Empty;
            IPAddress[] addressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
            for (int i = 0; i < addressList.Length; i++)
            {
                ip = addressList[i].ToString();
            }

            LogConfig.TestSetConfig(ip, "ip1");
            //string clientIP = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            //if (Request.Headers.ContainsKey("X-Forwarded-For"))
            //{
            //    clientIP = Request.Headers["X-Forwarded-For"];
            //}

            //获取发起当前请求的客户端的IP地址
            var forwardedFor = Request.Headers["X-Forwarded-For"];
            string clientIp = forwardedFor.FirstOrDefault() ?? Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            LogConfig.TestSetConfig(clientIp, "ip");

            return Ok(ip + "," + clientIp);
        }
    }
}
