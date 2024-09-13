using IService.ResumeSearch;
using Model.ResumeSearch;
using ServiceStack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Goodjob;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Dapper;
using Entity.GoodjobQuery;
using System.Diagnostics.Metrics;
using Entity.GoodjobOther;

namespace Service.ResumeSearch
{
    public class QuickSearch : IQuickSearch
    {
        private readonly GoodjobContext _context;
        private readonly Goodjob_QueryContext _queryContext;
        public QuickSearch(GoodjobContext context, Goodjob_QueryContext query)
        {
            _context = context;
            _queryContext = query;
        }


        public async Task<IList<ResumeListQueryInfo>> GetData(string filter, string keyword, int orderBy)
        {
            //先获取简历Id集合
           
            string proc = "Mng_GetResumeListQueryID"; //按更新时间排序
            if (orderBy != 1) //按注册时间排序
            {
                proc = "Mng_GetResumeListQueryID_Sub";
               // filter = filter.Replace("goodjob_query.dbo.MyResume_Query", " Goodjob.dbo.My_Resume");
            }
            var param = new { Where = filter, Keyword = keyword };
            var ids = await _context.Database.GetDbConnection().QueryAsync<int>(proc, param, commandType: CommandType.StoredProcedure);
            if (ids.Any())
            {
                IList<ResumeListQueryInfo> listQueryList = new List<ResumeListQueryInfo>();
                string idsStr = string.Join(",", ids);
                var para = new { idstr = idsStr };
                using (var reader = await _context.Database.GetDbConnection()
                           .ExecuteReaderAsync("Mng_GetResumeListQuery", para,
                               commandType: CommandType.StoredProcedure))
                {
                    while (await reader.ReadAsync())
                    {
                        var listQuery = new ResumeListQueryInfo(reader.GetInt32(0), reader.GetByte(1), reader.GetByte(2), reader.GetByte(3), reader.GetString(4), reader.GetByte(5), reader.GetDateTime(6), reader.GetInt32(7), reader.GetInt32(8), reader.GetInt32(9), reader.GetInt32(10), reader.GetInt32(11), reader.GetString(12), reader.GetString(13), reader.GetInt32(14), reader.GetString(15), reader.GetDateTime(16));
                        listQuery.RegisterDate = Convert.ToDateTime(reader["RegisterDate"]);
                        listQueryList.Add(listQuery);
                    }
                }

                var im = await _context.PubImRoamingLogs.Where(p =>
                        ids.Contains(p.MyUserId) && p.Direction == 2 && p.InsertTime > DateTime.Now.AddHours(-1))
                    .ToListAsync();
                //投递记录
                var pa = await _context.PubPerApplyPos
                    .Where(p => ids.Contains(p.MyUserId) && p.PerTime > DateTime.Now.AddDays(-7)).ToListAsync();
                //offer记录
                var pi = await (from a in _context.PubInterviewings
                          join b in _context.MemOfferLetters on a.OfferLetterId equals b.Id
                          //into temp from subItem in temp.DefaultIfEmpty()
                          where ids.Contains(a.MyUserId) && a.MyResultTime < b.SendTime.AddDays(1)
                          select a.MyUserId).ToListAsync();

                foreach (var item in listQueryList) 
                {
                    //补充消息记录，求职者回复状态
                    if (im.Exists(i => i.MyUserId == item.ResumeID))
                    {
                        int c = im.Where(i => i.MyUserId == item.ResumeID).Count();
                        item.MyResponseStatus = "1小时内回复" + c + "次";
                    }
                    //补充投递记录
                    else
                    {
                        var myTodayApply =pa.Where(w => w.PerTime > DateTime.Now.AddDays(-1) && w.MyUserId == item.ResumeID).ToList();
                        var myWeekApply = pa.Where(w => w.MyUserId == item.ResumeID).ToList();
                        if (pi.Exists(e => e == item.ResumeID))
                        {
                            item.MyResponseStatus = "回复快";
                        }
                        if (myWeekApply.Count > 0)
                        {
                            item.MyResponseStatus = "最近活跃";
                        }
                        if (myTodayApply.Count > 0)
                        {
                            item.MyResponseStatus = "今日活跃";
                        }

                    }
                }
                return listQueryList;
            }

            return null;
        }



        public async Task<string> ExcludeRecommendResumeId(int[] myUserId, int posId)
        {
            var yn = await _queryContext.MemPosQueries.Where(m => m.PosId == posId).FirstOrDefaultAsync();
            if (yn == null)
                return "该职位暂未发布";
            var str = await _context.MemLibCommends.Where(m => m.PosId == posId && myUserId.Contains(m.MyUserId))
                .Select(m => m.MyUserId).ToArrayAsync();
            return string.Join(",", str);
        }
        public async Task<string> ExcludeDownResumeId(int[] myUserId, int posId)
        {
            var yn = await _queryContext.MemPosQueries.Where(m => m.PosId == posId).FirstOrDefaultAsync();
            if (yn == null)
                return "该职位暂未发布";
            var str = await _context.MemLibDownloads.Where(m => m.MemId == yn.MemId && myUserId.Contains(m.MyUserId))
                .Select(m => m.MyUserId).ToArrayAsync();
            return string.Join(",", str);
        }
        private string FormatListSearchSql(IDictionary dictList)
        {
            if (dictList == null) return "";
            if (dictList.Count == 0) return "";
            string oneSql = string.Empty;
            string listSql = string.Empty;
            foreach (DictionaryEntry item in dictList)
            {
                if (item.Value == null) continue;
                switch (item.Key.ToString())
                {
                    case "ResumeID": oneSql = string.Format(item.Value.ToString(), "MyUserID"); break;
                    case "ResumeName": oneSql = string.Format(item.Value.ToString(), "PerName"); break;
                    case "ResumeStatus": oneSql = string.Format(item.Value.ToString(), "ResumeStatus"); break;
                    case "MobileNum": oneSql = string.Format(item.Value.ToString(), "MobileNum"); break;
                    case "MaritalStatus": oneSql = string.Format(item.Value.ToString(), "MaritalStatus"); break;
                    case "LastPos": oneSql = string.Format(item.Value.ToString(), "LastPosName"); break;
                    case "RegisterFrom": oneSql = item.Value.ToString(); break;
                }
                listSql += oneSql;
            }

            return listSql;
        }

        public async Task<List<ResumeCheckingInfo>> GetResumeCheckingCollection(int[] myUserId)
        {
            var s = await _context.MyResumes.Where(m => myUserId.Contains(m.MyUserId)).Select(m => new ResumeCheckingInfo
                {
                    MyUserID = m.MyUserId,
                    PerName = m.PerName,
                    Email = m.Email,
                })
                .ToListAsync();
            return s;
        }
        public async Task<int[]> JobFunctionSmall(int posId)
        {
            var jobFunctionSmall = await _context.MemPosJobFunctions.Where(m => m.PosId == posId)
                .Select(m => m.JobFunctionSmall).Distinct().ToArrayAsync();
            return jobFunctionSmall;
        }

        public async Task<bool> CheckPosExist(int posId)
        {
            var s = await _context.MemPositions.Where(m => m.PosId == posId).FirstOrDefaultAsync();
            return s != null;
        }

    }
}
