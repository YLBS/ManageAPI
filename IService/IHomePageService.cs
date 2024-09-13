using Model;
using System;
using System.Collections.Generic;
namespace IService
{
    public interface IHomePageService
    {
        /// <summary>
        /// 登录认证
        /// </summary>
        /// <returns></returns>
        Task<(string validCode, DateTime loginTime)> GetValidCode(string userName);

        Task<IEnumerable<UserMenuDto>> GetUserMenu(int userId);
        /// <summary>
        /// 获取用户部分信息,传账号
        /// </summary>
        /// <param name="userName">这个是账号！！不是姓名</param>
        /// <returns></returns>
        Task<LoginUserInfo> GetUserInfo(string userName);
    }
}
