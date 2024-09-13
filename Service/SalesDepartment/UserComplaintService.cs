using IService.SalesDepartment;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Entity.Goodjob;
using Microsoft.EntityFrameworkCore;
using Model.SalesDepartment;

namespace Service.SalesDepartment
{
    public class UserComplaintService : IUserComplaint
    {
        private readonly GoodjobContext _context;

        public UserComplaintService(GoodjobContext context)
        {
            _context = context;
        }


        public async Task<(IEnumerable<UserComplaint> item, int count)> GetUserComplaintListNews(string filter, string order, int top, int totalCount)
        {
            var param = new { Top = $" top {top}", CountToal = totalCount, Filter = filter, Order = order };
            var result = await _context.Database.GetDbConnection().QueryAsync<UserComplaint>("Sales_Get_UserComplaint_ListNews", param, commandType: CommandType.StoredProcedure);
            return (result, result.Count());
        }

        public async Task<IEnumerable<UserComplaint>> GetUserComplaintListByPosId(int posId)
        {
            var param = new { PosID = posId };
            var result = await _context.Database.GetDbConnection()
                .QueryAsync<UserComplaint>("Sales_Get_UserComplaintListByPosID", param,
                    commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<(bool tf, string msg)> GetUserComplaintByPosId(int posId, int sqlType)
        {
            // 这里不需要查询那么多的字段,待优化
            var param = new { PosID = posId, SqlType=sqlType };
            var result = await _context.Database.GetDbConnection()
                .QueryFirstOrDefaultAsync<UserComplaint>("Sales_Get_UserComplaintByPosID", param,
                    commandType: CommandType.StoredProcedure);
            if (result == null)
            {
                return (false, "职位不存在");
            }

            bool tf = result.HasDealWith;
            string str = result.ProcessingDetail;
            if (tf) //已处理的
            {
                return (false, str);
            }

            if (!string.IsNullOrEmpty(str))
            {
                return (false, str);
            }
            return (true, "该投诉尚未处理，若已经处理完毕，请在下面输入处理的详细信息");

        }

        public async Task<int> DealWithUserComplaint(int posId, string processingDetail)
        {
            var s = await _context.PubComplaints.Where(p => p.PosId == posId && p.HasDealWith == false).ToListAsync();
            foreach (var iem in s)
            {
                iem.HasDealWith = true;
                iem.ProcessingDetail = processingDetail;
            }

            return await _context.SaveChangesAsync();
            /*
            var param = new { PosID = posId, SqlType = 2 }; //SqlType 为2 查詢未處理的
            var result = await _context.Database.GetDbConnection()
                .QueryFirstOrDefaultAsync<UserComplaint>("Sales_Get_UserComplaintByPosID", param,
                    commandType: CommandType.StoredProcedure);
            if (result != null)
            {
                
            }

            return 0;*/
        }

        public async Task<IEnumerable<ExplosiveCompany>> GetExplosiveCompanies(string filter)
        {
            string sql =
                $"SELECT mi.MemID ,mi.MemName ,mrr.MemState , mu.LastLoginDate ,mu.EndValidDate ,  (select COUNT(1) from Mem_Position mp where mp.MemID = mi.MemID) as PosCount, (select COUNT(1) from Pub_PerApplyPos ppap where ppap.MemID = mi.MemID and ppap.CommendType <>2 ) as ApplyCount ,  (select COUNT(1) from Mem_ExplosiveProductsResume_Log meprl where meprl.MemId = mi.MemID ) as SimulateCount FROM Mem_RecommendRecord mrr left join Mem_Info mi on mrr.MemID = mi.MemID left join Mem_Users mu on mu.MemID = mi.MemID where 1 = 1 {filter}";
            var result = await _context.Database.GetDbConnection().QueryAsync<ExplosiveCompany>(sql);
            return result;
        }
    }
}
