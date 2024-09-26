using Entity.Goodjob;
using Entity.GoodjobQuery;
using IService.SalesDepartment;
using Microsoft.EntityFrameworkCore.Query;
using Model.SalesDepartment;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Model.Common;
using Entity.GoodjobResum;
using static ServiceStack.Script.Lisp;
using Goodjob.Resume;
using Commons.Tool;
using Entity.GoodjobOther;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;
using ServiceStack;
using Goodjob.Position;
using System.Security.Cryptography;
using System.Reflection.Metadata;

namespace Service.SalesDepartment
{
    public class ResumeShareVice : IResumeShareVice
    {
        private readonly Goodjob_QueryContext _queryContext;
        private readonly GoodjobContext _goodjobContext;
        private readonly Goodjob_OtherContext _otherContext;

        public ResumeShareVice(Goodjob_QueryContext queryContext, GoodjobContext goodjobContext, Goodjob_OtherContext orContext)
        {
            _queryContext = queryContext;
            _goodjobContext = goodjobContext;
            _otherContext = orContext;
        }

        #region const strings...


        // FORMAT
        private const string FIELD_RESUMEID = "MyUserID";
        private const string FIELD_RESUMENAME = "PerName";

        private string[] FIELD_HOPEWORK = new string[]
        {
            "goodjob_query.dbo.MyResume_Query.MyUserID", "Select MyUserID from My_JobFunction", "JobFunction_big",
            "JobFunction_small"
        };

        private string[] FIELD_HOPEAREA = new string[]
        {
            "goodjob_query.dbo.MyResume_Query.MyUserID", "Select MyUserID from My_JobLocation", "JobLocation_P",
            "JobLocation_C"
        };

        private string[] FIELD_LANGUAGE = new string[]
        {
            "goodjob_query.dbo.MyResume_Query.MyUserID", "Select MyUserID from My_Language", "LanguageID",
            "LanguageLevel"
        };

        private const string FIELD_SEX = "Sex";
        private const string FIELD_AGE = "datediff(year,Birthday,getdate())";
        private const string FIELD_DEGREE = "DegreeID";
        private const string FIELD_WORKYEAR = "WorkedYear";

        private const string FIELD_LOCATIONP = "Location_P";
        private const string FIELD_LOCATIONC = "Location_C";
        private const string FIELD_HOMETOWNP = "Hometown_P";
        private const string FIELD_HOMETOWNC = "Hometown_C";
        private const string FIELD_HAVEPHOTO = "PhotoFlag";
        private const string FIELD_HAVEENGLISH = "EngResumeFlag";
        private const string FIELD_SCHOOL = "SchoolName";
        private const string FIELD_SPECIAL = "Speciality";
        private const string FIELD_LASTPOS = "LastPosName";

        private const string FIELD_RESUMESTATUS = "ResumeStatus";

        private const string FIELD_MOBILENUM = "MobileNum";


        private const string FIELD_UPDATEDATE = "datediff(day,UpdateDate,getdate())";

        #endregion


        public async Task<(List<ResumeViceInfo> item, int cout)> GetResumeShareViceList(Singles singles)
        {
            List<ResumeViceInfo> l = new List<ResumeViceInfo>();
            string sql =
                $"DECLARE @PageSize INT = {singles.PageSize}; DECLARE @PageNumber INT = {singles.PageIndex}; select  my.MyUserID,my.PerName,my.JobSeeking,my.MobileNum,my.ContactPhone,srf.RegisterFrom,srf.RegisterFromDate, mrf.EplID, (select top 1 PosName from Goodjob.dbo.My_WorkExp as we where we.MyUserID = my.MyUserID order by BeginDate desc) as LastPosName from Goodjob.dbo.My_Resume as my left join [Goodjob].[dbo].[Sys_RegisterFrom] srf on my.MyUserID = srf.MyUserID  left join [Goodjob].[dbo].[My_RegisterForCrm] mrf on my.MyUserID = mrf.MyUserID where 1=1 {singles.filter}   OFFSET (@PageNumber - 1) * @PageSize ROWS  FETCH NEXT @PageSize ROWS ONLY;";
            using (var reader = await _goodjobContext.Database.GetDbConnection().ExecuteReaderAsync(sql))
            {
                while (await reader.ReadAsync())
                {
                    string JobSeeking = reader["JobSeeking"] is DBNull ? "" : Convert.ToString(reader["JobSeeking"]);
                    string LastPosName = reader["LastPosName"] is DBNull ? "" : Convert.ToString(reader["LastPosName"]);
                    string MobileNum = reader["MobileNum"] is DBNull ? "" : Convert.ToString(reader["MobileNum"]);
                    string ContactPhone =
                        reader["ContactPhone"] is DBNull ? "" : Convert.ToString(reader["ContactPhone"]);
                    string resumeTitle = string.IsNullOrEmpty(JobSeeking) ? LastPosName : JobSeeking;
                    l.Add(new ResumeViceInfo()
                    {
                        PerName = Convert.ToString(reader["PerName"]) + "(" + resumeTitle + ")",
                        AddTime = Convert.ToDateTime(reader["RegisterFromDate"]),
                        MyUserId = Convert.ToInt32(reader["MyUserID"]),
                        Contact = string.IsNullOrEmpty(MobileNum) ? ContactPhone : MobileNum,
                        RegisterFrom = Convert.ToInt32(reader["RegisterFrom"]),
                        EPL_Name = "请选择"
                    });
                }
            }

            string str = singles.filter.Substring(0, singles.filter.LastIndexOf("order"));

            string sql1 =
                $"select  count(*) from Goodjob.dbo.My_Resume as my  left join[Goodjob].[dbo].[Sys_RegisterFrom] srf on my.MyUserID = srf.MyUserID  left join[Goodjob].[dbo].[My_RegisterForCrm] mrf on my.MyUserID = mrf.MyUserID where 1=1 {str}";

            var result = await _goodjobContext.Database.GetDbConnection().QueryFirstOrDefaultAsync<int>(sql1);
            return (l, result);
        }

        public async Task<(IEnumerable<ResumeViceInfo> item, int cout)> GetViceResume(Singles singles)
        {
            //旧的 My_GetResumeVice 没有分页。 ResumeVice 使用了
            var parameters = new { where = singles.filter, pageIndex = singles.PageIndex, pagesize = singles.PageSize };
            var result = await _queryContext.Database.GetDbConnection().QueryMultipleAsync("My_GetResumeVice_N",
                parameters, commandType: CommandType.StoredProcedure); //QueryAsync<>
            var firstResultSet = result.Read<ResumeViceInfo>();
            var count = result.Read<int>().ToList().FirstOrDefault();
            //最后关闭释放
            result.Dispose();
            return (firstResultSet, count);
        }

        public async Task<ResumeInfo> GetResume(int myUserId)
        {
            ResumeInfo resumeInfo = new ResumeInfo();
            var parm = new { MyUserID = myUserId };
            try
            {
                //var resume = await _goodjobContext.MyResumes.Where(m => m.MyUserId == myUserId).SingleAsync();
                //var user = await _goodjobContext.MyUsers.Where(m => m.MyUserId == myUserId).SingleAsync();
                ////更新时间
                //resumeInfo = new ResumeInfo(myUserId, 0, resume.UpdateDate, 0, null, null, null, null, null, null, null, null, null, null, null, null, null);
                ////用户表信息
                //resumeInfo.HitCount = user.HitCount;
                //resumeInfo.EngResumeFlag = user.EngResumeFlag;
                ////简历完整性 原存储过程 My_GetResumeWrite
                //resumeInfo.ResumeWrite.MyUserID = myUserId;
                //resumeInfo.ResumeWrite.BaseWrite = resume.BaseWrite;
                //resumeInfo.ResumeWrite.WorkWrite = resume.WorkWrite;
                //resumeInfo.ResumeWrite.TrainingWrite = resume.TrainingWrite;
                //resumeInfo.ResumeWrite.LanguageWrite = resume.LanguageWrite;
                //resumeInfo.ResumeWrite.HopeWrite = resume.HopeWrite;
                //resumeInfo.ResumeWrite.ProjectWrite = resume.ProjectWrite;
                //resumeInfo.ResumeWrite.CertWrite = resume.CertWrite;
                //resumeInfo.ResumeWrite.SkillWrite = resume.SkillWrite;
                //resumeInfo.ResumeWrite.AllWrite = resume.AllWrite;
                ////基本资料 原 My_GetBaseInfo
                //resumeInfo.BaseInfo.MyUserID = myUserId;
                //resumeInfo.BaseInfo.PerName= resume.PerName;
                //resumeInfo.BaseInfo.Sex = resume.Sex;
                //resumeInfo.BaseInfo.Nationality = resume.Nationality;
                //resumeInfo.BaseInfo.Birthday = (DateTime)resume.Birthday;
                //resumeInfo.BaseInfo.CardType = resume.CardType;

                //resumeInfo.BaseInfo.CardNum = resume.CardNum;
                //resumeInfo.BaseInfo.Height = resume.Height;
                //resumeInfo.BaseInfo.Weight = resume.Weight;
                //resumeInfo.BaseInfo.MaritalStatus = resume.MaritalStatus;
                //resumeInfo.BaseInfo.HometownP = resume.HometownP;
                //resumeInfo.BaseInfo.HometownC = resume.HometownC;
                //resumeInfo.BaseInfo.LocationP = resume.LocationP;
                //resumeInfo.BaseInfo.LocationC = resume.LocationC;
                //resumeInfo.BaseInfo.DegreeID = resume.DegreeId;
                //resumeInfo.BaseInfo.SelfDescription = resume.SelfDescription;
                //resumeInfo.BaseInfo.WorkedYear = resume.WorkedYear;
                //resumeInfo.BaseInfo.WorkedMonth = resume.WorkedMonth;
                //resumeInfo.BaseInfo.LocationT = resume.LocationT;


                //更新时间
                var a = await _goodjobContext.MyResumes.Where(m => m.MyUserId == myUserId).Select(m => m.UpdateDate)
                    .SingleAsync();
                resumeInfo = new ResumeInfo(myUserId, 0, a, 0, null, null, null, null, null, null, null, null, null,
                    null, null, null, null);
                //用户表信息
                var b = await _goodjobContext.MyUsers.Where(m => m.MyUserId == myUserId)
                    .Select(m => new { m.HitCount, m.EngResumeFlag }).SingleAsync();
                resumeInfo.HitCount = b.HitCount;
                resumeInfo.EngResumeFlag = b.EngResumeFlag;
                //简历完整性 原存储过程 My_GetResumeWrite
                string sql =
                    $"SELECT [MyUserID], [BaseWrite],[WorkWrite],[EduWrite],[TrainingWrite],[LanguageWrite],[HopeWrite],[ProjectWrite],[CertWrite],[FulfilWrite],[SkillWrite],[AllWrite]  FROM [dbo].[My_Resume]  WHERE [MyuserID]={myUserId}";
                resumeInfo.ResumeWrite = await _goodjobContext.Database.GetDbConnection()
                    .QueryFirstOrDefaultAsync<ResumeWriteInfo>(sql);
                //基本资料 原 My_GetBaseInfo
                string sql1 =
                    $"SELECT [MyUserID], [PerName],[Sex], [Nationality],[Birthday], [CardType], [CardNum], [Height], [Weight], [MaritalStatus],  [Hometown_P] hometownP, [Hometown_C] hometownC,[Location_P] locationP, [Location_C] locationC, [DegreeID],  [SelfDescription],[WorkedYear],[WorkedMonth],Location_T locationT FROM [dbo].[My_Resume] WHERE [MyuserID]={myUserId}";
                resumeInfo.BaseInfo =
                    await _goodjobContext.Database.GetDbConnection().QueryFirstOrDefaultAsync<BaseInfo>(sql1);
                //联系方式 [My_GetContact]
                string sql2 =
                    $"SELECT [MyuserID], [ContactPhone], [MobileNum], [IMNum],  [Email], [Homepage], [Address], [ZipCode] FROM  [dbo].[My_Resume]  WHERE  [MyuserID]= {myUserId}";
                resumeInfo.BaseInfo.Contact = await _goodjobContext.Database.GetDbConnection()
                    .QueryFirstOrDefaultAsync<ContactInfo>(sql2);
                //教育经历 
                if (resumeInfo.ResumeWrite.EduWrite == 2)
                {
                    // var l= await _goodjobContext.MyResumeOldTexts.Where(m => m.MyUserId == myUserId)  .Select(m =>new { m.EduText,m.}).FirstOrDefaultAsync();
                    //string sql3=$"select EduText from My_Resume_OldText where MyUserID={myUserId}";
                    resumeInfo.EduText = await _goodjobContext.MyResumeOldTexts.Where(m => m.MyUserId == myUserId)
                        .Select(m => m.EduText).FirstOrDefaultAsync();
                }
                else
                {
                    // 原 My_GetEducation
                    string sql3 =
                        $"SELECT  [ID], [MyuserID], [BeginDate], [EndDate], [SchoolName], [Speciality], [Certificate], [DegreeID],  [SpecialityMemo] FROM [dbo].[My_Education] WHERE [MyuserID]={myUserId} ORDER BY  [BeginDate] DESC ";
                    resumeInfo.Education = (IList<EducationInfo>)await _goodjobContext.Database.GetDbConnection()
                        .QueryAsync<EducationInfo>(sql3);
                }
                //培训经历 My_GetTraining
                // string sql4 = $"SELECT [ID],   [MyUserID], [BeginDate], [EndDate],  [Organization],  [Course],  [Certificate], [EduMemo], FROM [dbo].[My_Training] WHERE [MyuserID]={myUserId} ORDER BY  [BeginDate] DESC ";
                //resumeInfo.Training = (IList<TrainingInfo>)await _goodjobContext.Database.GetDbConnection().QueryAsync<TrainingInfo>(sql4);

                resumeInfo.Training = (IList<TrainingInfo>)await _goodjobContext.Database.GetDbConnection()
                    .QueryAsync<TrainingInfo>("My_GetTraining", parm, commandType: CommandType.StoredProcedure);
                //语言能力  [My_GetLanguage]
                //string sql5 = $"SELECT  [ID], [MyUserID],[LanguageID],[LanguageLevel], [LanguageDescription]  FROM [dbo].[My_Language] WHERE [MyuserID]={myUserId} ";

                resumeInfo.Language = (IList<LanguageInfo>)await _goodjobContext.Database.GetDbConnection()
                    .QueryAsync<LanguageInfo>("My_GetLanguage", parm, commandType: CommandType.StoredProcedure);
                //工作经历
                if (resumeInfo.ResumeWrite.WorkWrite == 2)
                {
                    resumeInfo.WorkText = await _goodjobContext.MyResumeOldTexts.Where(m => m.MyUserId == myUserId)
                        .Select(m => m.WorkText).FirstOrDefaultAsync();
                }
                else
                {
                    //原 My_GetWorkExp
                    resumeInfo.WorkExp = (IList<WorkExpInfo>)await _goodjobContext.Database.GetDbConnection()
                        .QueryAsync<WorkExpInfo>("My_GetWorkExp", parm, commandType: CommandType.StoredProcedure);
                }

                //社會实践 My_GetFulfil
                resumeInfo.Fulfil = (IList<FulfilInfo>)await _goodjobContext.Database.GetDbConnection()
                    .QueryAsync<FulfilInfo>("My_GetFulfil", parm, commandType: CommandType.StoredProcedure);
                //项目管理  My_GetProject
                resumeInfo.Project = (IList<ProjectInfo>)await _goodjobContext.Database.GetDbConnection()
                    .QueryAsync<ProjectInfo>("My_GetProject", parm, commandType: CommandType.StoredProcedure);
                //工作意向 My_GetHopeJob
                using (var reader = await _goodjobContext.Database.GetDbConnection()
                           .ExecuteReaderAsync("My_GetHopeJob", parm, commandType: CommandType.StoredProcedure))
                {
                    if (await reader.ReadAsync())
                    {
                        HopeJobInfo hopeJob = new HopeJobInfo(reader.GetInt32(0), reader.GetString(1), string.Empty,
                            string.Empty, string.Empty, reader.GetInt32(2), reader.GetString(3), reader.GetString(4),
                            (short)reader.GetByte(5), (short)reader.GetByte(6), reader.GetString(7));
                        if (await reader.NextResultAsync())
                        {
                            string jobLocationP = string.Empty;
                            string jobLocationC = string.Empty;

                            while (await reader.ReadAsync())
                            {
                                if (reader.GetInt32(1) == 0)
                                {
                                    jobLocationP += reader.GetInt32(0) + ",";
                                }
                                else
                                {
                                    jobLocationC += reader.GetInt32(1) + ",";
                                }
                            }

                            if (!string.IsNullOrEmpty(jobLocationP))
                            {
                                jobLocationP = jobLocationP.Remove(jobLocationP.Length - 1, 1);
                                hopeJob.JobLocation_P = jobLocationP;
                            }

                            if (!string.IsNullOrEmpty(jobLocationC))
                            {
                                jobLocationC = jobLocationC.Remove(jobLocationC.Length - 1, 1);
                                hopeJob.JobLocation_C = jobLocationC;
                            }
                        }

                        if (await reader.NextResultAsync())
                        {
                            string jobFunction = string.Empty;
                            while (await reader.ReadAsync())
                            {
                                jobFunction += reader.GetInt32(0) + ",";
                            }

                            if (!string.IsNullOrEmpty(jobFunction))
                            {
                                jobFunction = jobFunction.Remove(jobFunction.Length - 1, 1);
                                hopeJob.JobFunction = jobFunction;
                            }
                        }

                        resumeInfo.HopeJob = hopeJob;
                    }
                }

                //技能专长 [My_GetSkill]
                resumeInfo.Skill = await _goodjobContext.Database.GetDbConnection()
                    .QueryFirstOrDefaultAsync<SkillInfo>("My_GetSkill", parm, commandType: CommandType.StoredProcedure);
                //相片[My_GetPhoto]
                using (var reader = await _goodjobContext.Database.GetDbConnection()
                           .ExecuteReaderAsync("My_GetPhoto", parm, commandType: CommandType.StoredProcedure))
                {
                    if (await reader.ReadAsync())
                    {

                        PhotoInfo photo = new PhotoInfo(reader.GetInt32(0),
                            reader[1] is DBNull ? DateTime.MinValue : reader.GetDateTime(1), reader.GetString(2),
                            (short)reader.GetByte(3));
                        resumeInfo.Photo = photo;
                    }
                }
                //resumeInfo.Photo = await _goodjobContext.Database.GetDbConnection().QueryFirstOrDefaultAsync<PhotoInfo>("My_GetSkill", parm, commandType: CommandType.StoredProcedure);
            }
            catch (Exception e)
            {
                LogConfig.TestSetConfig($"查询简历{myUserId}出错啦,详细信息：{e.Message}      \r\n  {e.InnerException}", "查询简历异常");
                return null;
            }

            return resumeInfo;
        }

        public async Task<string> GetExtranetReusmeListQueryIDString(IDictionary dic)
        {
            string idstr = string.Empty;
            string keyWord = string.Empty;
            if (dic.Contains("Keyword"))
            {
                keyWord = dic["Keyword"].ToString();
            }

            var parm = new { Where = FormatListSearchSql(dic), Keyword = keyWord };

            using (var reader = await _goodjobContext.Database.GetDbConnection()
                       .ExecuteReaderAsync("Mng_GetExtranetResumeListQueryID", parm,
                           commandType: CommandType.StoredProcedure))
            {
                while (await reader.ReadAsync())
                {
                    idstr += reader.GetInt32(0).ToString() + ",";
                }
            }

            if (idstr != string.Empty)
            {
                idstr = idstr.Substring(0, idstr.Length - 1);
            }

            return idstr;
        }

        private string FormatListSearchSql(IDictionary dictList)
        {
            if (dictList == null) return null;
            string oneSql = string.Empty;
            string listSql = string.Empty;
            foreach (DictionaryEntry item in dictList)
            {
                switch (item.Key.ToString())
                {

                    case "MyRegisterForCrm":
                        oneSql = string.Format(item.Value.ToString(), FIELD_RESUMEID);
                        break;
                    case "ResumeID":
                        oneSql = string.Format(item.Value.ToString(), FIELD_RESUMEID);
                        break;
                    case "ResumeName":
                        oneSql = string.Format(item.Value.ToString(), FIELD_RESUMENAME);
                        break;
                    case "HopeWork":
                        oneSql = string.Format(item.Value.ToString(), FIELD_HOPEWORK);
                        break;
                    case "HopeArea":
                        oneSql = string.Format(item.Value.ToString(), FIELD_HOPEAREA);
                        break;
                    case "Sex":
                        oneSql = string.Format(item.Value.ToString(), FIELD_SEX);
                        break;
                    case "Age":
                        oneSql = string.Format(item.Value.ToString(), FIELD_AGE);
                        break;
                    case "Degree":
                        oneSql = string.Format(item.Value.ToString(), FIELD_DEGREE);
                        break;
                    case "WorkYear":
                        oneSql = string.Format(item.Value.ToString(), FIELD_WORKYEAR);
                        break;
                    case "LocationP":
                        oneSql = string.Format(item.Value.ToString(), FIELD_LOCATIONP);
                        break;
                    case "LocationC":
                        oneSql = string.Format(item.Value.ToString(), FIELD_LOCATIONC);
                        break;
                    case "HometownP":
                        oneSql = string.Format(item.Value.ToString(), FIELD_HOMETOWNP);
                        break;
                    case "LocationT":
                        oneSql = string.Format(item.Value.ToString(), "Location_T");
                        break;

                    case "HometownC":
                        oneSql = string.Format(item.Value.ToString(), FIELD_HOMETOWNC);
                        break;
                    case "Language":
                        oneSql = string.Format(item.Value.ToString(), FIELD_LANGUAGE);
                        break;
                    case "HavePhoto":
                        oneSql = string.Format(item.Value.ToString(), FIELD_HAVEPHOTO);
                        break;
                    case "HaveEnglish":
                        oneSql = string.Format(item.Value.ToString(), FIELD_HAVEENGLISH);
                        break;
                    case "School":
                        oneSql = string.Format(item.Value.ToString(), FIELD_SCHOOL);
                        break;
                    case "Special":
                        oneSql = string.Format(item.Value.ToString(), FIELD_SPECIAL);
                        break;
                    case "LastPos":
                        oneSql = string.Format(item.Value.ToString(), FIELD_LASTPOS);
                        break;
                    case "UpdateDate":
                        oneSql = string.Format(item.Value.ToString(), FIELD_UPDATEDATE);
                        break;
                    case "ResumeStatus":
                        oneSql = string.Format(item.Value.ToString(), FIELD_RESUMESTATUS);
                        break;
                    case "MobileNum":
                        oneSql = string.Format(item.Value.ToString(), FIELD_MOBILENUM);
                        break;
                    case "MaritalStatus":
                        oneSql = string.Format(item.Value.ToString(), "MaritalStatus");
                        break;

                    case "RegisterFrom":
                        oneSql = item.Value.ToString();
                        break;

                    default:
                        oneSql = "";
                        break;
                }

                listSql += oneSql;
            }

            return listSql;
        }

        public async Task<bool> IsShieldCompany(int myUserId, int posId)
        {
            //原存储过程 [Mng_ShieldCompany]
            int memId = await _queryContext.MemPosQueries.Where(m => m.PosId == posId).Select(m => m.MemId).FirstOrDefaultAsync();
            if (memId == 0) return false;
            var l = await _goodjobContext.MyExcludeMems.Where(m => m.MemId == memId && m.MyUserId == myUserId).FirstOrDefaultAsync();
            return l != null;
        }

        public async Task<int> CommendResumeLog(int posId, string myUserIds, int opertype, string userId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
            parameters.Add("@OperType", value: opertype, dbType: DbType.Int32, direction: ParameterDirection.Input);
            parameters.Add("@PosID", value: posId, dbType: DbType.Int32, direction: ParameterDirection.Input);
            parameters.Add("@ResumeIDStr", value: myUserIds, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add("@SalerID", value: userId, dbType: DbType.Int32, direction: ParameterDirection.Input);

            await _goodjobContext.Database.GetDbConnection().ExecuteAsync("Mng_CommendResumeLog", parameters, commandType: CommandType.StoredProcedure);
            int procedureReturn = parameters.Get<int>("ReturnValue");
            return procedureReturn;
        }

        public async Task CommendResume(int posId, int resumeId, int opertype, string userId)
        {
            var parameters = new { PosID = posId, MyUserID = resumeId, OperType = opertype, SalerID = userId };
            await _goodjobContext.Database.GetDbConnection().ExecuteAsync("Mng_CommendResume", parameters, commandType: CommandType.StoredProcedure);
        }


        public async Task<(string name, string password,short memberClass)> GetNameAndPassWord(int memId)
        {
            //string sql = $" select UserName,PassWord from Mem_Users where MemID={memId}";
            //var result = await _goodjobContext.Database.GetDbConnection().QueryFirstOrDefaultAsync<(string userName, string passWord)>(sql);
            var s = await _goodjobContext.MemUsers.Where(m => m.MemId == memId)
                .Select(m => new { m.UserName, m.PassWord,m.MemberClass }).FirstOrDefaultAsync();
            if (s != null) return (s.UserName,s.PassWord,s.MemberClass);
            return ("", "",0);
        }

        public async Task<IEnumerable<ResumeListQueryInfo>> GetResumeListQueryCollection(int myUserId)
        {
            //待优化，字段过多
            var para = new { idstr = myUserId };
            var result = await _goodjobContext.Database.GetDbConnection().QueryAsync<ResumeListQueryInfo>("Mng_GetResumeListQuery", para, commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<(int memId, string posName, string contactPerson, string email)> GetDataByPosId(int posId)
        {
            var result = await _goodjobContext.MemPositions.Where(m => m.PosId == posId).Select(m => new{ m.MemId ,m.PosName,m.ContactPerson,m.Email}).FirstOrDefaultAsync();
            if (result == null) return (0, "", "", "");
            return (result.MemId, result.PosName, result.ContactPerson, result.Email);
        }

        public async Task<List<KeyValue>> GetCompanyList(string keyWord)
        {
            //if(IsNumeric())
            if (string.IsNullOrEmpty(keyWord))
                return null;
            int.TryParse(keyWord, out int memId);
            if (memId != 0)
            {
                var result = await _goodjobContext.MemInfos.Where(m => m.MemId == memId)
                    .Select(m => new KeyValue { Id = m.MemId, Name = m.MemName }).ToListAsync();
                return result;
            }
            else
            {
                var result = await _goodjobContext.MemInfos.Where(m => m.MemName.Contains(keyWord))
                    .Select(m => new KeyValue { Id = m.MemId, Name = m.MemName }).ToListAsync();
                return result;
            }
        }

        public async Task<List<KeyValue>> GetPosListByMemId(int memId)
        {
            var result = await _goodjobContext.MemPositions.Where(m => m.MemId == memId && m.PosState==2)
                .Select(m => new KeyValue { Id = m.PosId, Name = m.PosName }).ToListAsync();
            return result;
        }

        public async Task<int> SalerDownloadResumeCount(int myUserId, int salerId)
        {
            //原查存储过程 [Mng_GetSalerDownloadCountByMyUserID]
            var i = await _goodjobContext.SysSalerDownloads.Where(s =>
                s.MyUserId == myUserId && s.SalerId == salerId && s.BrowseTime.Date == DateTime.Now.Date).CountAsync();
            return i;
        }

        public async Task<List<string>> GetJobFunction_s(int myUserId)
        {
            //原查存储过程 [My_GetJobFunction_S]
            var s = await (from a in  _goodjobContext.MyJobFunctions1 
                join  b in _otherContext.DicJobFunctionsSmals on a.JobFunctionSmall equals b.Id 
                   where a.MyUserId == myUserId
                    select b.Pname).ToListAsync();
            return s;
           
        }

        public async Task<string> GetJobFunctionListBySmallID(int smallId)
        {

           var s=await _otherContext.DicJobFunctions.Where(d=>d.Id==smallId).Select(d=>d.Pname).FirstOrDefaultAsync();
           return s;
        }

        public async Task<bool> Checkcommendlogo(int posId,int myUserId)
        {
            var m = await _goodjobContext.MemPositions.Where(m => m.PosId == posId).FirstOrDefaultAsync();
            if(m==null) return false;//职位不存在
            var count= await _otherContext.LogCommendResumes.Where(l=>l.MyUserId==myUserId.ToString() && l.MemId==m.MemId).CountAsync();
            return count > 0;
        }

        public async Task<string[]> GetPersonEmail(int[] myUserId)
        {
            //原存储过程 [Mng_GetEmailNameByID]

            var  email = await _goodjobContext.MyResumes.Where(m=> myUserId.Contains(m.MyUserId)).Select(m=>m.Email).ToArrayAsync();
            return email;
        }

        public async Task<PositionInfo> GetPositionById(int posId)
        {
            PositionInfo position = null;
            var param = new { PosID = posId };
            using (var reader = await _goodjobContext.Database.GetDbConnection()
                       .ExecuteReaderAsync("Job_GetPositionByID", param, commandType: CommandType.StoredProcedure))
            {
                if (await reader.ReadAsync())
                {
                    position = new PositionInfo(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), (short)reader.GetByte(5), reader.GetString(6), reader.GetInt32(7), reader.GetDateTime(8), reader.GetString(9), 20, (short)reader.GetByte(11), reader.GetInt32(12), (short)reader.GetByte(13), (short)reader.GetByte(14), reader.GetInt32(15), reader.GetInt32(16), (short)reader.GetByte(17), (short)reader.GetByte(18), (short)reader.GetByte(19), reader.GetString(20), reader.GetString(21), reader.GetString(22), reader.GetString(23), reader.GetString(24), reader.GetString(25), reader.GetString(26), reader.GetBoolean(27), reader.GetString(28), reader.GetString(29), reader.GetString(30), reader.GetBoolean(31), reader.GetString(32), reader.GetBoolean(33), reader.GetByte(34), reader.GetString(35), reader.GetString(36), (short)reader.GetByte(37), reader.GetDateTime(38), reader.GetDateTime(39), reader.GetInt32(40), reader.GetString(41), reader.GetInt16(42));
                }
            }

            //var result = await  _goodjobContext.Database.GetDbConnection().QueryFirstOrDefaultAsync<PositionInfo>("Job_GetPositionByID", param, commandType: CommandType.StoredProcedure);
            return position;
            throw new NotImplementedException();
        }

        public async Task<int[]> CheckResume(int[] resumeIds)
        {
            var s =  await _goodjobContext.MyUsers.Where(m => m.ResumeStatus == 1 && resumeIds.Contains(m.MyUserId)).Select(m=>m.MyUserId).ToArrayAsync();
            return s;
        }

        public async Task<int> GetUserRecommendId(int myUserId)
        {
            int id = await _goodjobContext.MyUsers.Where(m => m.MyUserId == myUserId).Select(m => m.RecommendId)
                .FirstOrDefaultAsync();
            return id;
        }
    }
}
