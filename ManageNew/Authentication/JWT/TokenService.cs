using Microsoft.IdentityModel.Tokens;
using Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Common.ConfigOptions;
using Common.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Data;
using ServiceStack;

namespace ManageNew.Authentication.JWT
{
    /// <summary>
    /// Token服务
    /// </summary>
    public class TokenService : ITokenService
    {
        private IOptions<JwtAuthOptions> _jwtAuthOptions;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="jwtAuthOptions"></param>
        public TokenService(IOptions<JwtAuthOptions> jwtAuthOptions)
        {
            _jwtAuthOptions = jwtAuthOptions;
        }
        /// <summary>
        /// 生成token
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<Token> IssueTokenAsync(LoginUserInfo loginUserInfo)
        {
            var nowTime = DateTime.Now;
            var expiresTime = nowTime.AddSeconds(10); //有效期4小时
            var refreshTokenExpires = nowTime.AddHours(_jwtAuthOptions.Value.RefreshTokenExpires); 
            List<Claim> claims = new List<Claim>
            {
                new ("loginTime", nowTime.ToUnixTimeStampSecond().ToString()),
                new ("refreshTokenExpires", refreshTokenExpires.ToUnixTimeStampSecond().ToString()),
                new ("userName", loginUserInfo.trueName),//真实姓名
                new ("userId", loginUserInfo.UserId.ToString()),
                new ("account", loginUserInfo.Username),//这个才是账号
            };
            foreach (var role in loginUserInfo.Roles)
            {
                //无语，竟然会有NULL 报错了。
                if(!string.IsNullOrEmpty(role))
                    claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtAuthOptions.Value.SecurityKey));

           

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _jwtAuthOptions.Value.Issuer,
                audience: _jwtAuthOptions.Value.Audience,
                claims: claims,
                expires: expiresTime, 
                notBefore: nowTime,
                signingCredentials: creds);
            string returnToken = new JwtSecurityTokenHandler().WriteToken(token);

            return await Task.FromResult(new Token()
            {
                AccessToken = returnToken,
                Expires = expiresTime
            });
        }
        /// <summary>
        /// 读取token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<JwtSecurityToken> ReadJwtToken(string token)
        {
            token = token.Replace("Bearer", "").Trim();
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            if (jwtSecurityTokenHandler.CanReadToken(token))
            {
                var signinCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtAuthOptions.Value.SecurityKey)),
                        SecurityAlgorithms.HmacSha256);
                JwtSecurityToken jwtSecurityToken = jwtSecurityTokenHandler.ReadJwtToken(token);
                var rawSignature = JwtTokenUtilities.CreateEncodedSignature(
                    jwtSecurityToken.RawHeader + "." + jwtSecurityToken.RawPayload,
                    signinCredentials);
                if (jwtSecurityToken.RawSignature == rawSignature)
                {
                    return await Task.FromResult(jwtSecurityToken);
                }
            }
            return null;
        }
    }
}
