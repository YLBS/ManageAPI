using IService.SalesDepartment;
using Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Goodjob;
using Microsoft.EntityFrameworkCore;
using Model.SalesDepartment;
using Dapper;
using System.Data;

namespace Service.SalesDepartment
{
    public class UseEffectService : IUseEffectService
    {
        private readonly GoodjobContext _context;

        public UseEffectService(GoodjobContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<EffectPositionInfo> effect, int count)> GetOneJobFunctionCollection(PagingModel paging, string filter)
        {
            var param = new { Filter = filter };
            var multiReader = await _context.Database.GetDbConnection().QueryMultipleAsync("Sales_Effect_OneJobFunction", param, commandType: CommandType.StoredProcedure);
            var count = multiReader.Read<int>().ToList().FirstOrDefault();
            if (count == 0)
                return (null, 0);
            var result =  await multiReader.ReadAsync<EffectPositionInfo>();
            multiReader.Dispose();
           // var ss = result.Take(paging.PageSize).Skip(paging.PageIndex);
            return (result,count);
        }

        public async Task<int[]> GetPerApplyPosInfos(DateTime? beginDate, DateTime? endDate, string? memName, int memId)
        {
            int[] result = new int[4];

            string m = "";
            if (!string.IsNullOrEmpty(memName))
            {
                int[] memIds = await _context.MemInfos.Where(m => m.MemName.Contains(memName)).Select(m => m.MemId).Take(1000)
                    .ToArrayAsync();
                if (memIds.Length == 0)
                {
                    return result;
                }
                var list = await _context.PubPerApplyPos
                    .WhereIf(!string.IsNullOrEmpty(beginDate.ToString()), p => p.ReceiveDate.Date >= beginDate)
                    .WhereIf(!string.IsNullOrEmpty(endDate.ToString()), p => p.ReceiveDate.Date <= endDate)
                    .WhereIf(memId != 0, p => p.MemId == memId)
                    .WhereIf(memIds.Length != 0, p => memIds.Contains(p.MemId)).Select(p=>p.CommendType).ToListAsync();
                result[0] = list.Count;
                result[1] = list.Where(p=>p==0).Count();
                result[2] = list.Where(p => p == 2).Count();
                result[3] = list.Where(p => p == 3).Count();
            }
            else
            {
                var list = await _context.PubPerApplyPos
                    .WhereIf(!string.IsNullOrEmpty(beginDate.ToString()), p => p.ReceiveDate.Date >= beginDate)
                    .WhereIf(!string.IsNullOrEmpty(endDate.ToString()), p => p.ReceiveDate.Date <= endDate)
                    .WhereIf(memId != 0, p => p.MemId == memId).Select(p => p.CommendType).ToListAsync();
                result[0] = list.Count;
                result[1] = list.Where(p => p == 0).Count();
                result[2] = list.Where(p => p == 2).Count();
                result[3] = list.Where(p => p == 3).Count();
            }
            

            return result;
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<PubPerApplyPosInfo>> GetPerApplyPosInfos(DateTime? beginDate, DateTime? endDate, string? memName, int memId, int commendType, PagingModel paging)
        {

            string filter = " 1=1 ";
            if (beginDate != null)
                filter += "and CAST(ReceiveDate AS date) >= '" + beginDate + "'";
            if (endDate != null)
                filter += "and CAST(ReceiveDate AS date) <= '" + endDate + "'";
            if (commendType != -1) 
                filter += " and CommendType=" + commendType;
            if (memId != 0)
                filter += " and memId=" + memId;
            if (!string.IsNullOrEmpty(memName))
            {
                int[] memIds = await _context.MemInfos.Where(m => m.MemName.Contains(memName)).Select(m => m.MemId).Take(1000)
                    .ToArrayAsync();
                filter += " and MemId in (" + string.Join(",", memIds) + ")";
            }
            int p = (paging.PageIndex - 1) * paging.PageSize;
            if (p < 0)
                p = 0;
            string sql= "select a.MemId,b.MemName,a.PosID, dbo.[F_Mem_GetLibPosName](a.PosID) as  PosName,ReceiveDate ReceiveDates,CommendType, a.MyUserID,c.PerName,a.MemFlag from (select top(" + paging.PageSize + ") *from Pub_PerApplyPos where Id not in (select top(" + p + ")Id from Pub_PerApplyPos where " + filter + " order by id desc) and " + filter + " order by id desc ) a left join Mem_Info b on a.MemID = b.MemID left join My_Resume c on a.MyUserID = c.MyUserID";
            var r = await _context.Database.GetDbConnection()
                .QueryAsync<PubPerApplyPosInfo>(sql);
            return r;
        }
    }
}
