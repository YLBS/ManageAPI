using IService.SalesDepartment;
using Model.SalesDepartment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.GoodjobQuery;
using System.Data;
using Dapper;
using Entity.Goodjob;
using Microsoft.EntityFrameworkCore;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

namespace Service.SalesDepartment
{
    public class ResumeVice : IResumeVice
    {
         private readonly Goodjob_QueryContext _queryContext;
         private readonly GoodjobContext _goodjobContext;

         public ResumeVice(Goodjob_QueryContext queryContext, GoodjobContext goodjobContext)
         {
             _queryContext = queryContext;
             _goodjobContext = goodjobContext;
         }

        public Task<int> AddMemLibCommendVice(int memId, int salerUserId, int userId, int posId)
        {

            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ResumeViceInfo>> GetData(string filter)
        {
            var parameters = new { where = filter};
            var result = await _queryContext.Database.GetDbConnection().QueryAsync<ResumeViceInfo>("My_GetResumeVice", parameters, commandType: CommandType.StoredProcedure);
            return result;
        }

        public Task<IEnumerable<ResumeViceInfo>> GetData_N(string keyWord, int depId, int depUserId)
        {

            throw new NotImplementedException();
        }

        public async Task<IEnumerable<PositionListInfo>> GetSalerCompanyByPosWord(string salerUserId, string keyWord)
        {
            string sql =
                $"select mq.PosID,mq.PosName,mu.MemID,mq.MemName from Goodjob_Query.dbo.MemPos_Query as mq  left join  Goodjob.dbo.Mem_Users as mu on mq.MemID = mu.MemID where   mu.SalerUserID in ({salerUserId}) and mu.MemberClass = 2 and Contains((PosName,MemName),'{keyWord}')";
            var result = await _goodjobContext.Database.GetDbConnection().QueryAsync<PositionListInfo>(sql);
            return result;
        }

        public async Task<List<PositionListInfo>> GetVicePos(int vid)
        {
            var list = await (from a in _goodjobContext.MemPositions
                join b in _goodjobContext.MemLibCommendVices on a.PosId equals b.PosId
                where b.ViceId == vid
                select new PositionListInfo
                {
                    posId = a.PosId,
                    posName = a.PosName,
                    memId = b.MemId,
                    memName = a.MemName,
                }).ToListAsync();
            return list;
        }
    }
}
