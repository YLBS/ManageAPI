﻿using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ManageNew.Extensions
{
    /// <summary>
    /// JWT认证
    /// </summary>
    public static class JwtAuthentication
    {
        /// <summary>
        /// 添加认证
        /// </summary>
        public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(option =>
                {
                    option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = configuration["JwtAuth:Issuer"],
                        ValidateAudience = true,
                        ValidAudience = configuration["JwtAuth:Audience"],
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtAuth:SecurityKey"])),
                        ClockSkew = TimeSpan.Zero // 可以设置为0，以避免时间偏差
                    };
                });
        }
        /// <summary>
        /// 添加授权策略
        /// </summary>
        public static void AddJwtAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(option =>
            {
                var ss = ClaimTypes.Role;

                option.AddPolicy("Role", policy => policy.RequireRole("角色1", "角色2"));
                
                //option.AddPolicy("Role", policyBuilder =>
                //    policyBuilder.RequireAssertion(context =>
                //        context.User.HasClaim(c => c.Type == "Role")
                //        && context.User.Claims.First(c => c.Type.Equals("Role")).Value == "角色1"));

                option.AddPolicy("Role1", policyBuilder =>
                    policyBuilder.RequireAssertion(context =>
                        context.User.HasClaim(c => c.Type == "Role")
                        && context.User.Claims.First(c => c.Type.Equals("Role")).Value == "角色2"));
            });
        }
    }
}
