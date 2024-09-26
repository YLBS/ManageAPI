using IService.TalentManagement;
using Model.TalentManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Entity.Goodjob;
using Microsoft.EntityFrameworkCore;
using Model.Common;

namespace Service.TalentManagement
{
    public class SentHistroy : ISentHistroy
    {
        private readonly GoodjobContext _context;

        public SentHistroy(GoodjobContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<MySMSInfo> data,int count)> GetSentSMSUsersList(PagingModel paging, string filter, string order)
        {
            var parameters = new { filter, order };
            var result = await _context.Database.GetDbConnection().QueryAsync<MySMSInfo>("Mng_GetAllSentUsersSMSCollection", parameters, commandType: CommandType.StoredProcedure);
            int count = result.Count();
            return (result,count);
        }
    }
}
