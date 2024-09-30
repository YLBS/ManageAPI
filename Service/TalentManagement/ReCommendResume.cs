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
using System.Security.Cryptography;
using Entity.GoodjobResum;
using System.Reflection.Metadata;

namespace Service.TalentManagement
{
    public class ReCommendResume : IReCommendResume
    {
        private readonly GoodjobContext _context;
        public ReCommendResume(GoodjobContext context)
        {
            _context = context;
        }

        public async Task<int> DeleteReComMyUserId(string myUserIds)
        {
            //原存储过程Mem_Resume_DeleteRecomMyuserID

            string delSql = $"delete from RecomMyuserID where MyuserID in(" + myUserIds + ")";

            int i = await _context.Database.GetDbConnection().ExecuteAsync(delSql);
            return i;
        }

        public async Task<IEnumerable<ReCommendResumeInfo>> GetRecommendResumeList(string getTop, string filter, string orderBy, bool searchType)
        {
            var parameters = new { @GetTop = getTop, @Filter = filter , @OrderBy = orderBy, @SearchType = searchType };
            var result = await _context.Database.GetDbConnection().QueryAsync<ReCommendResumeInfo>("Mng_GetReCommendResumeList", parameters, commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<bool> InsertReComMyUserId(int myUserId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue); 
            parameters.Add("@myuserid", value: myUserId, dbType: DbType.Int32, direction: ParameterDirection.Input);
            int i= await _context.Database.GetDbConnection().ExecuteAsync("Mem_Resume_InsertRecomMyuserID", parameters, commandType: CommandType.StoredProcedure);
            int procedureReturn = parameters.Get<int>("ReturnValue");
            if (procedureReturn == 1)
                return false;
            return i != 0;
        }
    }
}
