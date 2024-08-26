using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Entity.Goodjob;
using IService.SalesDepartment;
using Microsoft.EntityFrameworkCore;
using Model.SalesDepartment;

namespace Service.SalesDepartment
{
    public class PosWxPushRecord : IPosWxPushRecord
    {
        private readonly GoodjobContext _context;
        public PosWxPushRecord(GoodjobContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<WxPusRecordInfo>> GetPusRecordInfosAsync(string filter, string elpName)
        {
            string sql =
                @"select a.MemId,b.MemName,a.WxPushContext,SendDateTime,Result,SendType,EplId,EplName from Goodjob.dbo.Mng_WxPusRecord a left join Mem_Info b on a.MemId =b.MemID " + filter + "  order by a.Id  desc";
            var result = await _context.Database.GetDbConnection().QueryAsync<WxPusRecordInfo>(sql);
            if (string.IsNullOrEmpty(elpName))
                return result;
            return result.Where(w => w.SendEplName.Contains(elpName));
        }
    }
}
