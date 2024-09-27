using IService.ResumeSearch;
using Model.ResumeSearch;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Entity.Goodjob;
using Microsoft.EntityFrameworkCore;

namespace Service.ResumeSearch
{
    public class GEService: IGEService
    {
        private readonly GoodjobContext _context;
        public GEService(GoodjobContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<GEResumeInfo>> GE_GetResumesList(string filter, string order)
        {
            //只查询前1000条数据
            var param = new { filter, orderBy = order };
            var result = await _context.Database.GetDbConnection().QueryAsync<GEResumeInfo>("GE_Get_ResumesList", param, commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<GEResumeInfo> GE_GetResumeInfoById(int id)
        {

            var param = new { ID= id };
            var result = await _context.Database.GetDbConnection().QueryFirstOrDefaultAsync<GEResumeInfo>("GE_Get_ResumeInfoById", param, commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<int> GetCountByFilter(string filter)
        {
            string sql = "select count(*) FROM [Goodjob].[dbo].[GE_Resume] inner join Goodjob.dbo.Mem_Position on [Goodjob].[dbo].[GE_Resume].[PosID]= Goodjob.dbo.Mem_Position.[PosID]  where 1=1 "+ filter;

            var result = await _context.Database.GetDbConnection().QueryFirstOrDefaultAsync<int>(sql);
            return result;
            //throw new NotImplementedException();
        }

        public async Task<int> Del_GEResumeByIds(string isd)
        {
            //原存储过程 GE_Del_ResumeByIds
            string sql = "delete from Goodjob.dbo.GE_Resume  where id in(" + isd + ")";

            var result = await _context.Database.GetDbConnection().QueryFirstOrDefaultAsync<int>(sql);
            return result;
        }
    }
}
