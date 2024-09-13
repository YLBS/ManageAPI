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
using System.Collections;

namespace Service.ResumeSearch
{
    public class ExtranetResumeCheck: IExtranetResumeCheck
    {
        private readonly GoodjobContext _context;
        public ExtranetResumeCheck(GoodjobContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ResumeListInfo>> CreateNewsResumeListCollection(string filter, string orderBy)
        {
            var param = new { @ResumeIDStr = filter, @OrderString = orderBy };
            var result = await _context.Database.GetDbConnection().QueryAsync<ResumeListInfo>("Mng_NewsGetResumeListMain", param, commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<bool> RefreshResume(string myUserIds)
        {
            var parameters = new { @MyUserStr = myUserIds };
            var result = await _context.Database.GetDbConnection().ExecuteAsync("Mng_RefreshResume", parameters, commandType: CommandType.StoredProcedure);
            return result > 0;
        }

        public async Task<bool> UpdateResumeStatus(int myUserId, short resumeStatus, string partComName, short engResumeFlag)
        {
            var parameters = new { @MyUserID = myUserId, @ResumeStatus=resumeStatus, @PartComName=partComName, @EngResumeFlag= engResumeFlag };
            var result = await _context.Database.GetDbConnection().ExecuteAsync("My_UpdateResumeStatus", parameters, commandType: CommandType.StoredProcedure);
            return result > 0;
        }

        public async Task<string> GetWorkText(int myUserId)
        {
            var str = await _context.MyResumeOldTexts.Where(m => m.MyUserId == myUserId).Select(m => m.WorkText)
                .FirstOrDefaultAsync();
            return str;
        }

        public async Task<DictionaryEntry> GetUserPass(int myUserId)
        {
            DictionaryEntry entry = new DictionaryEntry();
            var para = new { @MyUserID = myUserId };
            using (var reader = await _context.Database.GetDbConnection()
                       .ExecuteReaderAsync("Mng_GetPersonUserNameAndPassWord", para,
                           commandType: CommandType.StoredProcedure))
            {
                if (await reader.ReadAsync())
                {
                    entry.Key = reader.GetString(0);
                    entry.Value = reader.GetString(1);
                }
            }
            return entry;
        }

        public async Task<bool> UpdateWorkText(int myUserId,string workText)
        {
            string sql = "update My_Resume_OldText set WorkText = @workText WHERE MyUserID = @myUserId";
            // 参数
            var parameters = new { myUserId = myUserId, workText = workText };
            var result = await _context.Database.GetDbConnection().ExecuteAsync(sql, parameters);
            return result > 0;
        }
    }
}
