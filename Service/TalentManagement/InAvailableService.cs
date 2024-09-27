using IService.TalentManagement;
using Model.TalentManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Goodjob;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Dapper;

namespace Service.TalentManagement
{
    public class InAvailableService : IInAvailable
    {
        private readonly GoodjobContext _context;
        public InAvailableService(GoodjobContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<InAvailableMobileInfo>> GetData(string filter, string order = "desc")
        {
            var param = new { @Filter = filter, @OrderBy = order };
            var result = await _context.Database.GetDbConnection().QueryAsync<InAvailableMobileInfo>("Mng_GetInAvailableMobileInfoList", param, commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<int> UpdateDealStatue(int id, string contact)
        {
            string sql =
                $"UPDATE [Goodjob].[dbo].[My_PhoneDisabled] SET [HasDealWith] = 1,[Contact] = {contact} WHERE ID = {id}";
            var result = await _context.Database.GetDbConnection().ExecuteAsync(sql);
            return result;
        }
    }
}
