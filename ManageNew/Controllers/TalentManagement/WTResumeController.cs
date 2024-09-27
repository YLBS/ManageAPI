using Commons.Tool;
using IService.ResumeSearch;
using IService.TalentManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using System.Xml.Linq;

namespace ManageNew.Controllers.TalentManagement
{
    /// <summary>
    /// 委托简历 WTesumeList.aspx
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "人才管理")]
    public class WTResumeController : ControllerBase
    {
        private readonly IWTResume _resume;
        private readonly IConfiguration _configuration; 
        private readonly IExtranetResumeCheck _extranetResumeCheck;
        /// <summary>
        /// 构造方法
        /// </summary>

        public WTResumeController(IWTResume resume, IConfiguration configuration, IExtranetResumeCheck extranet)
        {
            _resume = resume;
            _configuration = configuration;
            _extranetResumeCheck = extranet;
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="type">提交类型 -1 不限，1 完善简历，2 委托简历</param>
        /// <param name="keyWord">关键字</param>
        /// <param name="beginDate">更新时间</param>
        /// <param name="endDate">更新时间</param>
        /// <param name="dealType">处理状态 1 不限，0 未处理，-1 已处理</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetData(int type,string? keyWord,string? beginDate,string? endDate,int dealType)
        {

            string filter=string.Empty;
            string orderBy = "applyTime desc";
            if (!string.IsNullOrEmpty(keyWord))
                filter = $"and resume_txt like '%{keyWord}%' ";
            if (!string.IsNullOrEmpty(beginDate))
                filter += "  AND applytime > CONVERT(DATETIME,'" + beginDate + "') ";
            if (!string.IsNullOrEmpty(endDate))
                filter += "  AND applytime <DATEADD(DAY, 1,CONVERT(DATETIME, '" + endDate + "')) ";
            if(type==1)
                filter += " and NewsMyuserID>0";
            if(type==2)
                filter += "and NewsMyuserID=0";
            if (dealType == 0)
                filter += " and myuserid=0 "; 
            if (dealType == -1)
                filter += " and myuserid=-1 ";
            filter = " posid=0  " + filter;
            var s = await _resume.GetTableAsync(filter, orderBy);
            return Ok(ResultMode<object>.Success(new { Count = s.count, Data = s.table }));
        }

        /// <summary>
        /// 获取发送短信内容
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetSMSContent(int myUserId,string mobileNum)
        {
            string userName = string.Empty;
            string password = string.Empty;
            if (myUserId > 0)
            {
                var dictionary = await _extranetResumeCheck.GetUserPass(myUserId);
                if (dictionary.Key != null)
                    userName = dictionary.Key.ToString();
                if (dictionary.Value != null)
                    password = dictionary.Value.ToString();
                if (userName == "" && password == "")
                    return NotFound($"简历{myUserId}不存在");
            }
            else
            {
                var s = await _resume.GetMyUserAccount(mobileNum);
                userName=s.userName;
                password=s.password;
            }
            string content = _configuration["GetSMSContentForWTesume"];
            string result=string.Format(content, userName, password);
            return Ok(ResultMode<string>.Success(result));

        }
        /// <summary>
        /// 更新简历处理状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="myUserId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> UpdateDealWithStatus(int id, int myUserId)
        {
            bool yn = await _resume.UpdateDealWithStatus(id, myUserId);
            if (yn)
                return Ok(ResultMode<string>.Success("", "更新成功"));
            return Ok(ResultMode<string>.Failed("更新失败"));
        }
        /// <summary>
        /// 获取html
        /// </summary>
        /// <param name="newsMyUserId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetUrlHtml(int newsMyUserId)
        {
            var str = await _extranetResumeCheck.GetUserPass(newsMyUserId);
            if (str.Key == null)
            {
                return NotFound($"简历{newsMyUserId}不存在");
            }
            str.Key = CompanyTool.EncryptDESTool(str.Key.ToString());
            if (str.Value != null)
                str.Value = CompanyTool.EncryptDESTool(str.Value.ToString());

            string mobile = await _resume.GetAllWriteName(newsMyUserId);
            string html = $"{mobile},{str.Key},{str.Value}";
            return Ok(ResultMode<string>.Success(html));
        }
        /// <summary>
        /// 查看简历
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetResumeInfo(int id)
        {
            var s = await _resume.GetResumeInfoById(id);
            if (s == null)
                s = "<p>简历不存在</p>";
            return Ok(ResultMode<string>.Success(s));
        }
    }
}
