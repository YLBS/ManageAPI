using IService.TalentManagement;
using Entity.Goodjob;
using Dapper;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Model.TalentManagement;

namespace Service.TalentManagement
{
    public class WTResumeService : IWTResume
    {
        private readonly GoodjobContext _context;

        public WTResumeService(GoodjobContext context)
        {
            _context = context;
        }

        public async Task<string> GetAllWriteName(int myUserId)
        {
            var mobileNum = await _context.MyResumes.Where(m => m.MyUserId == myUserId).Select(m => m.MobileNum)
                .FirstOrDefaultAsync();
            return mobileNum;
        }

        public async Task<(string userName,string password)> GetMyUserAccount(string mobileNum)
        {
            string userName=string.Empty;
            string password=string.Empty;
            string sql =
                "SELECT TOP 1 [UserName],[Password] FROM [Goodjob].[dbo].[My_Users] u,[Goodjob].[dbo].[My_Resume] r WHERE u.[MyUserID]=r.[MyUserID] AND [MobileNum] LIKE '%@MobileNum%' ORDER BY u.[RegisterDate] DESC ";
            var param = new { mobileNum };
            using (var reader = await _context.Database.GetDbConnection().ExecuteReaderAsync(sql, param))
            {
                if (await reader.ReadAsync())
                {
                    userName=reader.GetString(0);
                    password=reader.GetString(1);
                }
            }
            return (userName,password);
        }

        public async Task<string> GetResumeInfoById(int id)
        {
            //原存储过程 usp_SelectHTRApplyList
            var s =await _context.HtrApplyLists.Where(h=>h.Id==id).Select(h=>h.ResumeContext).FirstOrDefaultAsync();
            return s;
        }

        public async Task<(IEnumerable<HtrApplyListDto> table, int count)> GetTableAsync(string where, string orderBy)
        {
            var parameters = new DynamicParameters();
            parameters.Add("RecordCount", dbType: DbType.Int32, direction: ParameterDirection.InputOutput); //输出值
            parameters.Add("WhereCondition", value: where, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add("OrderByExpression", value: orderBy, dbType: DbType.String, direction: ParameterDirection.Input);

            var result = (await _context.Database.GetDbConnection().QueryAsync<HtrApplyListDto>(
                "usp_SelectHTRApplyListDynamic", parameters,
                commandType: CommandType.StoredProcedure
            ));
            object outputParameterValue = parameters.Get<object>("RecordCount");
            int count = Convert.ToInt32(outputParameterValue);
            return (result, count);
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateDealWithStatus(int id, int myUserId)
        {
            var param = new { id, myUserId };
            string sql = " UPDATE [Goodjob].[dbo].[HTR_ApplyList] SET [MyUserID] = @myUserId WHERE [ID] = @ID ";
            var i = await _context.Database.GetDbConnection().ExecuteAsync(sql, param);
            return i > 0;
        }
    }
}
