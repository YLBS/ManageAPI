using IService.TalentManagement;
using Model.TalentManagement;
using Entity.Goodjob;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Dapper;
using Entity.GoodjobResum;

namespace Service.TalentManagement
{
    public class ExtranetResumeManage : IExtranetResumeManage
    {
        private readonly GoodjobContext _context;
        private readonly Goodjob_resumeContext _resumeContext;

        public ExtranetResumeManage(GoodjobContext context, Goodjob_resumeContext resumeContext)
        {
            _context = context;
            _resumeContext = resumeContext;
        }

        public async Task<List<ResumeNewListInfo>> GetData(string filter)
        {
            List<ResumeNewListInfo> list =new List<ResumeNewListInfo>();
            var para = new { @Filter = filter };
            using (var reader = await _context.Database.GetDbConnection()
                       .ExecuteReaderAsync("[Mng_GetResumeNewListMain_New]", para,
                           commandType: CommandType.StoredProcedure))
            {
                while (await reader.ReadAsync())
                {
                    ResumeNewListInfo info = new ResumeNewListInfo
                    (reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetByte(3), reader.GetInt32(4), reader.GetInt32(5), reader.GetDateTime(6), reader.GetDateTime(7), reader.GetInt32(9), reader.GetInt32(8), reader.GetString(10), reader.GetBoolean(11), reader.GetInt32(12), reader.GetInt32(13));
                    info.LastLoginDate = Convert.ToDateTime(reader["LastLoginDate"]).ToString("yyyy/MM/dd");
                    info.SourceName = reader["SourceName"] is DBNull ? "" : reader["SourceName"].ToString();
                    list.Add(info);
                }
            }

            return list;
        }

        public async Task<List<MyUserInfo>> GetMyUserInfoAsync(int[] myUserIds)
        {
            //原存储过程 My_GetUserNameAndPassWordAndEmail 以及 My_GetUserNameAndPassWordAndPhone
            var s =  await (from a in _resumeContext.MyUsers  
                    join b in   _resumeContext.MyResumes on a.MyUserId equals b.MyUserId
                    where myUserIds.Contains(a.MyUserId)
                    select new MyUserInfo()
                    {
                        MyUserId = a.MyUserId,
                        UserName = a.UserName,
                        Password = a.Password,
                        Email = a.Email,
                        Phone = b.MobileNum
                    }).ToListAsync();
            return s;
        }
    }
}
