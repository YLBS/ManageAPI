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

        
    }
}
