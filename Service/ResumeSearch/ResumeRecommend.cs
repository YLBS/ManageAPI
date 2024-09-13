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
    public class ResumeRecommend : IResumeRecommend
    {
        private readonly GoodjobContext _context;
        public ResumeRecommend(GoodjobContext context)
        {
            _context = context;
        }

        public async Task<int> AddMemLibCommendNew(int memId, string myUserIds, int operType,int userId)
        {
            var memInfo=await _context.MemInfos.Where(m=>m.MemId==memId).FirstOrDefaultAsync();
            if (memInfo == null)
            {
                return -1;
            }
            int[] ids= myUserIds.Split(",").Select(s=>Convert.ToInt32(s)).ToArray();
            //原存储过程 [Mem_AddMemLibCommendNews]
            var list = await _context.MemLibCommends.Where(m => m.MemId == memId && ids.Contains(m.MyUserId))
                .ToListAsync();
            foreach (var item in list)
            {
                item.InsertDate = DateTime.Now;
                ids = ids.Where(s => s != item.MyUserId).ToArray();//去重
            }

            foreach (var id in ids)
            {
                MemLibCommend m = new MemLibCommend();
                m.MemId = memId;
                m.MyUserId = id;
                m.RecommendType = operType;
                m.PosId = 0;
                m.SalerId = userId;
                await _context.MemLibCommends.AddAsync(m);
            }
            return await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ResumeListQueryInfo>> GetExtranetResumeRecommendList(string idStr)
        {
            var parameters = new { idstr = idStr, };
            var result = await _context.Database.GetDbConnection().QueryAsync<ResumeListQueryInfo>("Mng_GetExtranetResumeRecommendListQuery", parameters, commandType: CommandType.StoredProcedure);
            return result;
        }
    }
}
