using System.Collections;
using Goodjob.Common;
using IService.ResumeSearch;
using IService.SalesDepartment;
using Microsoft.AspNetCore.Mvc;
using Model;
using Model.ResumeSearch;

namespace ManageNew.Controllers.ResumeSearch
{
    /// <summary>
    /// 外部简历推荐
    /// </summary>
    [Route("api/ResumeRecommend/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "简历搜索")]
    public class ExtranetResumeRecommendController : ControllerBase
    {

        private readonly IResumeShareVice _resumeShareVice;
        private readonly IResumeRecommend _recommend;
        /// <summary>
        /// 构造方法
        /// </summary>
        public ExtranetResumeRecommendController(IResumeShareVice resumeShareVice, IResumeRecommend recommend)
        {
            _resumeShareVice = resumeShareVice;
            _recommend = recommend;
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="resumeId"></param>
        /// <param name="registerFrom"></param>
        /// <param name="resumeStatus"></param>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetData(int? resumeId,int? registerFrom,string? resumeStatus="", string? keyWord="")
        {
            IDictionary dictList = new Dictionary<string, string>(); 
            string resumeStatusSql = string.Empty;
            string myRegisterForCrmSql = string.Empty;
            string resumeSql = string.Empty;
            string keywordSql = string.Empty;
            if (registerFrom != null)
            {
                myRegisterForCrmSql += " and {0} in(SELECT [MyUserID] FROM [Goodjob].[dbo].[Sys_RegisterFrom] where [RegisterFrom]=" + registerFrom + ") ";
            }
            else
            {
                myRegisterForCrmSql += " and {0} in( SELECT [MyUserID] FROM [Goodjob].[dbo].[Sys_RegisterFrom] ) ";
            }

            if (resumeStatus != null)
            {
                resumeStatusSql = " and {0} in( " + resumeStatus + ")";
            }
            if (resumeId != null)
            {
                resumeSql = " and {0} in (" + resumeId + ")";
            }

            if (!string.IsNullOrEmpty(keyWord))
            {
                if (registerFrom != null && registerFrom == 12)
                {
                    myRegisterForCrmSql += " and {0} in( select sr.MyUserID from Goodjob.dbo.[Sys_RegisterFrom] as sr ";
                    myRegisterForCrmSql += "left join  Goodjob.dbo.My_WorkExp as mw on sr.MyUserID = mw.MyUserID ";
                    myRegisterForCrmSql += "left join Goodjob.dbo.My_Resume as mr on sr.MyUserID = mr.MyUserID ";
                    myRegisterForCrmSql += "where  sr.RegisterFrom = 12 and  ";
                    myRegisterForCrmSql += "((PosName like '%" + keyWord + "%' or [Description] like '%" + keyWord + "%') or mr.PerName like '%" + keyWord + "%' )";
                    myRegisterForCrmSql += ")";
                }
                else
                    keywordSql = PickSearch.PickKeyWord(keyWord);

            }
            if (!string.IsNullOrEmpty(resumeStatusSql)) dictList.Add("ResumeStatus", resumeStatusSql);
            if (!string.IsNullOrEmpty(myRegisterForCrmSql)) dictList.Add("MyRegisterForCrm", myRegisterForCrmSql);
            if (!string.IsNullOrEmpty(resumeSql)) dictList.Add("ResumeID", resumeSql);
            if (!string.IsNullOrEmpty(keywordSql)) dictList.Add("Keyword", keywordSql);
            string str = await _resumeShareVice.GetExtranetReusmeListQueryIDString(dictList);
            if (string.IsNullOrEmpty(str))
                return Ok(ResultMode<object>.Success( new List<ResumeListQueryInfo>()));
            var result = await _recommend.GetExtranetResumeRecommendList(str);
            return Ok(ResultMode<object>.Success(result));
        }
        /// <summary>
        /// 推荐到企业
        /// </summary>
        /// <param name="myUserId"></param>
        /// <param name="memId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> AddCommemd(string myUserId, int memId)
        {
            string userIdStr = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            int.TryParse(userIdStr, out int id);
            var result = await _recommend.AddMemLibCommendNew(memId, myUserId, 1, id);
            if (result == -1)
                return Ok(ResultMode<string>.NotFound("企业不存在"));
            if(result == 0)
                return Ok(ResultMode<string>.Failed("推荐失败"));
            return Ok(ResultMode<bool>.Success(true, "推荐成功"));
        }
    }
}
