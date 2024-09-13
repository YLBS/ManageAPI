using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Security.Cryptography;
using Goodjob.Common;
using IService.ResumeSearch;
using Model;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using ManageNew.Tool;
using Model.enums;
using Model.ResumeSearch;
using Goodjob.Common.Controls;
using IService.SalesDepartment;
using Model.Common;

namespace ManageNew.Controllers.ResumeSearch
{
    /// <summary>
    /// 快速搜索，高级搜索
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "简历搜索")]
    public class ResumeSearchController : ControllerBase
    {

        private readonly IQuickSearch _quickSearch;
        private readonly CheckPermission _checkPermission;
        private readonly IJobEffect _iEffect;
        /// <summary>
        /// 构造方法
        /// </summary>
        public ResumeSearchController(IQuickSearch quickSearch, CheckPermission checkPermission, IJobEffect effect)
        {
            _quickSearch = quickSearch;
            _checkPermission = checkPermission;
            _iEffect = effect;
        }

        /// <summary>
        /// 快速搜索，点击搜索，获取数据，除搜索条件，其他都为可选参数
        /// </summary>
        /// <param name="resumeStatus">搜索条件</param>
        /// <param name="myUserId">按简历编号搜索</param>
        /// <param name="perName">按姓名搜索</param>
        /// <param name="keyWord">关键字搜索</param>
        /// <param name="mobleNum">手机号</param>
        /// <param name="lastPos">最近任职</param>
        /// <param name="exclude">是否排除应聘过的简历，默认false</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetDataQuick(string resumeStatus, int myUserId, string? perName,string? keyWord,string? mobleNum,string? lastPos,bool exclude=false)
        {
            //条件为最近任职时，搜索过慢，待优化 Mng_GetResumeListQueryID_Sub

            string filter =string.Empty;
            filter+= " and ResumeStatus in ( " + resumeStatus + ")";
            if (myUserId != 0)
                filter += " and MyUserID = " + myUserId;
            else if (!string.IsNullOrEmpty(perName))
                filter += " and PerName = '" + perName + "'";
            else if (!string.IsNullOrEmpty(mobleNum))
                filter += "and   MyUserID in( SELECT [MyUserID] FROM [Goodjob].[dbo].[My_Resume] where MobileNum='" + mobleNum + "' ) ";
            else if (!string.IsNullOrEmpty(lastPos))
                filter += "and MyUserID in( select MyUserID from  goodjob_query.dbo.MyResume_Query where LastPosName like '%" + lastPos +"%')";
            else
            {
                filter +=  "and MyUserID in(SELECT [MyUserID] FROM [Goodjob].[dbo].[Sys_RegisterFrom] where [RegisterFrom]= 9)";
            }
            string key=string.Empty;
            if (!string.IsNullOrEmpty(keyWord))
                key = PickSearch.PickKeyWord(keyWord);
            int order = 0;
            if (exclude)
            {
                order = 1;
                filter += "and MyUserID not in (select MyUserID from goodjob_Other.dbo.Log_CommendResume  where  CommendType = 2)";
            }
               
            var result = await _quickSearch.GetData(filter, key, order);
            return Ok(ResultMode<object>.Success(result));

            //IDictionary dictList = new Dictionary<string, string>();
            //dictList.Add("ResumeStatus", " and {0} in( " + resumeStatus + ")");
            //if(myUserId!=0)
            //    dictList.Add("ResumeID", "  and {0} =  " + myUserId);
            //if (!string.IsNullOrEmpty(perName))
            //    dictList.Add("ResumeName", " and {0} = '" + perName + "'");
            //else if (!string.IsNullOrEmpty(keyWord))
            //{
            //    string str = PickSearch.PickKeyWord(keyWord);
            //    if (!string.IsNullOrEmpty(str))
            //        dictList.Add("Keyword",str);
            //}
            //else if(string.IsNullOrEmpty(mobleNum))
            //    dictList.Add("MobileNum", "and   MyUserID in( SELECT [MyUserID] FROM [Goodjob].[dbo].[My_Resume] where  {0}=  '" + mobleNum + "' )  ");
            //else if (!string.IsNullOrEmpty(lastPos))
            //    dictList.Add("LastPos", "and MyUserID in( select MyUserID from  goodjob_query.dbo.MyResume_Query where {0} like '%\" + lastPos + \"%')\"");
            //else
            //{
            //    dictList.Add("RegisterFrom", "and MyUserID in(SELECT [MyUserID] FROM [Goodjob].[dbo].[Sys_RegisterFrom] where [RegisterFrom]= 9)");
            //}
        }
        /// <summary>
        /// ResumeSearchResult.aspx.aspx 检查权限,ExtranetResumeRecommend.aspx
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> CheckPersonViewPermission()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            var result = await _checkPermission.CheckPersonViewPermission(PersonViewEnum.SearchResume, Convert.ToInt32(userId));
            return Ok(ResultMode<bool>.Success(result));
        }
        /// <summary>
        /// 高级搜索，获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="exclude">是否排除已应聘过的简历</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetDataAdvance([FromQuery]AdvanceSearchKey key,bool exclude=false)
        {
            string where1 = string.Empty;//goodjob_query.dbo.MyResume_Query 划分哪些字段是属于这张表的
            string where2 = string.Empty;//  Goodjob.dbo.My_Resume 划分哪些字段是属于这张表的

            string filter1 = " and myUserID in(select  myUserID  from goodjob_query.dbo.MyResume_Query where 1=1 {0})";
            string filter2 = " and myUserID in(select  myUserID  from Goodjob.dbo.My_Resume where 1=1 {0})";

            string filter = string.Empty;
            if (!string.IsNullOrEmpty(key.HopeWork))
            {
                string sql = string.Empty;
                string[] arrType = PickSearch.PickHopeWork(key.HopeWork);
                if (!string.IsNullOrEmpty(arrType[0]))
                {
                    sql = " and MyUserID in (Select MyUserID from My_JobFunction where JobFunction_small in (" + arrType[0] + ") ";
                    if (!string.IsNullOrEmpty(arrType[1]))
                    {
                        sql += " or JobFunction_big in(" + arrType[1] + ")";
                    }
                }
                else if (!string.IsNullOrEmpty(arrType[1]))
                {
                    sql = " and MyUserID in (Select MyUserID from My_JobFunction where JobFunction_small in (" + arrType[1] + ")) ";
                }

                if (!string.IsNullOrEmpty(sql))
                {
                    filter1 += sql;
                }
                
            }
            if(key.Language!=0)
            {
                filter +=
                    " and MyUserID in (Select MyUserID from My_Language where LanguageID = " +
                    key.Language;
                if (key.LanguageLevel != 0)
                {
                    filter += " and LanguageLevel=" + key.LanguageLevel;
                }
                filter += ")"; 
            }

            if (!string.IsNullOrEmpty(key.HopeArea))
            {
                string sql = string.Empty;
                string[] arrType = PickSearch.PickHopeArea(key.HopeArea);
                string cityStr = "," + arrType[1] + ",";
                cityStr = cityStr.Replace(",724,", ",724,1,2,28,25,26,27,").Replace(",725,", ",725,3,4,45,46,47,").Replace(",10,", ",10,726,727,728,729,730,731,732,");
                arrType[1] = cityStr.Substring(1, cityStr.Length - 2);
                if (!string.IsNullOrEmpty(arrType[0]))
                {
                    sql += " where JobLocation_P in (" + arrType[0] + ")";
                    if (!string.IsNullOrEmpty(arrType[1]))
                    {
                        sql += " or JobLocation_C in (" + arrType[1] + ")" + " or (JobLocation_P in (" + arrType[2] + ") and JobLocation_C = 0)";
                    }
                }
                else if (!string.IsNullOrEmpty(arrType[1]))
                    sql += " where JobLocation_C in (" + arrType[1] + ")" + " or (JobLocation_P in (" + arrType[2] + ") and JobLocation_C = 0)";

                if (!string.IsNullOrEmpty(sql))
                    where1 += " and MyUserID in (Select MyUserID from My_JobLocation " + sql + ")";
            }

            if (!string.IsNullOrEmpty(key.LastPos))
                where1 +=
                    " and  LastPosName like '%" + key.LastPos + "%'";
            if (key.WorkYear2 != 0)
            {
                int minWorkYearId = 0;
                int maxWorkYearId = 11;
                if(key.WorkYear1<minWorkYearId || key.WorkYear1>maxWorkYearId) key.WorkYear1 = minWorkYearId;
                if (key.WorkYear2 < minWorkYearId || key.WorkYear2 > maxWorkYearId) key.WorkYear2 = maxWorkYearId;
                if (key.WorkYear1 != minWorkYearId && key.WorkYear2 != maxWorkYearId) //都不等於，取中间
                    where1 += " and WorkedYear between " + key.WorkYear1 + " and " + key.WorkYear2;
                if (key.WorkYear1 == minWorkYearId && key.WorkYear2 != maxWorkYearId)
                    where1 += " and WorkedYear <=" + key.WorkYear2;
                if (key.WorkYear1 != minWorkYearId && key.WorkYear2 == maxWorkYearId)
                    where1 += " and WorkedYear =<" + key.WorkYear1;
            }

            if (key.Sex != 0)
                where1 += " and Sex =" + key.Sex;
            if (key.LocationP != 0)
                where1 += " and Location_P=" + key.LocationP;
            if (key.LocationC != 0 && key.LocationC < 1000)
            {
                string cityStr = key.LocationC.ToString();
                cityStr = cityStr.Replace("724", "724,1,2,28,25,26,27").Replace("725", "725,3,4,45,46,47").Replace("10", "10,726,727,728,729,730,731,732");
                where1 += " and Location_C in(" + cityStr +")";
            }

            if (key.LocationT != 0)
                where1 += " and Location_T=" + key.LocationT;
            int minAgeId = 16;
            int maxAgeId = 61;
            if (key.Age1 != minAgeId && key.Age2 != maxAgeId)
                where1 += " and  datediff(year,Birthday,getdate()) between " + key.Age1 + " and " + key.Age2;
            if (key.HometownP != 0)
                where1 += " and Hometown_P=" + key.HometownP;
            if (key.HometownC != 0)
                where1 += " and Hometown_C=" + key.HometownC;
            if (key.Degree1 != 0 && key.Degree2!=0)
            {
                int minDegreeId = 10;
                int maxDegreeId = 70;
                if (key.Degree1 < minDegreeId || key.Degree1 > maxDegreeId) key.Degree1 = minDegreeId;
                if (key.Degree2 < minDegreeId || key.Degree2 > maxDegreeId) key.Degree2 = maxDegreeId;
                if (key.Degree1 != minDegreeId && key.Degree2 != maxDegreeId) //都不等於，取中间
                    where1 += " and WorkedYear between " + key.Degree1 + " and " + key.Degree2;
                if (key.Degree1 == minDegreeId && key.Degree2 != maxDegreeId)
                    where1 += " and WorkedYear <=" + key.Degree2;
                if (key.Degree1 != minDegreeId && key.Degree2 == maxDegreeId)
                    where1 += " and WorkedYear =<" + key.Degree1;
            }

            if (key.UpdateDate > 0)
                where1 += " and datediff(day,UpdateDate,getdate()) <=" + key.UpdateDate;
            if (!string.IsNullOrEmpty(key.School))
                where1 += " and SchoolName like '%" + key.School + "%'";
            if (!string.IsNullOrEmpty(key.Keyword))
            {
                string str = PickSearch.PickKeyWord(key.Keyword);
                if(str.Length>0)
                    key.Keyword = str.Substring(0,str.Length-1);
            }

            if (!string.IsNullOrEmpty(key.Special))
                where1 += " and Speciality like '%" + key.Special + "%'";
            if (key.MaritalStatus != 0)
                where2 += " and MaritalStatus=" + key.MaritalStatus;
            if (key.HavePhoto == 1)
                where1 += " and PhotoFlag=2";
            if (key.HaveEnglish == 1)
                where1 += " and EngResumeFlag=1";
            if(!string.IsNullOrEmpty(key.ResumeStatus))
                where1 += " and ResumeStatus in ( " + key.ResumeStatus + ")";
            string keyword = string.Empty;
            if (!string.IsNullOrEmpty(key.Keyword))
            {
                keyword = PickKeyWord(key.Keyword);
            }

            
            if (!string.IsNullOrEmpty(where2))
            {
                filter2 = string.Format(filter2, where2);
                filter +=  filter2;
            }

            if (!string.IsNullOrEmpty(where1))
            {
                filter1 = string.Format(filter1, where1);
                filter += filter1;
            }
            int order = 0;
            if (exclude)
            {
                order = 1;
                filter += "and MyUserID not in (select MyUserID from goodjob_Other.dbo.Log_CommendResume  where  CommendType = 2)";
            }
            var result = await _quickSearch.GetData(filter, keyword, key.OrderBy);
            return Ok(ResultMode<object>.Success(result));
        }
        /// <summary>
        /// 排除推荐过的简历，返回推荐过的简历Id,这里的Name为简历Id集合，逗号分割，Id为岗位id
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ExcludeRecommendResume([FromForm] KeyValue p)
        {
            var userIds = p.Name.Split(',').Select(s => int.Parse(s)).ToArray();
            //原 CommendResumeLog(Convert.ToInt32(PosID), item, -100);
            var msg = await _quickSearch.ExcludeRecommendResumeId(userIds, p.Id);
            if (msg == "")
            {
                return Ok(ResultMode<string>.Success("", "所选简历都没有被推荐过"));
            }
            if(msg== "该职位暂未发布")
            {
                return Ok(ResultMode<string>.Failed(msg));
            }
            return Ok(ResultMode<string>.Success(msg,"已推荐简历"));
        }
        /// <summary>
        /// 排除下载过的简历，返回下载过的简历Id,这里的Name为简历Id集合，逗号分割，Id为岗位id
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ExcludeDownResume([FromForm] KeyValue p)
        {
            var userIds = p.Name.Split(',').Select(s => int.Parse(s)).ToArray();
            //原 CommendResumeLog(Convert.ToInt32(PosID), item, -100);
            var msg = await _quickSearch.ExcludeDownResumeId(userIds, p.Id);
            if (msg == "")
            {
                return Ok(ResultMode<string>.Success("", "所选简历都没有被下载过"));
            }
            if (msg == "该职位暂未发布")
            {
                return Ok(ResultMode<string>.Failed(msg));
            }
            return Ok(ResultMode<string>.Success(msg, "已推荐简历"));
        }
        /// <summary>
        /// 排除下载并推荐过的简历。返回未推荐未下载的简历Id,这里的Name为简历Id集合，逗号分割，Id为岗位id
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ExcludeRecommendDownResume([FromForm] KeyValue p)
        {
            var userIds = p.Name.Split(',').Select(s => int.Parse(s)).ToArray();
            //原 CommendResumeLog(Convert.ToInt32(PosID), item, -100);
            var msg = await _quickSearch.ExcludeDownResumeId(userIds, p.Id);
            if (msg == "该职位暂未发布")
            {
                return Ok(ResultMode<string>.Failed(msg));
            }

            if (msg == "") //代表全部未下载，进而判断是否已推荐
            {
                var msg1 = await _quickSearch.ExcludeRecommendResumeId(userIds, p.Id);
                if (msg1 == "") //全部未推荐
                {
                    return Ok(ResultMode<string>.Success("", "所选简历都没有被下载以及推荐过"));
                }
                return Ok(ResultMode<string>.Success(msg1, "已被下载或推荐过"));
            }
            else //将返回以推荐的的简历ID进一步查询是否已下载
            {
                var userId = msg.Split(',').Select(s => int.Parse(s)).ToArray();
                var msg1 = await _quickSearch.ExcludeDownResumeId(userId, p.Id);
                if (msg1 == "") // 没有下载
                {
                    return Ok(ResultMode<string>.Success("", "所选简历都没有被下载以及推荐过"));
                }
                return Ok(ResultMode<string>.Success(msg1, "已被下载或推荐过"));
            }
        }

        /// <summary>
        /// SimulationResume.aspx，获取简历的Id,姓名，邮箱
        /// </summary>
        /// <param name="myUserIds"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetResumeCheckingCollection(string myUserIds)
        {
            int[] ids = myUserIds.Split(",").Select(s => Convert.ToInt32(s)).ToArray();
            var r = await _quickSearch.GetResumeCheckingCollection(ids);
            return Ok(ResultMode<object>.Success(r));
        }
        /// <summary>
        ///  PosCompany.aspx 获取数据 ，简历搜索结果 在招职位查询跳的页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetDataForPosCompany(int posId)
        {
            bool yn = await _quickSearch.CheckPosExist(posId);
            if (yn)
            {
                string userIdStr = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
                var jobFunctionSmall = string.Join(",", await _quickSearch.JobFunctionSmall(posId));
                string filter = " and P.PosID in (select mpjp.PosID from Goodjob_Query.dbo.[Mem_PosJobFunction] as mpjp where mpjp.Jobfunction_small in (" + string.Join(",", jobFunctionSmall) + "))";
                filter += " and P.MemID in (select mu.MemID from Goodjob.dbo.Mem_Users as mu where mu.MemberClass = 2 and mu.SalerUserID = " + userIdStr + ")";
                string order = "PosId";

                var s = await _iEffect.GetJobEffectCollection(filter, order);
                return Ok(ResultMode<object>.Success(new {Data=s.item,Count=s.count}));
            }

            return Ok(ResultMode<string>.Failed("职位不存在"));
        }

        private static string PickKeyWord(string keyword)
        {
            //处理干扰字符
            string[] strTemp;
            char[] chTemp = new char[] { '\'', '"', '\\', '/' };
            strTemp = keyword.Split(chTemp);
            string strTarget = string.Empty;
            for (int i = 0; i < strTemp.Length; i++)
            {
                strTarget += strTemp[i];
            }

            //替换成全文检索表达式
            string fulltextKeyWord = string.Empty;
            if (strTarget != "")
            {
                strTarget = strTarget.Replace(" ", "　").Replace(",", "，").Replace(";", "；");//英文标点替换成中文标点
                fulltextKeyWord = strTarget.Replace("　", " and ").Replace("，", " and ").Replace("；", " or ").Replace("、", " or ");
            }

            return fulltextKeyWord;
        }
    }

}
