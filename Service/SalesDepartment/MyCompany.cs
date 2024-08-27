using Entity.Goodjob;
using IService.SalesDepartment;
using Model.SalesDepartment;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Entity.GoodBoss;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;
using PubInformation = Entity.Goodjob.PubInformation;

namespace Service.SalesDepartment
{
    public class MyCompany : IMyCompany
    {
        private readonly GoodjobContext _context;
        private readonly GoodBossContext _bossContext;
        private readonly IMapper _mapper;
        public MyCompany(GoodjobContext context,IMapper mapper, GoodBossContext goodBoss)
        {
            _context = context;
            _mapper = mapper;
            _bossContext = goodBoss;
        }

        public async Task<(IEnumerable<MySalesCompanyListInfo> item, int count)> GetData(string filter)
        {
            var parm = new { Filter = filter };
            var multiReader = await _context.Database.GetDbConnection().QueryMultipleAsync("Sales_GetCompanyList_New", parm, commandType: CommandType.StoredProcedure);
            var count = multiReader.Read<int>().ToList().FirstOrDefault();
            if (count == 0)
            {
                return (null, 0);
            }
            var result = multiReader.Read<MySalesCompanyListInfo>();
            multiReader.Dispose();
            return (result, count);
        }
      
        /// <returns></returns>
        public async Task<bool> AddPubInformation(InputPubInformationDto dto)
        {
            var result = _mapper.Map<PubInformation>(dto);
            await _context.PubInformations.AddAsync(result);
            return await _context.SaveChangesAsync() > 0;
        }

        public async  Task<bool> EnableViewCompany(int memId,int userId,int[] departUserId)
        {
            //原执行存储过程Sales_EnableViewCompany
            var r1 = await _context.MemUsers.Where(m =>
                (m.SalerUserId == 0 && m.MemId == memId) || (m.MemId == memId && m.SalerUserId == userId ||
                                                             (m.MemId == memId &&
                                                              departUserId.Contains(m.SalerUserId)))).FirstOrDefaultAsync();
            return r1 != null;
        }

        public async Task<IEnumerable<SectionModel>> GetUserSectionDictionary(int userId)
        {
            var parameters = new { UserID = userId};
            var result = await _bossContext.Database.GetDbConnection().QueryAsync<SectionModel>("Sys_Mana_GetUserSection", parameters, commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<IEnumerable<CompanyInfo>> GetCompanyById(int memId)
        {
            var parameters = new { MemID = memId };
            var result = await _context.Database.GetDbConnection().QueryAsync<CompanyInfo>("Mng_GetCompanyById", parameters, commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<IEnumerable<CompanyServiceInfo>> GetCompanyServiceById(int memId)
        {
            var parameters = new { MemID = memId };
             var result = await _context.Database.GetDbConnection().QueryAsync<CompanyServiceInfo>("Mng_GetCompanyServiceByID", parameters, commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<(string userName, string passWord)> GetMemUserNameAndPassWord(int memId)
        {
            string sql = $" select UserName,PassWord from Mem_Users where MemID={memId}";
            var result = await _context.Database.GetDbConnection().QueryFirstOrDefaultAsync<(string userName, string passWord)>(sql);
            return result;
        }

        public async Task<bool> SendPasswordLink(int memId, string sid)
        {
            var l=await _context.MemUsers.Where(m => m.MemId == memId).FirstOrDefaultAsync();
            if (l == null)
            {
                return false;
            }
            var parameters = new { MemID = memId, EncryptionStr = sid };
            var result = await _context.Database.GetDbConnection().ExecuteAsync("SendPasswordLink", parameters, commandType: CommandType.StoredProcedure);
            return result > 0;
            //var list = await _context.MemBackPasswords.Where(m => m.MemId == memId).FirstOrDefaultAsync();
            //if (list == null)
            //{
            //    MemBackPassword m = new MemBackPassword();
            //    m.MemId = memId;
            //    m.EncryptionStr = sid;
            //    m.SendTime= DateTime.Now;
            //    m.Flag = false;
            //    _context.MemBackPasswords.Add(m);
            //    return await _context.SaveChangesAsync() > 0;
            //}
            //list.EncryptionStr = sid;
            //list.SendTime = DateTime.Now;
            //list.Flag = true;
            //_context.Update(list);
            //return await _context.SaveChangesAsync() > 0;
        }
    }
}
