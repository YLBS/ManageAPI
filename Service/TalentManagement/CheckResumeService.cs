using IService.TalentManagement;
using Model.TalentManagement;
using System.Data;
using Dapper;
using Entity.Goodjob;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace Service.TalentManagement
{
    public class CheckResumeService : ICheckResume
    {
        private readonly GoodjobContext _context;

        public CheckResumeService(GoodjobContext context)
        {
            _context = context;
        }

       

        public async Task<IEnumerable<CheckingResumeInfo>> GetCheckResume(string filter)
        {
            var parameters = new { Where = filter };
            var result = await _context.Database.GetDbConnection().QueryAsync<CheckingResumeInfo>("Mng_GetCheckingResume", parameters, commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<int> InitData()
        {
            var result =  await _context.Database.GetDbConnection().ExecuteAsync("Mng_InitCheckingResume", null, commandType: CommandType.StoredProcedure);
            return result;
        }
        public async Task<int> Count()
        {
            return await _context.MngCheckingResumes.Where(m => m.CheckFlag == 1 || m.CheckFlag == 4).CountAsync();
        }

        public async Task<bool> CheckResume(int myUserId, int checkFlag)
        {
            var parameters = new { @MyUserID = myUserId, @CheckFlag = checkFlag };
            var result = await _context.Database.GetDbConnection().ExecuteAsync("Mng_CheckResume", parameters, commandType: CommandType.StoredProcedure);
            return result != 0;
        }

        public async Task<(string userName, string passWord)> GetUserNameAndPassWord(int myUserId)
        {
            var s= await _context.MyUsers.Where(m=>m.MyUserId==myUserId).Select(m=>new{ m.UserName,m.Password}).FirstOrDefaultAsync();
            if (s!=null) return (s.UserName,s.Password);
            return ("","");
        }

        public async Task<int> InsertReComMyUserId(int myUserId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue); //返回值
            parameters.Add("@myuserid", value: myUserId, dbType: DbType.Int32, direction: ParameterDirection.Input); 

            await _context.Database.GetDbConnection().ExecuteAsync("[Mem_Resume_InsertRecomMyuserID]", parameters, commandType: CommandType.StoredProcedure);
            int procedureReturn = parameters.Get<int>("ReturnValue");
            return procedureReturn;
        }
    }
}
