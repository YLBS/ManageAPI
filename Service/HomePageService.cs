using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.GoodBoss;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Dapper;
using Model;
using AutoMapper;
using IService;

namespace Service
{
    public class HomePageService : IHomePageService
    {
        private readonly GoodBossContext _context;
        private readonly IMapper _mapper;

        public HomePageService(GoodBossContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<LoginUserInfo> GetUserInfo(string userName)
        {
            var dt = await _context.ErmEmployees.Where(e => e.UserName == userName).FirstOrDefaultAsync();
            if (dt != null)
            {
                int userid = dt.EplId;
                var list = await (from a in _context.SysUserInRoles
                                  join b in _context.SysRoles on a.RoleId equals b.RoleId
                        into tableGroup1
                                  from b in tableGroup1.DefaultIfEmpty()
                                  where a.UserId == userid
                                  select b.Name).ToListAsync();

                var loginUserInfo = _mapper.Map<LoginUserInfo>(dt);
                loginUserInfo.Roles = list.ToArray();

                var parameters = new { UserID = userid, Username = userName };
                var depResult = await _context.Database.GetDbConnection().QueryAsync<LoginUserInfo.ManagerDept>("Sys_Mana_GetManagerDept", parameters, commandType: CommandType.StoredProcedure);
                loginUserInfo.DeptName = depResult;

                var depUserResult = await _context.Database.GetDbConnection().QueryAsync<LoginUserInfo.ManagerDeptUser>("Sys_Mana_GetManagerDeptUser", parameters, commandType: CommandType.StoredProcedure);
                loginUserInfo.DeptUsers = depUserResult;
                return loginUserInfo;
            }
            return null;
        }

        public async Task<IEnumerable<UserMenuDto>> GetUserMenu(int userId)
        {
            var paras = new { UserID = userId };
            var list1 = await _context.Database.GetDbConnection()
                .QueryAsync<UserMenuDto>("Sys_Mana_GetUserMenu", paras, commandType: CommandType.StoredProcedure);

            return list1;
        }

        public async Task<(string validCode, DateTime loginTime)> GetValidCode(string userName)
        {
            string validCode = "";
            var loginTime = DateTime.MinValue;
            var data = await _context.Set<SysGoodjobLoginLog>().Where(s => s.UserName == userName && s.State == 0).FirstOrDefaultAsync();
            if (data != null)
            {
                validCode = data.Randomchar;
                loginTime = (DateTime)data.LoginTime;
            }
            return (validCode, loginTime);
        }
    }
}
