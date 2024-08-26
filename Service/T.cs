using Dapper;
using Entity.Goodjob;
using IService;
using Microsoft.EntityFrameworkCore;
using Model;
using System.Data;

namespace Service
{
    public class T : IT
    {
        private readonly GoodjobContext _goodjobdb;
        public T(GoodjobContext goodjobdb)
        {
            _goodjobdb = goodjobdb;
        }
        public async Task<MemInfoDto> GetMemInfo()
        {
            var sql = "select top 10 * from Mem_Info a left join  Mem_Users b on a.MemId=b.MemID";

            var dt1 = await _goodjobdb.Database.GetDbConnection().QueryFirstOrDefaultAsync<MemInfoDto>(sql);

            return dt1;
        }

        public async Task<IEnumerable<OutJiuYeStationDto>> GetOutJiuYeStationList()
        {
            // List<OutJiuYeStationDto> list=new List<OutJiuYeStationDto>();
            var parameters = new { ZphAdminID = 0 };
            var list1 = await _goodjobdb.Database.GetDbConnection()
                .QueryAsync<OutJiuYeStationDto>("sitedata.Dbo.InquireJYStation", parameters, commandType: CommandType.StoredProcedure);

            return list1;
        }
    }
}
