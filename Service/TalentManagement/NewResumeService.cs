using IService.TalentManagement;
using Model.ResumeSearch;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Commons.Tool;
using Dapper;
using Entity.Goodjob;
using Microsoft.EntityFrameworkCore;
using Goodjob.Resume;
using Entity.GoodjobResum;
using Model.TalentManagement;
using PubInformationMy = Entity.Goodjob.PubInformationMy;
using System.Reflection.Metadata;

namespace Service.TalentManagement
{
    public class NewResumeService : INewResumeService
    {
        private GoodjobContext _context;

        public NewResumeService (GoodjobContext context)
        {
            _context = context;
        }

        

        public async Task<bool> CreateMySMS(MySMSInfo info)
        {
            MySm mySm=new MySm();
            mySm.MyUserId=info.MyUserId;
            mySm.UserName=info.UserName;
            mySm.Password=info.Password;
            mySm.SentTime=DateTime.Now;
            await _context.MySms.AddAsync(mySm);
            return await _context.SaveChangesAsync()>0;
        }

        public async Task DeleteResume(int myUserId)
        {
            var parameters = new { @MyUserID = myUserId };
            var result = await _context.Database.GetDbConnection().ExecuteAsync("Mng_DeleteResume", parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<MySMSInfo> GetMySMSInfo(int myUserId)
        {
            var parameters = new { @MyUserID = myUserId };
            var result = await _context.Database.GetDbConnection().QueryFirstOrDefaultAsync<MySMSInfo>("My_Get_SMSInfo_ByMyUserID", parameters, commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<IList<ResumeListInfo>> GetNewResumeListCollection(string filter, string orderField, string orderType)
        {
            IList<ResumeListInfo> resumelist = new List<ResumeListInfo>();
            //先获取Id集合，存储过程Mng_GetResumeListMainID，三个参数，第二个参数才存储过程竟然再次初始化了，6啊
            var param = new { ResumeFilter = filter, UserFilter = "" };

            var ids = await _context.Database.GetDbConnection().QueryAsync<int>("Mng_GetResumeListMainID", param, commandType: CommandType.StoredProcedure);
            string idsStr = string.Join(",", ids);
            //获取数据 Mng_GetNewResumeListMain，原传了排序的又不用。
            var parameters = new { ResumeIDStr = idsStr, @OrderString = $" order by {orderField} {orderType}" };
            using (var reader = await _context.Database.GetDbConnection()
                       .ExecuteReaderAsync("Mng_GetNewResumeListMain", parameters,
                           commandType: CommandType.StoredProcedure))
            {
                while (await reader.ReadAsync())
                {
                    ResumeListInfo resumeInfo = new ResumeListInfo(reader.GetInt32(0), reader.GetString(1), (short)reader.GetByte(2), reader.GetString(3), reader.GetInt32(4), reader.GetInt32(5), reader.GetDateTime(6), reader.GetDateTime(7), (short)reader.GetByte(8), (short)reader.GetByte(9), reader.GetString(10), reader.GetBoolean(11));
                    resumeInfo.resumesource = reader.GetInt32(12);
                    resumeInfo.qdUserID = reader["QdUserID"] is DBNull ? 0 : Convert.ToInt32(reader["QdUserID"]);
                    resumelist.Add(resumeInfo);
                }
            }
            //循环调查询，最多时为800条数据，待优化
            foreach (var item in resumelist)
            {
                var para = new { item.MyUserID };
                using (var reader = await _context.Database.GetDbConnection()
                           .ExecuteReaderAsync("Mng_GetPersonUserNameAndPassWord", para,
                               commandType: CommandType.StoredProcedure))
                {
                    if (await reader.ReadAsync())
                    {
                        item.MyLoginString = "http://my.goodjob.cn/IndexLogin/?user=" + EnAndDecryption.DESURL(reader.GetString(0)) + "&pass=" + EnAndDecryption.DESURL(reader.GetString(1)) + "&LoginType=1";
                    }
                    
                }
                string registerDate_sub1 = item.RegisterDate_Sub1.ToString("d");
                if (registerDate_sub1.Equals(DateTime.Now.ToString("d")))//DateTime.Now.ToString("d")
                {
                    item.RegisterDate_Sub2 = "<label style=\"color: red\">今天" + item.RegisterDate_Sub1.ToString("t") + "</label>";
                }
                else if (registerDate_sub1.Equals(DateTime.Now.AddDays(-1).ToString("d")))
                {
                    item.RegisterDate_Sub2 = "<label style=\"color: red\">昨天" + item.RegisterDate_Sub1.ToString("t") + "</label>";
                }
                else
                {
                    item.RegisterDate_Sub2 = "<label>" + item.RegisterDate_Sub1.ToString("d") + "</label>";
                    //item.RegisterDate_Sub2 = "<label style=\"color: red\">" + item.RegisterDate_Sub1.ToString("g") + "</label>";
                }

            }
            return resumelist.OrderByDescending(c => c.RegisterDate_Sub1).ToList();
        }

        public async Task InsertPersonMessage(AddMessageInfo info)
        {
            //原存储过程 Pub_Information_My
            PubInformationMy information = new PubInformationMy();
            information.MyUserId = info.Id;
            information.ClerkId = info.UserId;
            information.Subject=info.Subject;
            information.Content=info.Content;
            information.Issuer = info.UserName;
            information.IssuedDate=DateTime.Now;
            await _context.PubInformationMies.AddAsync(information);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateResumeIsqd(int myUserId)
        {
            //原存储过程 Mng_Udate_ResumeIsqdFlag

            var parameters = new { @myuserid = myUserId };
            var result = await _context.Database.GetDbConnection().ExecuteAsync("Mng_Udate_ResumeIsqdFlag", parameters, commandType: CommandType.StoredProcedure);
            return result > 0;

            //var list = await _context.MyResumes.Where(m => m.MyUserId == myUserId).FirstOrDefaultAsync();
            //if (list == null)
            //    return false;
            //list.IsdqStatus = true;
            //return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> AddqdRecord(int myUserId, int userId)
        {
            var parameters = new { @myuserid = myUserId, @userid=userId };
            var result = await _context.Database.GetDbConnection().ExecuteAsync("Mng_Add_ResumeIsqdRecord", parameters, commandType: CommandType.StoredProcedure);
            return result > 0;
        }

        public async Task<bool> GetSendingLog(int myUserId)
        {
            var list = await _context.MySms.Where(m => m.MyUserId == myUserId).FirstOrDefaultAsync();
            return list != null;
        }
    }
}
