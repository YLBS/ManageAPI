using IService.TalentManagement;
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

namespace Service.TalentManagement
{
    public class CheckComResume : ICheckComResume
    {
        private readonly GoodjobContext _context;

        public CheckComResume(GoodjobContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ResumeListInfo>> GetNewResumeListCollection(string filter, string orderField, string orderType)
        {
            IList<ResumeListInfo> resumelist = new List<ResumeListInfo>();
           
            var param = new { ResumeFilter = filter, UserFilter = "" };

            var ids = await _context.Database.GetDbConnection().QueryAsync<int>("Mng_GetResumeListMainID", param, commandType: CommandType.StoredProcedure);
            string idsStr = string.Join(",", ids);
            var parameters = new { ResumeIDStr = idsStr, @OrderString = $" order by {orderField} {orderType}" };
            var result = await _context.Database.GetDbConnection().QueryAsync<ResumeListInfo>("Mng_GetResumeListMain", parameters, commandType: CommandType.StoredProcedure);
            return result;
            //using (var reader = await _context.Database.GetDbConnection()
            //           .ExecuteReaderAsync("Mng_GetResumeListMain", parameters,
            //               commandType: CommandType.StoredProcedure))
            //{
            //    while (await reader.ReadAsync())
            //    {
            //        ResumeListInfo resumeinfo = new ResumeListInfo(reader.GetInt32(0), reader.GetString(1), (short)reader.GetByte(2), reader.GetString(3), reader.GetInt32(4), reader.GetInt32(5), reader.GetDateTime(6), reader.GetDateTime(7), (short)reader.GetByte(8), (short)reader.GetByte(9), reader.GetInt32(10));
            //        resumelist.Add(resumeinfo);
            //    }
            //}
            return resumelist;
        }
    }
}
