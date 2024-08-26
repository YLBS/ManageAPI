using Common.ConfigOptions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ManageNew.Extensions
{
    /// <summary>
    /// jwt中间件
    /// </summary>
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly JwtAuthOptions _jwtAuthOptions;
        public JwtMiddleware(RequestDelegate next, IOptions<JwtAuthOptions> options)
        {
            _jwtAuthOptions = options.Value;
            _next = next;
        }
        /// <summary>
        /// 实行验证
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/api/auth") || context.Request.Path.StartsWithSegments("/api/Test"))
            {
                await _next(context);
                return;
            }

            var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            if (string.IsNullOrEmpty(token))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Missing or invalid token.");
                return;
            }

            try
            {
                var principal = ValidateToken(token);
                if (principal == null)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Invalid token.");
                    return;
                }

                context.User = principal;
            }
            catch (SecurityTokenExpiredException)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Token has expired.");
                return;
            }

            await _next(context);
        }

        private ClaimsPrincipal ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtAuthOptions.SecurityKey));
            var validations = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _jwtAuthOptions.Issuer,
                ValidAudience = _jwtAuthOptions.Audience,
                IssuerSigningKey = key,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, validations, out var validatedToken);
                return principal;
            }
            catch (SecurityTokenExpiredException)
            {
                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
