using Commons.Extensions;
using IService;
using ManageNew.Authentication.JWT;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Model;

namespace ManageNew.Controllers
{
    /// <summary>
    /// 授权、刷新token
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "登录认证")]
    public class AuthController : ControllerBase
    {
        #region 依赖注入
        private readonly IHomePageService _userRole;
        private readonly ITokenService _tokenService;
        private readonly IMemoryCache _cache;
        /// <summary>
        /// 构造方法
        /// </summary>
        public AuthController(IHomePageService userRole, ITokenService tokenService, IMemoryCache cache)
        {
            _userRole = userRole;
            _tokenService = tokenService;
            _cache = cache;
        }

        #endregion

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> VerifyLogin([FromBody] LoginInfo login)
        {
            DateTime loginTime = DateTime.MinValue;
            //string dbValidCode = _userRole.GetValidCode(userName, out loginTime);
            //if (string.IsNullOrEmpty(dbValidCode))
            //{
            //    return Ok(ResultMode<string>.NotFound("身份验证失败"));
            //}

            //if (code != EnAndDecryption.Md5Decryption(dbValidCode))
            //{
            //    return Ok(ResultMode<string>.Failed("身份验证信息无效"));
            //}
            //
            //TimeSpan timeSpan = DateTime.Now - loginTime;
            //if (timeSpan.Minutes > 60)
            //{
            //    return Ok(ResultMode<string>.Failed("您的身份验证信息已经过期"));
            //}
            LoginUserInfo loginUserInfo = new LoginUserInfo();
            loginUserInfo.Username = login.UserName;
            loginUserInfo.Roles = new[] { login.Code, "角色" };
            loginUserInfo.trueName = "123";
            var s = await _tokenService.IssueTokenAsync(loginUserInfo);
            return Ok(ResultMode<Token>.Success(s));

        }
        /// <summary>
        /// 登录，返回token
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Login(string userName, string code)
        {
            //var result = await _userRole.GetValidCode(userName);
            //string dbValidCode = result.validCode;
            //DateTime loginTime = result.loginTime;
            //if (string.IsNullOrEmpty(dbValidCode))
            //{
            //    return Ok(ResultMode<string>.NotFound("身份验证失败"));
            //}

            //if (code != EnAndDecryption.Md5Decryption(dbValidCode))
            //{
            //    return Ok(ResultMode<string>.Failed("身份验证信息无效"));
            //}
            //TimeSpan timeSpan = DateTime.Now - loginTime;
            //if (timeSpan.Minutes > 1)
            //{
            //    return Ok(ResultMode<string>.Failed("身份验证信息已经过期"));
            //}
            //LoginUserInfo loginUserInfo = await _userRole.GetUserInfo(userName);
            //var s = await _tokenService.IssueTokenAsync(loginUserInfo);
            ////将用户信息缓存起来
            //string cacheKey = "userInfo-" + loginUserInfo.Username;
            //_cache.Set(cacheKey, loginUserInfo);
            var s = await Get(userName);
            return Ok(ResultMode<Token>.Success(s));
        }

        /// <summary>
        /// 刷新token
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> RefreshToken()
        {
            //if (string.IsNullOrEmpty(token))
            //{
            //    return Ok(ResultMode<string>.Failed("Token已丢失"));
            //}
            var account = User.Claims.FirstOrDefault(c => c.Type == "account")?.Value;
            if (account != null)  //Token未过期，重新生成token返回
            {
                var s = await Get(account);
                return Ok(ResultMode<Token>.Success(s));
            }
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            //string c = cookieArgs.Replace(" ", "=");
            //解析过期token
            var jwtSecurityToken = await _tokenService.ReadJwtToken(token);
            if (jwtSecurityToken != null)
            {
                account = jwtSecurityToken.Claims
                    .FirstOrDefault(s => s.Type == "account")?.Value;
                var i = Convert.ToInt32(jwtSecurityToken.Claims
                    .FirstOrDefault(s => s.Type == "refreshTokenExpires")?.Value);
                var j = Convert.ToInt32(DateTime.Now.AddDays(1).ToUnixTimeStampSecond().ToString());
                if (i > j) //允许刷新token时间内
                {

                    //根据用户再查一遍用户信息，重新生成token返回
                    var s = await Get(account);
                    return Ok(ResultMode<Token>.Success(s));
                    //return Ok(jwtSecurityToken);
                }
                return Ok(ResultMode<string>.Failed("已超过刷新时间，请重新登录"));
            }
            return Ok(ResultMode<string>.Failed("token验证失败，请重新登录"));
        }

        private async Task<Token> Get(string userName)
        {
            LoginUserInfo loginUserInfo = await _userRole.GetUserInfo(userName);
            var s = await _tokenService.IssueTokenAsync(loginUserInfo);
            //将用户信息缓存起来
            string cacheKey = "userInfo-" + loginUserInfo.Username;
            _cache.Set(cacheKey, loginUserInfo);
            return s;
        }

    }
}
