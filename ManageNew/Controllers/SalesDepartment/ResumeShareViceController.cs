using System.Collections;
using Goodjob.Resume;
using IService.SalesDepartment;
using ManageNew.Tool;
using Microsoft.AspNetCore.Mvc;
using Model;
using Model.Common;
using Model.enums;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using IService.Common;
using Commons.Tool;
//using Goodjob.Common;
using System.Diagnostics;
using Commons.Send;
using Goodjob.Common;
using Mail = Commons.Send.Mail;
using ServiceStack.Script;

namespace ManageNew.Controllers.SalesDepartment
{
    /// <summary>
    /// 内部简历
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "销售部专用")]
    public class ResumeShareViceController : ControllerBase
    {
        private readonly IResumeShareVice _resumeShareVice;
        private readonly CheckPermission _checkPermission;
        private readonly IDicService _dicService;
        private readonly Commons.Helper.TempletHelper _templetHelper;
        private readonly Commons.Helper.HtmlHelper _htmlHelper;
        private readonly IConfiguration _configuration;

        private readonly Commons.Send.Mail _mail;
        //private readonly IHostingEnvironment _hostingEnvironment;
        /// <summary>
        /// 构造方法
        /// </summary>

        public ResumeShareViceController(IResumeShareVice resumeShareVice, CheckPermission checkPermission, IDicService dicService, Commons.Helper.TempletHelper templetHelper, Commons.Helper.HtmlHelper htmlHelper,IConfiguration iConfiguration, Commons.Send.Mail mail)
        {
            _resumeShareVice = resumeShareVice;
            _checkPermission = checkPermission;
            _dicService = dicService;
            _templetHelper = templetHelper;
            _htmlHelper = htmlHelper;
            _configuration = iConfiguration;
            _mail = mail;
        }
        /// <summary>
        /// 获取来源为百度的简历简历列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetViceResume([FromBody] Singles filter)
        {
            var list = await _resumeShareVice.GetViceResume(filter);
            return Ok(ResultMode<object>.Success(new { Data = list.item, Count = list.cout}));
        }
        /// <summary>
        /// 获取来源不是百度的简历列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetResumeShareViceList([FromBody] Singles filter)
        {
            var list = await _resumeShareVice.GetResumeShareViceList(filter);
            return Ok(ResultMode<object>.Success(new { Data = list.item, Count = list.cout }));
        }
        /// <summary>
        /// ViewResumeOne.aspx 检查查看简历权限
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> CheckPersonViewPermission()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            var result = await _checkPermission.CheckPersonViewPermission(PersonViewEnum.ViewResume, Convert.ToInt32(userId));
            return Ok(ResultMode<bool>.Success(result));
        }
        /// <summary>
        /// ViewResumeOne.aspx 获取人才简历
        /// </summary>
        /// <param name="myUserId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetResume(int myUserId)
        {
            var list = await _resumeShareVice.GetResume(myUserId);
            return Ok(ResultMode<object>.Success(list));
        }
        /// <summary>
        /// ViewResumeOne.aspx 点击模拟应聘，未测试
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ApplyClick(int myUserId,int posId)
        {
            IDictionary dic= new  Dictionary<string, string>();
            string MyRegisterForCrmSql = " and {0} in(SELECT [MyUserID] FROM [Goodjob].[dbo].[Sys_RegisterFrom] where [RegisterFrom]=" + 12 + " and MyUserID = " + myUserId + ") ";
            dic.Add("MyRegisterForCrm", MyRegisterForCrmSql);
            string str = await _resumeShareVice.GetExtranetReusmeListQueryIDString(dic);
            if (!string.IsNullOrEmpty(str))
            {
                return Ok(ResultMode<string>.Failed(""));
            }
            bool yn = await _resumeShareVice.IsShieldCompany(myUserId, posId);
            if (!yn)
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
                int i = await _resumeShareVice.CommendResumeLog(posId, myUserId.ToString(), 2, userId);
                switch (i)
                {
                    case 0:
                        await _resumeShareVice.CommendResume(posId, myUserId,2, userId);
                        await SenMailPos(posId, myUserId);
                        return Ok(ResultMode<string>.Success("", "发送成功"));
                    case 1:
                        return Ok(ResultMode<string>.NotFound("职位无效"));
                }
            }
            return Ok(ResultMode<string>.Failed("返回失败"));
        }
        /// <summary>
        /// ViewResumeOne.aspx  简历详情，搜索企业
        /// </summary>
        /// <param name="keyWord">搜搜关键字，为数字时查询Id,否则为企业名称</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetCompanyList(string keyWord)
        {
            var r = await _resumeShareVice.GetCompanyList(keyWord);
            return Ok(ResultMode<object>.Success(r));
        }
        /// <summary>
        /// ViewResumeOne.aspx  简历详情，搜索企业后，获取岗位
        /// </summary>
        /// <param name="memId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetPosListByMemId(int memId)
        {
            var r = await _resumeShareVice.GetPosListByMemId(memId);
            return Ok(ResultMode<object>.Success(r));
        }
        /// <summary>
        /// ViewResumeOne.aspx 推荐给企业 未测试，SimulationResume.aspx 推荐给企业，未测试
        /// </summary>
        /// <param name="myUserId"></param>
        /// <param name="posId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ButCommendClick(int myUserId, int posId)
        {
            //判断 屏蔽该职位，简历编号
            bool YN = await _resumeShareVice.IsShieldCompany(myUserId, posId);
            if (!YN)
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
                int i = await _resumeShareVice.CommendResumeLog(posId, myUserId.ToString(), 1, userId);
                switch (i)
                {
                    case 0:
                        await _resumeShareVice.CommendResume(posId, myUserId, 1, userId);

                        await SenMailcommendPos(posId, myUserId);
                        return Ok(ResultMode<string>.Success("", "发送成功"));
                    case 1:
                        return Ok(ResultMode<string>.NotFound("职位无效"));
                }
            }
            return Ok(ResultMode<string>.Failed("无"));
        }
        /// <summary>
        /// ViewResumeOne.aspx 读取带关键字搜索的简历(限制联系方式输出)
        /// </summary>
        /// <param name="myUserId"></param>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetResumeKeyWordText(int myUserId, string keyWord)
        {
            ResumeInfo resume = await _resumeShareVice.GetResume(myUserId);
            if (resume == null) return Ok(ResultMode<string>.Success(string.Empty));
            //简历不完整的不显示
            //if (resume.ResumeWrite.AllWrite == 0) return null;

            string htmlString = string.Empty;
            string dataStr = string.Empty;
            string tempStr = string.Empty;
            string loopStr = string.Empty;
            string loopAllStr;

            #region Resume Templet
            string templet = _templetHelper.GetTemplet(Commons.Helper.TempletType.ResumeTemplet);
            templet = templet.Replace("{MyUserID}", resume.MyUserID.ToString());
            templet = templet.Replace("{UpdateDate}", resume.UpdateDateName);
            templet = templet.Replace("{HitCount}", resume.HitCount.ToString());
            templet = templet.Replace("{EngResumeFlag}", resume.EngResumeFlag.ToString());
            templet = templet.Replace("{PhotoVisitUrl}", resume.Photo.PhotoVisitUrl);
            #endregion

            # region BaseInfo(*)
            if (resume.ResumeWrite.BaseWrite == 1)
            {
                htmlString = GetLabelContent(templet, "<!--BaseInfo Begin-->", "<!--BaseInfo End-->");
                htmlString = htmlString.Replace("{Age}", resume.BaseInfo.Age.ToString());
                htmlString = htmlString.Replace("{Birthday}", resume.BaseInfo.BirthdayName);
                htmlString = htmlString.Replace("{CardNum}", resume.BaseInfo.CardNum);
                htmlString = htmlString.Replace("{CardTypeName}", resume.BaseInfo.CardTypeName);
                htmlString = htmlString.Replace("{DegreeName}", GetReplaceKey(resume.BaseInfo.DegreeName, keyWord));
                htmlString = htmlString.Replace("{Height}", resume.BaseInfo.Height.ToString());
                htmlString = htmlString.Replace("{Hometown}", GetReplaceKey(resume.BaseInfo.Hometown, keyWord));
                htmlString = htmlString.Replace("{Location}", GetReplaceKey(resume.BaseInfo.Location, keyWord));
                htmlString = htmlString.Replace("{MaritalName}", resume.BaseInfo.MaritalName);
                htmlString = htmlString.Replace("{NationalityName}", resume.BaseInfo.NationalityName);
                htmlString = htmlString.Replace("{PerName}", GetReplaceKey(resume.BaseInfo.PerName, keyWord));
                htmlString = htmlString.Replace("{SelfDescription}", GetReplaceKey(resume.BaseInfo.SelfDescription, keyWord));
                htmlString = htmlString.Replace("{SexCN}", resume.BaseInfo.SexCN);
                htmlString = htmlString.Replace("{Weight}", resume.BaseInfo.Weight.ToString());
                htmlString = htmlString.Replace("{WorkExperience}", resume.BaseInfo.WorkExperience);
            }
            else
            {
                htmlString = string.Empty;
            }
            templet = ReplaceLabelContent(templet, "<!--BaseInfo Begin-->", "<!--BaseInfo End-->", htmlString);
            #endregion

            # region Education(*)
            htmlString = GetLabelContent(templet, "<!--Education Begin-->", "<!--Education End-->");
            switch (resume.ResumeWrite.EduWrite)
            {
                case 2:
                    dataStr = GetLabelContent(htmlString, "<!--OldData Begin-->", "<!--OldData End-->");
                    dataStr = dataStr.Replace("{EduText}", GetReplaceKey(resume.EduText, keyWord));
                    htmlString = ReplaceLabelContent(htmlString, "<!--DataCase Begin-->", "<!--DataCase End-->", dataStr);
                    break;
                case 1:
                    dataStr = GetLabelContent(htmlString, "<!--NewData Begin-->", "<!--NewData End-->");
                    loopStr = GetLabelContent(dataStr, "<!--Loop Begin-->", "<!--Loop End-->");
                    loopAllStr = string.Empty;
                    foreach (EducationInfo education in resume.Education)
                    {
                        tempStr = loopStr;
                        tempStr = tempStr.Replace("{BeginDateName}", education.BeginDateName);
                        tempStr = tempStr.Replace("{Certificate}", education.Certificate);
                        tempStr = tempStr.Replace("{DegreeName}", GetReplaceKey(education.DegreeName, keyWord));
                        tempStr = tempStr.Replace("{EndDateName}", education.EndDateName);
                        tempStr = tempStr.Replace("{SchoolName}", GetReplaceKey(education.SchoolName, keyWord));
                        tempStr = tempStr.Replace("{Speciality}", GetReplaceKey(education.Speciality, keyWord));
                        tempStr = tempStr.Replace("{SpecialityMemo}", GetReplaceKey(education.SpecialityMemo, keyWord));
                        loopAllStr += tempStr;
                    }
                    dataStr = ReplaceLabelContent(dataStr, "<!--Loop Begin-->", "<!--Loop End-->", loopAllStr);
                    htmlString = ReplaceLabelContent(htmlString, "<!--DataCase Begin-->", "<!--DataCase End-->", dataStr);
                    break;
                default:
                    htmlString = string.Empty;
                    break;
            }
            templet = ReplaceLabelContent(templet, "<!--Education Begin-->", "<!--Education End-->", htmlString);
            #endregion

            # region WorkExp(*)
            htmlString = GetLabelContent(templet, "<!--WorkExp Begin-->", "<!--WorkExp End-->");
            switch (resume.ResumeWrite.WorkWrite)
            {
                case 2:
                    dataStr = GetLabelContent(htmlString, "<!--OldData Begin-->", "<!--OldData End-->");
                    dataStr = dataStr.Replace("{WorkText}", GetReplaceKey(resume.WorkText, keyWord));
                    htmlString = ReplaceLabelContent(htmlString, "<!--DataCase Begin-->", "<!--DataCase End-->", dataStr);
                    break;
                case 1:
                    dataStr = GetLabelContent(htmlString, "<!--NewData Begin-->", "<!--NewData End-->");
                    loopStr = GetLabelContent(dataStr, "<!--Loop Begin-->", "<!--Loop End-->");
                    loopAllStr = string.Empty;
                    foreach (WorkExpInfo workExp in resume.WorkExp)
                    {
                        try
                        {

                            tempStr = loopStr;
                            tempStr = tempStr.Replace("{BeginDateName}", workExp.BeginDateName);
                            tempStr = tempStr.Replace("{ComCallingName}", GetReplaceKey(workExp.ComCallingName, keyWord));
                            tempStr = tempStr.Replace("{ComIntroduction}", GetReplaceKey(workExp.ComIntroduction, keyWord));
                            tempStr = tempStr.Replace("{ComName}", GetReplaceKey(workExp.ComName, keyWord));
                            tempStr = tempStr.Replace("{ComSizeName}", GetReplaceKey(workExp.ComSizeName, keyWord));
                            tempStr = tempStr.Replace("{ComTypeName}", GetReplaceKey(workExp.ComTypeName, keyWord));
                            tempStr = tempStr.Replace("{Description}", GetReplaceKey(workExp.Description, keyWord));
                            tempStr = tempStr.Replace("{EndDateName}", workExp.EndDateName);

                            if (workExp.LeftReason != string.Empty)
                            { tempStr = tempStr.Replace("{LeftReason}", workExp.LeftReason); }
                            else
                            { tempStr = ReplaceLabelContent(tempStr, "<!--LeftReason Begin-->", "<!--LeftReason End-->", string.Empty); }

                            tempStr = tempStr.Replace("{PosName}", GetReplaceKey(workExp.PosName, keyWord));
                            loopAllStr += tempStr;
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    dataStr = ReplaceLabelContent(dataStr, "<!--Loop Begin-->", "<!--Loop End-->", loopAllStr);
                    htmlString = ReplaceLabelContent(htmlString, "<!--DataCase Begin-->", "<!--DataCase End-->", dataStr);
                    break;
                default:
                    htmlString = string.Empty;
                    break;
            }
            templet = ReplaceLabelContent(templet, "<!--WorkExp Begin-->", "<!--WorkExp End-->", htmlString);
            #endregion

            # region HopeJob(*)
            if (resume.ResumeWrite.HopeWrite == 1)
            {
                htmlString = GetLabelContent(templet, "<!--HopeJob Begin-->", "<!--HopeJob End-->");
                htmlString = htmlString.Replace("{CareerDirection}", GetReplaceKey(resume.HopeJob.CareerDirection, keyWord));
                htmlString = htmlString.Replace("{CheckinDateName}", GetReplaceKey(resume.HopeJob.CheckinDateName, keyWord));
                htmlString = htmlString.Replace("{EatHouseNeeded}", GetReplaceKey(resume.HopeJob.EatHouseNeeded, keyWord));
                htmlString = htmlString.Replace("{JobSeeking}", GetReplaceKey(resume.HopeJob.JobSeeking, keyWord));
                htmlString = htmlString.Replace("{JobTypeName}", GetReplaceKey(resume.HopeJob.JobTypeName, keyWord));
                htmlString = htmlString.Replace("{OtherNeeded}", GetReplaceKey(resume.HopeJob.OtherNeeded, keyWord));

               // List<DicSalaryNew> dicSalaryNews = PosQueryDAL.GetSalaryNew();
                var dicSalaryNews = await _dicService.GetSalaryNew();
                var salaryName = "";
                if (resume.HopeJob.Salary == 20)
                {
                    salaryName = "面议";
                }
                else
                {
                    salaryName= dicSalaryNews.Where(s=>s.Id== resume.HopeJob.Salary).Select(s=>s.Name).FirstOrDefault();
                    //foreach (DicSalaryNew salaryNew in dicSalaryNews)
                    //{
                    //    if (resume.HopeJob.Salary == salaryNew.Id)
                    //    {
                    //        salaryName = salaryNew.Name;
                    //    }
                    //}
                }
                htmlString = htmlString.Replace("{SalaryName}", GetReplaceKey(salaryName, keyWord));

                //htmlString = htmlString.Replace("{SalaryName}", GetReplaceKey(resume.HopeJob.SalaryName,keyWord));
                htmlString = htmlString.Replace("{JobFunctionName}", GetReplaceKey(resume.HopeJob.JobFunctionName, keyWord));
                htmlString = htmlString.Replace("{JobLocationName}", GetReplaceKey(resume.HopeJob.JobLocationName, keyWord));
            }
            else
            {
                htmlString = string.Empty;
            }
            templet = ReplaceLabelContent(templet, "<!--HopeJob Begin-->", "<!--HopeJob End-->", htmlString);
            #endregion

            # region Contact(*)
            htmlString = GetLabelContent(templet, "<!--Contact Begin-->", "<!--Contact End-->");
            var userId = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            if (await HasSalerDownloadResume(myUserId, Convert.ToInt32(userId)))
            {
                loopAllStr = GetLabelContent(htmlString, "<!--Enabled Begin-->", "<!--Enabled End-->");
                loopAllStr = loopAllStr.Replace("{Address}", GetReplaceKey(resume.BaseInfo.Contact.Address, keyWord));
                loopAllStr = loopAllStr.Replace("{ContactPhone}", resume.BaseInfo.Contact.ContactPhone);
                loopAllStr = loopAllStr.Replace("{Email}", resume.BaseInfo.Contact.Email);
                loopAllStr = loopAllStr.Replace("{Homepage}", resume.BaseInfo.Contact.Homepage);
                loopAllStr = loopAllStr.Replace("{IMNum}", resume.BaseInfo.Contact.IMNum);
                loopAllStr = loopAllStr.Replace("{MobileNum}", resume.BaseInfo.Contact.MobileNum);
                loopAllStr = loopAllStr.Replace("{ZipCode}", resume.BaseInfo.Contact.ZipCode);
                templet = templet.Replace("{IsContactEnabled}", "1");
            }
            else
            {
                loopAllStr = GetLabelContent(htmlString, "<!--Disabled Begin-->", "<!--Disabled End-->");
                templet = templet.Replace("{IsContactEnabled}", "0");
            }

            htmlString = ReplaceLabelContent(htmlString, "<!--IsContactEnabled Begin-->", "<!--IsContactEnabled End-->", loopAllStr);
            templet = ReplaceLabelContent(templet, "<!--Contact Begin-->", "<!--Contact End-->", htmlString);
            #endregion

            # region Fulfil
            if (resume.ResumeWrite.FulfilWrite == 1)
            {
                htmlString = GetLabelContent(templet, "<!--Fulfil Begin-->", "<!--Fulfil End-->");
                loopStr = GetLabelContent(htmlString, "<!--Loop Begin-->", "<!--Loop End-->");
                loopAllStr = string.Empty;
                foreach (FulfilInfo fulfil in resume.Fulfil)
                {
                    tempStr = loopStr;
                    tempStr = tempStr.Replace("{BeginDateName}", fulfil.BeginDateName);
                    tempStr = tempStr.Replace("{ComName}", GetReplaceKey(fulfil.ComName, keyWord));
                    tempStr = tempStr.Replace("{Description}", GetReplaceKey(fulfil.Description, keyWord));
                    tempStr = tempStr.Replace("{EndDateName}", fulfil.EndDateName);
                    loopAllStr += tempStr;
                }
                htmlString = ReplaceLabelContent(htmlString, "<!--Loop Begin-->", "<!--Loop End-->", loopAllStr);
            }
            else
            {
                htmlString = string.Empty;
            }
            templet = ReplaceLabelContent(templet, "<!--Fulfil Begin-->", "<!--Fulfil End-->", htmlString);
            #endregion

            # region Training
            if (resume.ResumeWrite.TrainingWrite == 1)
            {
                htmlString = GetLabelContent(templet, "<!--Training Begin-->", "<!--Training End-->");
                loopStr = GetLabelContent(htmlString, "<!--Loop Begin-->", "<!--Loop End-->");
                loopAllStr = string.Empty;
                foreach (TrainingInfo training in resume.Training)
                {
                    tempStr = loopStr;
                    tempStr = tempStr.Replace("{BeginDateName}", training.BeginDateName);
                    tempStr = tempStr.Replace("{Certificate}", training.Certificate);
                    tempStr = tempStr.Replace("{Course}", training.Course);
                    tempStr = tempStr.Replace("{EduMemo}", GetReplaceKey(training.EduMemo, keyWord));
                    tempStr = tempStr.Replace("{EndDateName}", training.EndDateName);
                    tempStr = tempStr.Replace("{Organization}", training.Organization);
                    loopAllStr += tempStr;
                }
                htmlString = ReplaceLabelContent(htmlString, "<!--Loop Begin-->", "<!--Loop End-->", loopAllStr);
            }
            else
            {
                htmlString = string.Empty;
            }
            templet = ReplaceLabelContent(templet, "<!--Training Begin-->", "<!--Training End-->", htmlString);
            #endregion

            # region Language
            if (resume.ResumeWrite.LanguageWrite == 1)
            {
                htmlString = GetLabelContent(templet, "<!--Language Begin-->", "<!--Language End-->");
                loopStr = GetLabelContent(htmlString, "<!--Loop Begin-->", "<!--Loop End-->");
                loopAllStr = string.Empty;
                foreach (LanguageInfo language in resume.Language)
                {
                    tempStr = loopStr;
                    tempStr = tempStr.Replace("{LanguageLevelName}", GetReplaceKey(language.LanguageLevelName, keyWord));
                    tempStr = tempStr.Replace("{LanguageName}", GetReplaceKey(language.LanguageName, keyWord));
                    tempStr = tempStr.Replace("{LanguageDescription}", GetReplaceKey(language.LanguageDescription, keyWord));
                    loopAllStr += tempStr;
                }
                htmlString = ReplaceLabelContent(htmlString, "<!--Loop Begin-->", "<!--Loop End-->", loopAllStr);
            }
            else
            {
                htmlString = string.Empty;
            }
            templet = ReplaceLabelContent(templet, "<!--Language Begin-->", "<!--Language End-->", htmlString);
            #endregion

            # region Skill
            if (resume.ResumeWrite.SkillWrite == 1)
            {
                htmlString = GetLabelContent(templet, "<!--Skill Begin-->", "<!--Skill End-->");
                htmlString = htmlString.Replace("{ComputerLevelName}", GetReplaceKey(resume.Skill.ComputerLevelName, keyWord));
                htmlString = htmlString.Replace("{ComputerSkills}", GetReplaceKey(resume.Skill.ComputerSkills, keyWord));
                htmlString = htmlString.Replace("{Interesting}", GetReplaceKey(resume.Skill.Interesting, keyWord));
                htmlString = htmlString.Replace("{OtherSkills}", GetReplaceKey(resume.Skill.OtherSkills, keyWord));
                htmlString = htmlString.Replace("{PositionName}", GetReplaceKey(resume.Skill.PositionName, keyWord));
            }
            else
            {
                htmlString = string.Empty;
            }
            templet = ReplaceLabelContent(templet, "<!--Skill Begin-->", "<!--Skill End-->", htmlString);
            #endregion

            # region Project
            if (resume.ResumeWrite.ProjectWrite == 1)
            {
                htmlString = GetLabelContent(templet, "<!--Project Begin-->", "<!--Project End-->");
                loopStr = GetLabelContent(htmlString, "<!--Loop Begin-->", "<!--Loop End-->");
                loopAllStr = string.Empty;
                foreach (ProjectInfo project in resume.Project)
                {
                    tempStr = loopStr;
                    tempStr = tempStr.Replace("{BeginDateName}", project.BeginDateName);
                    tempStr = tempStr.Replace("{EndDateName}", GetReplaceKey(project.EndDateName, keyWord));
                    tempStr = tempStr.Replace("{ProjectDuty}", GetReplaceKey(project.ProjectDuty, keyWord));
                    tempStr = tempStr.Replace("{ProjectMemo}", GetReplaceKey(project.ProjectMemo, keyWord));
                    tempStr = tempStr.Replace("{ProjectName}", GetReplaceKey(project.ProjectName, keyWord));
                    loopAllStr += tempStr;
                }
                htmlString = ReplaceLabelContent(htmlString, "<!--Loop Begin-->", "<!--Loop End-->", loopAllStr);
            }
            else
            {
                htmlString = string.Empty;
            }
            templet = ReplaceLabelContent(templet, "<!--Project Begin-->", "<!--Project End-->", htmlString);
            #endregion

            return Ok(ResultMode<string>.Success(templet));
        }
        /// <summary>
        /// ViewResumeOne.aspx 读取简历(限制联系方式输出)
        /// </summary>
        /// <param name="myUserId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetResumeText(int myUserId)
        {
            ResumeInfo resume = await _resumeShareVice.GetResume(myUserId);
            if (resume == null)  return Ok(ResultMode<string>.Success(""));
            
            string htmlString = string.Empty;
            string dataStr = string.Empty;
            string tempStr = string.Empty;
            string loopStr = string.Empty;
            string loopAllStr;

            #region Resume Templet
            string templet = _templetHelper.GetTemplet(Commons.Helper.TempletType.ResumeTemplet);
            templet = templet.Replace("{MyUserID}", resume.MyUserID.ToString());
            templet = templet.Replace("{UpdateDate}", resume.UpdateDateName);
            templet = templet.Replace("{HitCount}", resume.HitCount.ToString());
            templet = templet.Replace("{EngResumeFlag}", resume.EngResumeFlag.ToString());
            templet = templet.Replace("{PhotoVisitUrl}", resume.Photo.PhotoVisitUrl);
            #endregion

            # region BaseInfo(*)
            if (resume.ResumeWrite.BaseWrite == 1)
            {
                htmlString = GetLabelContent(templet, "<!--BaseInfo Begin-->", "<!--BaseInfo End-->");
                htmlString = htmlString.Replace("{Age}", resume.BaseInfo.Age.ToString());
                htmlString = htmlString.Replace("{Birthday}", resume.BaseInfo.BirthdayName);
                htmlString = htmlString.Replace("{CardNum}", resume.BaseInfo.CardNum);
                htmlString = htmlString.Replace("{CardTypeName}", resume.BaseInfo.CardTypeName);
                htmlString = htmlString.Replace("{DegreeName}", resume.BaseInfo.DegreeName);
                htmlString = htmlString.Replace("{Height}", resume.BaseInfo.Height.ToString());
                htmlString = htmlString.Replace("{Hometown}", resume.BaseInfo.Hometown);
                htmlString = htmlString.Replace("{Location}", resume.BaseInfo.Location);
                htmlString = htmlString.Replace("{MaritalName}", resume.BaseInfo.MaritalName);
                htmlString = htmlString.Replace("{NationalityName}", resume.BaseInfo.NationalityName);
                htmlString = htmlString.Replace("{PerName}", resume.BaseInfo.PerName);
                htmlString = htmlString.Replace("{SelfDescription}", FormatHtmlString(resume.BaseInfo.SelfDescription, true));
                htmlString = htmlString.Replace("{SexCN}", resume.BaseInfo.SexCN);
                htmlString = htmlString.Replace("{Weight}", resume.BaseInfo.Weight.ToString());
                htmlString = htmlString.Replace("{WorkExperience}", resume.BaseInfo.WorkExperience);
            }
            else
            {
                htmlString = string.Empty;
            }
            templet = ReplaceLabelContent(templet, "<!--BaseInfo Begin-->", "<!--BaseInfo End-->", htmlString);
            #endregion

            # region Education(*)
            htmlString = GetLabelContent(templet, "<!--Education Begin-->", "<!--Education End-->");
            switch (resume.ResumeWrite.EduWrite)
            {
                case 2:
                    dataStr = GetLabelContent(htmlString, "<!--OldData Begin-->", "<!--OldData End-->");
                    dataStr = dataStr.Replace("{EduText}", FormatHtmlString(resume.EduText, true));
                    htmlString = ReplaceLabelContent(htmlString, "<!--DataCase Begin-->", "<!--DataCase End-->", dataStr);
                    break;
                case 1:
                    dataStr = GetLabelContent(htmlString, "<!--NewData Begin-->", "<!--NewData End-->");
                    loopStr = GetLabelContent(dataStr, "<!--Loop Begin-->", "<!--Loop End-->");
                    loopAllStr = string.Empty;
                    foreach (EducationInfo education in resume.Education)
                    {
                        tempStr = loopStr;
                        tempStr = tempStr.Replace("{BeginDateName}", education.BeginDateName);
                        tempStr = tempStr.Replace("{Certificate}", education.Certificate);
                        tempStr = tempStr.Replace("{DegreeName}", education.DegreeName);
                        tempStr = tempStr.Replace("{EndDateName}", education.EndDateName);
                        tempStr = tempStr.Replace("{SchoolName}", education.SchoolName);
                        tempStr = tempStr.Replace("{Speciality}", education.Speciality);
                        tempStr = tempStr.Replace("{SpecialityMemo}", education.SpecialityMemo);
                        loopAllStr += tempStr;
                    }
                    dataStr = ReplaceLabelContent(dataStr, "<!--Loop Begin-->", "<!--Loop End-->", loopAllStr);
                    htmlString = ReplaceLabelContent(htmlString, "<!--DataCase Begin-->", "<!--DataCase End-->", dataStr);
                    break;
                default:
                    htmlString = string.Empty;
                    break;
            }
            templet = ReplaceLabelContent(templet, "<!--Education Begin-->", "<!--Education End-->", htmlString);
            #endregion

            # region WorkExp(*)
            htmlString = GetLabelContent(templet, "<!--WorkExp Begin-->", "<!--WorkExp End-->");
            switch (resume.ResumeWrite.WorkWrite)
            {
                case 2:
                    dataStr = GetLabelContent(htmlString, "<!--OldData Begin-->", "<!--OldData End-->");
                    dataStr = dataStr.Replace("{WorkText}", FormatHtmlString(resume.WorkText, true));
                    htmlString = ReplaceLabelContent(htmlString, "<!--DataCase Begin-->", "<!--DataCase End-->", dataStr);
                    break;
                case 1:
                    dataStr = GetLabelContent(htmlString, "<!--NewData Begin-->", "<!--NewData End-->");
                    loopStr = GetLabelContent(dataStr, "<!--Loop Begin-->", "<!--Loop End-->");
                    loopAllStr = string.Empty;
                    foreach (WorkExpInfo workExp in resume.WorkExp)
                    {
                        try
                        {
                            tempStr = loopStr;
                            tempStr = tempStr.Replace("{BeginDateName}", workExp.BeginDateName);
                            tempStr = tempStr.Replace("{ComCallingName}", workExp.ComCallingName);
                            tempStr = tempStr.Replace("{ComIntroduction}", FormatHtmlString(workExp.ComIntroduction, true));
                            tempStr = tempStr.Replace("{ComName}", workExp.ComName);
                            tempStr = tempStr.Replace("{ComSizeName}", workExp.ComSizeName);
                            tempStr = tempStr.Replace("{ComTypeName}", workExp.ComTypeName);
                            tempStr = tempStr.Replace("{Description}", FormatHtmlString(workExp.Description, true));
                            tempStr = tempStr.Replace("{EndDateName}", workExp.EndDateName);

                            if (workExp.LeftReason != string.Empty)
                            { tempStr = tempStr.Replace("{LeftReason}", workExp.LeftReason); }
                            else
                            { tempStr = ReplaceLabelContent(tempStr, "<!--LeftReason Begin-->", "<!--LeftReason End-->", string.Empty); }

                            tempStr = tempStr.Replace("{PosName}", workExp.PosName);
                            loopAllStr += tempStr;
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    dataStr = ReplaceLabelContent(dataStr, "<!--Loop Begin-->", "<!--Loop End-->", loopAllStr);
                    htmlString = ReplaceLabelContent(htmlString, "<!--DataCase Begin-->", "<!--DataCase End-->", dataStr);
                    break;
                default:
                    htmlString = string.Empty;
                    break;
            }
            templet = ReplaceLabelContent(templet, "<!--WorkExp Begin-->", "<!--WorkExp End-->", htmlString);
            #endregion

            # region HopeJob(*)
            if (resume.ResumeWrite.HopeWrite == 1)
            {
                htmlString = GetLabelContent(templet, "<!--HopeJob Begin-->", "<!--HopeJob End-->");
                htmlString = htmlString.Replace("{CareerDirection}", FormatHtmlString(resume.HopeJob.CareerDirection, true));
                htmlString = htmlString.Replace("{CheckinDateName}", resume.HopeJob.CheckinDateName);
                htmlString = htmlString.Replace("{EatHouseNeeded}", resume.HopeJob.EatHouseNeeded);
                htmlString = htmlString.Replace("{JobSeeking}", resume.HopeJob.JobSeeking);
                htmlString = htmlString.Replace("{JobTypeName}", resume.HopeJob.JobTypeName);
                htmlString = htmlString.Replace("{OtherNeeded}", resume.HopeJob.OtherNeeded);
                htmlString = htmlString.Replace("{JobLocationName}", resume.HopeJob.JobLocationName);
                try
                {
                    var dicSalaryNews = await _dicService.GetSalaryNew();
                    var salaryName = "";
                    if (resume.HopeJob.Salary == 20)
                    {
                        salaryName = "面议";
                    }
                    else
                    {
                        salaryName = dicSalaryNews.Where(s => s.Id == resume.HopeJob.Salary).Select(s => s.Name)
                            .FirstOrDefault();
                        
                    }
                    htmlString = htmlString.Replace("{SalaryName}", salaryName);
                }
                catch (Exception)
                {

                    htmlString = htmlString.Replace("{SalaryName}", "面议");

                }
                string hl = "";
                string nameStr = "";
                try
                {
                    var jobfs = await _resumeShareVice.GetJobFunction_s(myUserId);
                    // var jobfs = resumeDAL.GetJobFunction_s(myUserId);
                    //if (jobfs != null)
                    if (jobfs.Count > 0)
                    {
                        hl += "jobs " + nameStr;
                        foreach (var item in jobfs)
                        {
                            //nameStr += item.JobFunction_smallName + ",";
                            nameStr += item + ",";
                        }
                        if (!string.IsNullOrEmpty(nameStr))
                            nameStr = nameStr.Substring(0, nameStr.Length - 1);
                    }
                    else
                    {
                        string[] arrid = resume.HopeJob.JobFunction.Split(new char[] { ',' });

                        hl += "arrid " + resume.HopeJob.JobFunction;
                        for (int i = 0; i < arrid.Length; i++)
                        {
                            if (string.IsNullOrEmpty(nameStr))
                            {
                                //nameStr = jobFunctionDAL.GetJobFunctionListBySmallID(Convert.ToInt32(arrid[i])).Pname;
                                nameStr = await _resumeShareVice.GetJobFunctionListBySmallID(Convert.ToInt32(arrid[i]));
                            }
                            else
                            {
                                //nameStr += " , " + jobFunctionDAL.GetJobFunctionListBySmallID(Convert.ToInt32(arrid[i])).Pname;
                                nameStr += " , " + await _resumeShareVice.GetJobFunctionListBySmallID(Convert.ToInt32(arrid[i]));
                            }
                                
                        }
                    }
                }
                catch (Exception ex)
                {
                    nameStr = hl;
                }

                if (string.IsNullOrEmpty(nameStr))
                    htmlString = htmlString.Replace("{JobFunctionName}", resume.HopeJob.JobFunctionName);
                else
                    htmlString = htmlString.Replace("{JobFunctionName}", nameStr);
            }
            else
            {
                htmlString = string.Empty;
            }
            templet = ReplaceLabelContent(templet, "<!--HopeJob Begin-->", "<!--HopeJob End-->", htmlString);
            #endregion

            # region Contact(*)
            #region 新
            htmlString = GetLabelContent(templet, "<!--Contact Begin-->", "<!--Contact End-->");
            //权限控制
            //检查权限
            //ManagePermission permissionBLL = new ManagePermission();
            //if (!permissionBLL.CheckCompanyViewPermission(CompanyViewEnum.ViewCompanyUsername))
            //{

            //    templet = ReplaceLabelContent(templet, "<!--Contact Begin-->", "<!--Contact End-->", "");

            //    // htmlString = ReplaceLabelContent(htmlString, "<!--IsContactEnabled Begin-->", "<!--IsContactEnabled End-->", loopAllStr);
            //    // templet = ReplaceLabelContent(templet, "<!--Contact Begin-->", "<!--Contact End-->", htmlString); 
            //}
            var userId = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            if (! await _checkPermission.CheckCompanyViewPermission(CompanyViewEnum.ViewCompanyUsername, Convert.ToInt32(userId)))
            {
                templet = ReplaceLabelContent(templet, "<!--Contact Begin-->", "<!--Contact End-->", "");
            }
            else
            {
                //2017/08/21 需求更改 权限设定
                //if (HasSalerDownloadResume(myUserId, ManageUser.ClerkID))
                //{
                loopAllStr = GetLabelContent(htmlString, "<!--Enabled Begin-->", "<!--Enabled End-->");
                loopAllStr = loopAllStr.Replace("{Address}", resume.BaseInfo.Contact.Address);
                loopAllStr = loopAllStr.Replace("{ContactPhone}", resume.BaseInfo.Contact.ContactPhone);
                loopAllStr = loopAllStr.Replace("{Email}", resume.BaseInfo.Contact.Email);
                loopAllStr = loopAllStr.Replace("{Homepage}", resume.BaseInfo.Contact.Homepage);
                loopAllStr = loopAllStr.Replace("{IMNum}", resume.BaseInfo.Contact.IMNum);
                loopAllStr = loopAllStr.Replace("{MobileNum}", resume.BaseInfo.Contact.MobileNum);
                loopAllStr = loopAllStr.Replace("{ZipCode}", resume.BaseInfo.Contact.ZipCode);
                templet = templet.Replace("{IsContactEnabled}", "1");
                //}
                htmlString = ReplaceLabelContent(htmlString, "<!--IsContactEnabled Begin-->", "<!--IsContactEnabled End-->", loopAllStr);
                templet = ReplaceLabelContent(templet, "<!--Contact Begin-->", "<!--Contact End-->", htmlString);

            }
            #endregion

            #region 旧
            //htmlString = GetLabelContent(templet, "<!--Contact Begin-->", "<!--Contact End-->");

            //if (HasSalerDownloadResume(myUserId, ManageUser.ClerkID))
            //{
            //    loopAllStr = GetLabelContent(htmlString, "<!--Enabled Begin-->", "<!--Enabled End-->");
            //    loopAllStr = loopAllStr.Replace("{Address}", resume.BaseInfo.Contact.Address);
            //    loopAllStr = loopAllStr.Replace("{ContactPhone}", resume.BaseInfo.Contact.ContactPhone);
            //    loopAllStr = loopAllStr.Replace("{Email}", resume.BaseInfo.Contact.Email);
            //    loopAllStr = loopAllStr.Replace("{Homepage}", resume.BaseInfo.Contact.Homepage);
            //    loopAllStr = loopAllStr.Replace("{IMNum}", resume.BaseInfo.Contact.IMNum);
            //    loopAllStr = loopAllStr.Replace("{MobileNum}", resume.BaseInfo.Contact.MobileNum);
            //    loopAllStr = loopAllStr.Replace("{ZipCode}", resume.BaseInfo.Contact.ZipCode);
            //    templet = templet.Replace("{IsContactEnabled}", "1");
            //}
            //else
            //{
            //    loopAllStr = GetLabelContent(htmlString, "<!--Disabled Begin-->", "<!--Disabled End-->");
            //    templet = templet.Replace("{IsContactEnabled}", "0");
            //}

            //htmlString = ReplaceLabelContent(htmlString, "<!--IsContactEnabled Begin-->", "<!--IsContactEnabled End-->", loopAllStr);
            //templet = ReplaceLabelContent(templet, "<!--Contact Begin-->", "<!--Contact End-->", htmlString); 
            #endregion




            #endregion

            # region Fulfil
            if (resume.ResumeWrite.FulfilWrite == 1)
            {
                htmlString = GetLabelContent(templet, "<!--Fulfil Begin-->", "<!--Fulfil End-->");
                loopStr = GetLabelContent(htmlString, "<!--Loop Begin-->", "<!--Loop End-->");
                loopAllStr = string.Empty;
                foreach (FulfilInfo fulfil in resume.Fulfil)
                {
                    tempStr = loopStr;
                    tempStr = tempStr.Replace("{BeginDateName}", fulfil.BeginDateName);
                    tempStr = tempStr.Replace("{ComName}", fulfil.ComName);
                    tempStr = tempStr.Replace("{Description}", FormatHtmlString(fulfil.Description, true));
                    tempStr = tempStr.Replace("{EndDateName}", fulfil.EndDateName);
                    loopAllStr += tempStr;
                }
                htmlString = ReplaceLabelContent(htmlString, "<!--Loop Begin-->", "<!--Loop End-->", loopAllStr);
            }
            else
            {
                htmlString = string.Empty;
            }
            templet = ReplaceLabelContent(templet, "<!--Fulfil Begin-->", "<!--Fulfil End-->", htmlString);
            #endregion

            # region Training
            if (resume.ResumeWrite.TrainingWrite == 1)
            {
                htmlString = GetLabelContent(templet, "<!--Training Begin-->", "<!--Training End-->");
                loopStr = GetLabelContent(htmlString, "<!--Loop Begin-->", "<!--Loop End-->");
                loopAllStr = string.Empty;
                foreach (TrainingInfo training in resume.Training)
                {
                    tempStr = loopStr;
                    tempStr = tempStr.Replace("{BeginDateName}", training.BeginDateName);
                    tempStr = tempStr.Replace("{Certificate}", training.Certificate);
                    tempStr = tempStr.Replace("{Course}", training.Course);
                    tempStr = tempStr.Replace("{EduMemo}", training.EduMemo);
                    tempStr = tempStr.Replace("{EndDateName}", training.EndDateName);
                    tempStr = tempStr.Replace("{Organization}", training.Organization);
                    loopAllStr += tempStr;
                }
                htmlString = ReplaceLabelContent(htmlString, "<!--Loop Begin-->", "<!--Loop End-->", loopAllStr);
            }
            else
            {
                htmlString = string.Empty;
            }
            templet = ReplaceLabelContent(templet, "<!--Training Begin-->", "<!--Training End-->", htmlString);
            #endregion

            # region Language
            if (resume.ResumeWrite.LanguageWrite == 1)
            {
                htmlString = GetLabelContent(templet, "<!--Language Begin-->", "<!--Language End-->");
                loopStr = GetLabelContent(htmlString, "<!--Loop Begin-->", "<!--Loop End-->");
                loopAllStr = string.Empty;
                foreach (LanguageInfo language in resume.Language)
                {
                    tempStr = loopStr;
                    tempStr = tempStr.Replace("{LanguageLevelName}", language.LanguageLevelName);
                    tempStr = tempStr.Replace("{LanguageName}", language.LanguageName);
                    tempStr = tempStr.Replace("{LanguageDescription}", language.LanguageDescription);
                    loopAllStr += tempStr;
                }
                htmlString = ReplaceLabelContent(htmlString, "<!--Loop Begin-->", "<!--Loop End-->", loopAllStr);
            }
            else
            {
                htmlString = string.Empty;
            }
            templet = ReplaceLabelContent(templet, "<!--Language Begin-->", "<!--Language End-->", htmlString);
            #endregion

            # region Skill
            if (resume.ResumeWrite.SkillWrite == 1)
            {
                htmlString = GetLabelContent(templet, "<!--Skill Begin-->", "<!--Skill End-->");
                htmlString = htmlString.Replace("{ComputerLevelName}", resume.Skill.ComputerLevelName);
                htmlString = htmlString.Replace("{ComputerSkills}", FormatHtmlString(resume.Skill.ComputerSkills, true));
                htmlString = htmlString.Replace("{Interesting}", FormatHtmlString(resume.Skill.Interesting, true));
                htmlString = htmlString.Replace("{OtherSkills}", FormatHtmlString(resume.Skill.OtherSkills, true));
                htmlString = htmlString.Replace("{PositionName}", resume.Skill.PositionName);
            }
            else
            {
                htmlString = string.Empty;
            }
            templet = ReplaceLabelContent(templet, "<!--Skill Begin-->", "<!--Skill End-->", htmlString);
            #endregion

            # region Project
            if (resume.ResumeWrite.ProjectWrite == 1)
            {
                htmlString = GetLabelContent(templet, "<!--Project Begin-->", "<!--Project End-->");
                loopStr = GetLabelContent(htmlString, "<!--Loop Begin-->", "<!--Loop End-->");
                loopAllStr = string.Empty;
                foreach (ProjectInfo project in resume.Project)
                {
                    tempStr = loopStr;
                    tempStr = tempStr.Replace("{BeginDateName}", project.BeginDateName);
                    tempStr = tempStr.Replace("{EndDateName}", project.EndDateName);
                    tempStr = tempStr.Replace("{ProjectDuty}", project.ProjectDuty);
                    tempStr = tempStr.Replace("{ProjectMemo}", project.ProjectMemo);
                    tempStr = tempStr.Replace("{ProjectName}", project.ProjectName);
                    loopAllStr += tempStr;
                }
                htmlString = ReplaceLabelContent(htmlString, "<!--Loop Begin-->", "<!--Loop End-->", loopAllStr);
            }
            else
            {
                htmlString = string.Empty;
            }
            templet = ReplaceLabelContent(templet, "<!--Project Begin-->", "<!--Project End-->", htmlString);
            #endregion
            return Ok(ResultMode<string>.Success(templet));
        }

        /// <summary>
        /// ViewResumeOne.aspx 读取英文简历
        /// </summary>
        /// <param name="myUserId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetResumeTextEn(int myUserId)
        {
            ResumeInfo resume = await _resumeShareVice.GetResume(myUserId);
            if (resume == null) return Ok(ResultMode<string>.Success(""));


            //简历不完整的不显示
            //if (resume.ResumeWrite.AllWrite == 0) return null;

            string htmlString = string.Empty;
            string dataStr = string.Empty;
            string tempStr = string.Empty;
            string loopStr = string.Empty;
            string loopAllStr;

            #region Resume Templet
            string templet = _templetHelper.GetTemplet(Commons.Helper.TempletType.ResumeEnTemplet);
            templet = templet.Replace("{MyUserID}", resume.MyUserID.ToString());
            templet = templet.Replace("{UpdateDate}", resume.UpdateDateName);
            templet = templet.Replace("{HitCount}", resume.HitCount.ToString());
            templet = templet.Replace("{EngResumeFlag}", resume.EngResumeFlag.ToString());
            templet = templet.Replace("{PhotoVisitUrl}", resume.Photo.PhotoVisitUrl);
            #endregion

            # region BaseInfo(*)
            if (resume.ResumeWrite.BaseWrite == 1)
            {
                htmlString = GetLabelContent(templet, "<!--BaseInfo Begin-->", "<!--BaseInfo End-->");
                htmlString = htmlString.Replace("{Age}", resume.BaseInfo.Age.ToString());
                htmlString = htmlString.Replace("{Birthday}", resume.BaseInfo.BirthdayEnName);
                htmlString = htmlString.Replace("{CardNum}", resume.BaseInfo.CardNum);
                htmlString = htmlString.Replace("{CardTypeName}", resume.BaseInfo.CardTypeEnName);
                htmlString = htmlString.Replace("{DegreeName}", resume.BaseInfo.DegreeEnName);
                htmlString = htmlString.Replace("{Height}", resume.BaseInfo.Height.ToString());
                htmlString = htmlString.Replace("{Hometown}", resume.BaseInfo.HometownEn);
                htmlString = htmlString.Replace("{Location}", resume.BaseInfo.LocationEn);
                htmlString = htmlString.Replace("{MaritalName}", resume.BaseInfo.MaritalEnName);
                htmlString = htmlString.Replace("{NationalityName}", resume.BaseInfo.NationalityEnName);
                htmlString = htmlString.Replace("{PerName}", resume.BaseInfo.PerName);
                htmlString = htmlString.Replace("{SelfDescription}", _htmlHelper.FormatHtmlString(resume.BaseInfo.SelfDescription, true));
                htmlString = htmlString.Replace("{SexCN}", resume.BaseInfo.SexEn);
                htmlString = htmlString.Replace("{Weight}", resume.BaseInfo.Weight.ToString());
                htmlString = htmlString.Replace("{WorkExperience}", resume.BaseInfo.WorkExperienceEn);
            }
            else
            {
                htmlString = string.Empty;
            }
            templet = ReplaceLabelContent(templet, "<!--BaseInfo Begin-->", "<!--BaseInfo End-->", htmlString);
            #endregion

            # region Education(*)
            htmlString = GetLabelContent(templet, "<!--Education Begin-->", "<!--Education End-->");
            switch (resume.ResumeWrite.EduWrite)
            {
                case 2:
                    dataStr = GetLabelContent(htmlString, "<!--OldData Begin-->", "<!--OldData End-->");
                    dataStr = dataStr.Replace("{EduText}", _htmlHelper.FormatHtmlString(resume.EduText, true));
                    htmlString = ReplaceLabelContent(htmlString, "<!--DataCase Begin-->", "<!--DataCase End-->", dataStr);
                    break;
                case 1:
                    dataStr = GetLabelContent(htmlString, "<!--NewData Begin-->", "<!--NewData End-->");
                    loopStr = GetLabelContent(dataStr, "<!--Loop Begin-->", "<!--Loop End-->");
                    loopAllStr = string.Empty;
                    foreach (EducationInfo education in resume.Education)
                    {
                        tempStr = loopStr;
                        tempStr = tempStr.Replace("{BeginDateName}", education.BeginDateEnName);
                        tempStr = tempStr.Replace("{Certificate}", education.Certificate);
                        tempStr = tempStr.Replace("{DegreeName}", education.DegreeEnName);
                        tempStr = tempStr.Replace("{EndDateName}", education.EndDateEnName);
                        tempStr = tempStr.Replace("{SchoolName}", education.SchoolName);
                        tempStr = tempStr.Replace("{Speciality}", education.Speciality);
                        tempStr = tempStr.Replace("{SpecialityMemo}", education.SpecialityMemo);
                        loopAllStr += tempStr;
                    }
                    dataStr = ReplaceLabelContent(dataStr, "<!--Loop Begin-->", "<!--Loop End-->", loopAllStr);
                    htmlString = ReplaceLabelContent(htmlString, "<!--DataCase Begin-->", "<!--DataCase End-->", dataStr);
                    break;
                default:
                    htmlString = string.Empty;
                    break;
            }
            templet = ReplaceLabelContent(templet, "<!--Education Begin-->", "<!--Education End-->", htmlString);
            #endregion

            # region WorkExp(*)
            htmlString = GetLabelContent(templet, "<!--WorkExp Begin-->", "<!--WorkExp End-->");
            switch (resume.ResumeWrite.WorkWrite)
            {
                case 2:
                    dataStr = GetLabelContent(htmlString, "<!--OldData Begin-->", "<!--OldData End-->");
                    dataStr = dataStr.Replace("{WorkText}", _htmlHelper.FormatHtmlString(resume.WorkText, true));
                    htmlString = ReplaceLabelContent(htmlString, "<!--DataCase Begin-->", "<!--DataCase End-->", dataStr);
                    break;
                case 1:
                    dataStr = GetLabelContent(htmlString, "<!--NewData Begin-->", "<!--NewData End-->");
                    loopStr = GetLabelContent(dataStr, "<!--Loop Begin-->", "<!--Loop End-->");
                    loopAllStr = string.Empty;
                    foreach (WorkExpInfo workExp in resume.WorkExp)
                    {
                        tempStr = loopStr;
                        tempStr = tempStr.Replace("{BeginDateName}", workExp.BeginDateEnName);
                        tempStr = tempStr.Replace("{ComCallingName}", workExp.ComCallingEnName);
                        tempStr = tempStr.Replace("{ComIntroduction}", _htmlHelper.FormatHtmlString(workExp.ComIntroduction, true));
                        tempStr = tempStr.Replace("{ComName}", workExp.ComName);
                        tempStr = tempStr.Replace("{ComSizeName}", workExp.ComSizeEnName);
                        tempStr = tempStr.Replace("{ComTypeName}", workExp.ComTypeEnName);
                        tempStr = tempStr.Replace("{Description}", _htmlHelper.FormatHtmlString(workExp.Description, true));
                        tempStr = tempStr.Replace("{EndDateName}", workExp.EndDateEnName);
                        tempStr = tempStr.Replace("{LeftReason}", workExp.LeftReason);
                        tempStr = tempStr.Replace("{PosName}", workExp.PosName);
                        loopAllStr += tempStr;
                    }
                    dataStr = ReplaceLabelContent(dataStr, "<!--Loop Begin-->", "<!--Loop End-->", loopAllStr);
                    htmlString = ReplaceLabelContent(htmlString, "<!--DataCase Begin-->", "<!--DataCase End-->", dataStr);
                    break;
                default:
                    htmlString = string.Empty;
                    break;
            }
            templet = ReplaceLabelContent(templet, "<!--WorkExp Begin-->", "<!--WorkExp End-->", htmlString);
            #endregion

            # region HopeJob(*)
            if (resume.ResumeWrite.HopeWrite == 1)
            {
                htmlString = GetLabelContent(templet, "<!--HopeJob Begin-->", "<!--HopeJob End-->");
                htmlString = htmlString.Replace("{CareerDirection}", _htmlHelper.FormatHtmlString(resume.HopeJob.CareerDirection, true));
                htmlString = htmlString.Replace("{CheckinDateName}", resume.HopeJob.CheckinDateEnName);
                htmlString = htmlString.Replace("{EatHouseNeeded}", resume.HopeJob.EatHouseNeeded);
                htmlString = htmlString.Replace("{JobSeeking}", resume.HopeJob.JobSeeking);
                htmlString = htmlString.Replace("{JobTypeName}", resume.HopeJob.JobTypeEnName);
                htmlString = htmlString.Replace("{OtherNeeded}", resume.HopeJob.OtherNeeded);
                htmlString = htmlString.Replace("{SalaryName}", resume.HopeJob.SalaryEnName);
                htmlString = htmlString.Replace("{JobFunctionName}", resume.HopeJob.JobFunctionEnName);
                htmlString = htmlString.Replace("{JobLocationName}", resume.HopeJob.JobLocationEnName);
            }
            else
            {
                htmlString = string.Empty;
            }
            templet = ReplaceLabelContent(templet, "<!--HopeJob Begin-->", "<!--HopeJob End-->", htmlString);
            #endregion

            # region Contact(*)
            htmlString = GetLabelContent(templet, "<!--Contact Begin-->", "<!--Contact End-->");

            loopAllStr = GetLabelContent(htmlString, "<!--Enabled Begin-->", "<!--Enabled End-->");
            loopAllStr = loopAllStr.Replace("{Address}", resume.BaseInfo.Contact.Address);
            loopAllStr = loopAllStr.Replace("{ContactPhone}", resume.BaseInfo.Contact.ContactPhone);
            loopAllStr = loopAllStr.Replace("{Email}", resume.BaseInfo.Contact.Email);
            loopAllStr = loopAllStr.Replace("{Homepage}", resume.BaseInfo.Contact.Homepage);
            loopAllStr = loopAllStr.Replace("{IMNum}", resume.BaseInfo.Contact.IMNum);
            loopAllStr = loopAllStr.Replace("{MobileNum}", resume.BaseInfo.Contact.MobileNum);
            loopAllStr = loopAllStr.Replace("{ZipCode}", resume.BaseInfo.Contact.ZipCode);

            htmlString = ReplaceLabelContent(htmlString, "<!--IsContactEnabled Begin-->", "<!--IsContactEnabled End-->", loopAllStr);
            templet = ReplaceLabelContent(templet, "<!--Contact Begin-->", "<!--Contact End-->", htmlString);
            #endregion

            # region Fulfil
            if (resume.ResumeWrite.FulfilWrite == 1)
            {
                htmlString = GetLabelContent(templet, "<!--Fulfil Begin-->", "<!--Fulfil End-->");
                loopStr = GetLabelContent(htmlString, "<!--Loop Begin-->", "<!--Loop End-->");
                loopAllStr = string.Empty;
                foreach (FulfilInfo fulfil in resume.Fulfil)
                {
                    tempStr = loopStr;
                    tempStr = tempStr.Replace("{BeginDateName}", fulfil.BeginDateEnName);
                    tempStr = tempStr.Replace("{ComName}", fulfil.ComName);
                    tempStr = tempStr.Replace("{Description}", _htmlHelper.FormatHtmlString(fulfil.Description, true));
                    tempStr = tempStr.Replace("{EndDateName}", fulfil.EndDateEnName);
                    loopAllStr += tempStr;
                }
                htmlString = ReplaceLabelContent(htmlString, "<!--Loop Begin-->", "<!--Loop End-->", loopAllStr);
            }
            else
            {
                htmlString = string.Empty;
            }
            templet = ReplaceLabelContent(templet, "<!--Fulfil Begin-->", "<!--Fulfil End-->", htmlString);
            #endregion

            # region Training
            if (resume.ResumeWrite.TrainingWrite == 1)
            {
                htmlString = GetLabelContent(templet, "<!--Training Begin-->", "<!--Training End-->");
                loopStr = GetLabelContent(htmlString, "<!--Loop Begin-->", "<!--Loop End-->");
                loopAllStr = string.Empty;
                foreach (TrainingInfo training in resume.Training)
                {
                    tempStr = loopStr;
                    tempStr = tempStr.Replace("{BeginDateName}", training.BeginDateEnName);
                    tempStr = tempStr.Replace("{Certificate}", training.Certificate);
                    tempStr = tempStr.Replace("{Course}", training.Course);
                    tempStr = tempStr.Replace("{EduMemo}", training.EduMemo);
                    tempStr = tempStr.Replace("{EndDateName}", training.EndDateEnName);
                    tempStr = tempStr.Replace("{Organization}", training.Organization);
                    loopAllStr += tempStr;
                }
                htmlString = ReplaceLabelContent(htmlString, "<!--Loop Begin-->", "<!--Loop End-->", loopAllStr);
            }
            else
            {
                htmlString = string.Empty;
            }
            templet = ReplaceLabelContent(templet, "<!--Training Begin-->", "<!--Training End-->", htmlString);
            #endregion

            # region Language
            if (resume.ResumeWrite.LanguageWrite == 1)
            {
                htmlString = GetLabelContent(templet, "<!--Language Begin-->", "<!--Language End-->");
                loopStr = GetLabelContent(htmlString, "<!--Loop Begin-->", "<!--Loop End-->");
                loopAllStr = string.Empty;
                foreach (LanguageInfo language in resume.Language)
                {
                    tempStr = loopStr;
                    tempStr = tempStr.Replace("{LanguageLevelName}", language.LanguageLevelEnName);
                    tempStr = tempStr.Replace("{LanguageName}", language.LanguageEnName);
                    tempStr = tempStr.Replace("{LanguageDescription}", language.LanguageDescription);
                    loopAllStr += tempStr;
                }
                htmlString = ReplaceLabelContent(htmlString, "<!--Loop Begin-->", "<!--Loop End-->", loopAllStr);
            }
            else
            {
                htmlString = string.Empty;
            }
            templet = ReplaceLabelContent(templet, "<!--Language Begin-->", "<!--Language End-->", htmlString);
            #endregion

            # region Skill
            if (resume.ResumeWrite.SkillWrite == 1)
            {
                htmlString = GetLabelContent(templet, "<!--Skill Begin-->", "<!--Skill End-->");
                htmlString = htmlString.Replace("{ComputerLevelName}", resume.Skill.ComputerLevelEnName);
                htmlString = htmlString.Replace("{ComputerSkills}", _htmlHelper.FormatHtmlString(resume.Skill.ComputerSkills, true));
                htmlString = htmlString.Replace("{Interesting}", _htmlHelper.FormatHtmlString(resume.Skill.Interesting, true));
                htmlString = htmlString.Replace("{OtherSkills}", _htmlHelper.FormatHtmlString(resume.Skill.OtherSkills, true));
                htmlString = htmlString.Replace("{PositionName}", resume.Skill.PositionName);
            }
            else
            {
                htmlString = string.Empty;
            }
            templet = ReplaceLabelContent(templet, "<!--Skill Begin-->", "<!--Skill End-->", htmlString);
            #endregion

            # region Project
            if (resume.ResumeWrite.ProjectWrite == 1)
            {
                htmlString = GetLabelContent(templet, "<!--Project Begin-->", "<!--Project End-->");
                loopStr = GetLabelContent(htmlString, "<!--Loop Begin-->", "<!--Loop End-->");
                loopAllStr = string.Empty;
                foreach (ProjectInfo project in resume.Project)
                {
                    tempStr = loopStr;
                    tempStr = tempStr.Replace("{BeginDateName}", project.BeginDateEnName);
                    tempStr = tempStr.Replace("{EndDateName}", project.EndDateEnName);
                    tempStr = tempStr.Replace("{ProjectDuty}", project.ProjectDuty);
                    tempStr = tempStr.Replace("{ProjectMemo}", project.ProjectMemo);
                    tempStr = tempStr.Replace("{ProjectName}", project.ProjectName);
                    loopAllStr += tempStr;
                }
                htmlString = ReplaceLabelContent(htmlString, "<!--Loop Begin-->", "<!--Loop End-->", loopAllStr);
            }
            else
            {
                htmlString = string.Empty;
            }
            templet = ReplaceLabelContent(templet, "<!--Project Begin-->", "<!--Project End-->", htmlString);
            #endregion

            return Ok(ResultMode<string>.Success(templet));
        }

        /// <summary>
        /// ViewResumeOne.aspx  检测是否已应聘或推荐
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ButCommendLogoClick(int posId,int myUserId)
        {
            var yn = await _resumeShareVice.Checkcommendlogo(posId,myUserId);
            return Ok(ResultMode<bool>.Success(yn));
        }
        /// <summary>
        /// ViewResumeOne.aspx 下载简历,未完成
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> DownResume(int myUserId,string cookieArgs)
        {

            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            //string c = cookieArgs.Replace(" ", "=");
            //string basePath = _env.ContentRootPath;
            string pageUrl = _configuration["DownResumeConfig:PrintPageUrl"] + myUserId;//要打印的页面
            var basePath = _configuration["DownResumeConfig:DownResumePath"];
            string pdf = basePath + "wkhtmltox/bin/wkhtmltopdf.exe";//转换器路径
            string pdfPath = basePath + myUserId + ".pdf";//生成的PDF文件路径

            await Task.Delay(1);
            //ConvertWebPageToPdf(pageUrl, pdfpath,pdf);
            var startInfo = new ProcessStartInfo
            {
                FileName = pdf,
                Arguments = "-q --disable-smart-shrinking --cookie " + token + " " + pageUrl + " " + pdfPath,
                //Arguments =  pageUrl + " " + pdfPath,
                UseShellExecute = false,
                //CreateNoWindow = true
            };

            //startInfo.EnvironmentVariables["COOKIES"] = t + ";" + c;

            using (var process1 = Process.Start(startInfo))
            {
                process1.WaitForExit();
            }
            return Ok(ResultMode<string>.Success(pdfPath));
        }


        /// <summary>
        /// ViewResumeOne.aspx 职位推荐给求职者,未测试,ResumeSearchResult.aspx 发送给个人，未测试
        /// </summary>
        /// <param name="posId"></param>
        /// <param name="myUserId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> BtnIntroduceClick(int posId,string myUserId)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            int returnValue = await _resumeShareVice.CommendResumeLog(posId, myUserId, 3, userId);
            switch (returnValue)
            {
                case 0:
                    await SendMail(myUserId, posId);
                    break;
                case 1:
                    return Ok(ResultMode<string>.NotFound("职位无效"));
            }
            return Ok(ResultMode<string>.Success("推荐成功", "推荐成功"));
        }


        #region ResumeSearchResult.aspx 简历搜索结果 页面的按钮事件

        /// <summary>
        /// ResumeSearchResult.aspx,按钮 发送到企业邮箱
        /// </summary>
        /// <param name="resumeIds">简历Id</param>
        /// <param name="email">邮箱</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ButCommendNewClick(string resumeIds, string email = "@qq.com")
        {
            await Task.Delay(1);
            string[] str = resumeIds.Split(',');
            string subject = "职位简历-俊才网推荐";
            var tempPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "HrSendEmailToCompany.htm");
            int y = 0;
            int n = 0;
            foreach (var value in str)
            {
                string boby = Commons.Helper.FileHelper.GetFileText(tempPath);
                string resumeTemplate = Regex.Replace(await GetResumeTextNotContact(Convert.ToInt32(value)), "<!--ResumeButton Begin-->(.|\n)*<!--ResumeButton End-->", string.Empty);
                boby = string.Format(boby, "亲", "020", "66835555", "俊才网客服", resumeTemplate);
                bool yn = _mail.SendMail(email, subject, boby);
                if (yn) y++;
                else n++;
            }
            return Ok(ResultMode<string>.Success($"成功发送{y}条，{n}条发送失败"));
        }

        /// <summary>
        /// ResumeSearchResult.aspx,推荐给企业,未测试
        /// </summary>
        /// <param name="posId"></param>
        /// <param name="myUserIds"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ButCommendClickNew(string myUserIds,int posId)
        {
            
            var userIds = myUserIds.Split(',').Select(s => int.Parse(s)).ToArray();
            var ids = await _resumeShareVice.CheckResume(userIds);
            if (ids.Any()) 
                return Ok(ResultMode<string>.Failed($"失败，简历中含有隐藏简历，编号为:{string.Join(",", ids)}"));
            var userId = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            int returnValue = await _resumeShareVice.CommendResumeLog(posId, myUserIds, 1, userId);
            string msg = "操作成功，邮件已发送！"+ DateTime.Now;
            string backMyUserId = string.Empty;
            switch (returnValue)
            {
                case 0:
                    foreach (var myUserId in userIds)
                    {
                        try
                        {
                            string tocle = String.Empty;
                            bool yn = await _resumeShareVice.IsShieldCompany(myUserId, posId);
                            if (yn)
                            {
                                backMyUserId += myUserId + "&nbsp&nbsp";
                            }
                            else
                            {
                                await SenMailcommendPos(posId, myUserId);
                                string sss = "";
                                tocle = Goodjob.Encryption.Encryption.EncryptDES(0 + "&" + 0 + "&" + 0 + "&" + 0 + "&" + myUserId + "&" + posId);
                                //开始推送
                                sss = Ticke.GetPage("http://apphr.goodjob.cn/Test/JG.ashx?Tocke=" + tocle, "");
                            }

                            
                            msg = "屏蔽该职位简历编号为：" + backMyUserId + "其它简历操作成功！" + tocle;
                        }
                        catch (Exception e)
                        {
                            LogConfig.TestSetConfig($"ResumeSearchResult.aspx 错误信息{e.Message},{e.InnerException}", "ResumeSearchResult.aspx,推荐给企业");
                            throw;
                        }
                    }
                  
                    break;
                case 1://职位不存在于查询表中
                    //WebHelper.ShowAlert(MessageType.PositionIsNotVaild, AlertType.HistoryBack);
                    msg = "职位不存在于查询表中";
                    return Ok(ResultMode<string>.NotFound("职位无效"));
                    break;
                case 2://同一个职位当天推荐超过100份
                    msg = "当天推荐超过100份";
                    break;
                case 3://同一家企业当天推荐超过500份 
                    msg = "当天推荐超过500份";
                    break;
            }
            return Ok(ResultMode<string>.Success(msg,msg));
        }

        /// <summary>
        /// SimulationResume.aspx 模拟应聘，不发邮箱
        /// </summary>
        /// <param name="resumeId"></param>
        /// <param name="posId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ApplySimulationResume(int resumeId, int posId)
        {
            bool yn = await _resumeShareVice.IsShieldCompany(resumeId, posId);
            if (!yn)
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
                int i = await _resumeShareVice.CommendResumeLog(posId, resumeId.ToString(), 2, userId);
                switch (i)
                {
                    case 0:
                        await _resumeShareVice.CommendResume(posId, resumeId, 2, userId);
                        //await SenMailPos(posId, resumeId);
                        return Ok(ResultMode<string>.Success("", "发送成功"));
                    case 1:
                        return Ok(ResultMode<string>.NotFound("职位无效"));
                }
            }
            return Ok(ResultMode<string>.Failed("职位被屏蔽"));
        }
        #endregion



        #region 私有方法

        //发邮箱
        private async Task SendMail(string myUserIds,int posId)
        {
            var myUserId = myUserIds.Split(",").Select(s => Convert.ToInt32(s)).ToArray();
            //Goodjob.Common.Mail mail = null;
            string email = string.Join(",", await _resumeShareVice.GetPersonEmail(myUserId));
            if (!string.IsNullOrEmpty(email))
            {
                try
                {
                    var mail = _mail.GetMailInfo(Mail.MailType.IntroducePosToPerson);
                    var pos = await _resumeShareVice.GetPositionById(posId);
                    mail.Body = string.Format(mail.Body, pos, posId);
                    _mail.SendMail(mail, email);
                }
                catch (Exception e)
                {
                    LogConfig.TestSetConfig($"错误信息{e.Message},{e.InnerException}", "ViewResumeOne.aspx职位推荐给求职者");
                }
            }
        }
        /// <summary>
        /// 推荐给企业发送邮箱
        /// </summary>
        /// <param name="posId"></param>
        /// <param name="myUserId"></param>
        /// <returns></returns>
        private async Task SenMailcommendPos(int posId,int myUserId)
        {
            var l = await _resumeShareVice.GetDataByPosId(posId);
            if (l.memId == 0)
            {
                return;
            }

            try
            {
                var s = await _resumeShareVice.GetNameAndPassWord(l.memId);
                string memUserName = s.name;
                string memPassWord = s.password;
                var resumeInfo = await _resumeShareVice.GetResume(myUserId);
                string resumeContent = await GetResumeTextAll(resumeInfo, false);
                var appPositionlist = await _resumeShareVice.GetResumeListQueryCollection(myUserId);
                foreach (var appPosition in appPositionlist)
                {
                    string ButtonHtmls = ComButtonHtml(memUserName, memPassWord, myUserId.ToString());
                    if (!string.IsNullOrEmpty(l.email))
                    {
                        string[] NewsEmais = l.email.Split(',');
                        foreach (string value in NewsEmais)
                        {
                            if (!string.IsNullOrEmpty(value))
                            {
                                var mail = _mail.GetMailInfo(Mail.MailType.GoodjobRecommandResume);
                                mail.Subject = string.Format(mail.Subject, l.posName, appPosition.PerName);
                                mail.Body = string.Format(mail.Body, l.contactPerson, appPosition.PerName, appPosition.ResumeID, l.posName, null, resumeContent, DESURL(posId.ToString()), DESURL(appPosition.ResumeID.ToString()), "10", ButtonHtmls);
                                _mail.SendMail(mail, value);
                            }
                        }
                    }
                }

            }
            catch (Exception e)
            {
                LogConfig.TestSetConfig($"错误信息{e.Message},{e.InnerException}", "ViewResumeOne推荐给企业发送邮箱失败");
            }
            
        }

        /// <summary>
        /// 模拟应聘 发邮箱
        /// </summary>
        /// <param name="posId"></param>
        /// <param name="myUserId"></param>
        /// <returns></returns>
        private async Task SenMailPos(int posId, int myUserId)
        {
            //获取岗位的企业ID
            var l = await _resumeShareVice.GetDataByPosId(posId);
            if (l.memId == 0)
            {
                return;
            }
            var s = await _resumeShareVice.GetNameAndPassWord(l.memId);
            string memUserName = s.name;
            string memPassWord = s.password;
            try
            {
                var resumeInfo = await _resumeShareVice.GetResume(myUserId);
                string resumeContent = await GetResumeTextAll(resumeInfo, s.memberClass == 2);
                resumeContent = Regex.Replace(resumeContent, "<!--ResumeButton Begin-->(.|\n)*<!--ResumeButton End-->",
                    string.Empty);
                var appPositionlist = await _resumeShareVice.GetResumeListQueryCollection(myUserId);
                foreach (var appPosition in appPositionlist)
                {
                    string ButtonHtmls = ButtonHtml(memUserName, memPassWord, myUserId.ToString(), l.posName, posId.ToString());
                    if (!string.IsNullOrEmpty(l.email))
                    {
                        string[] NewsEmais = l.email.Split(',');
                        foreach (string value in NewsEmais)
                        {
                            if (!string.IsNullOrEmpty(value))
                            {
                                var mail = _mail.GetMailInfo(Mail.MailType.PersonApplyPosition);
                                mail.Subject = string.Format(mail.Subject, l.posName, appPosition.PerName);
                                mail.Body = string.Format(mail.Body, l.contactPerson, appPosition.PerName, appPosition.ResumeID, l.posName, null, resumeContent, DESURL(posId.ToString()), DESURL(appPosition.ResumeID.ToString()), "0", ButtonHtmls);
                                _mail.SendMail(mail, value);
                            }
                        }
                    }
                }
                //虽然存储过程做了重复推荐的判断，但没必要再多发起一次请求
                //var userId = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
                //await _resumeShareVice.CommendResume(posId, myUserId, 2, userId);

            }
            catch (Exception e)
            {
                LogConfig.TestSetConfig($"错误信息{e.Message},{e.InnerException}", "ViewResumeOne模拟应聘发送邮箱失败");

            }
        }
        //增加招聘按钮
        private string ButtonHtml(string memUserName, string memPassWord, string MyuserID, string PosName, string PosID)
        {
            memUserName = DESURL(memUserName);
            memPassWord = DESURL(memPassWord);
            MyuserID = DESURL(MyuserID);
            PosName = Uri.EscapeDataString(PosName); //替换Server.UrlEncode，在.net6中已被移除 //Server.UrlEncode(PosName);
            PosID = DESURL(PosID);
            string Html = "<a href='http://hr.goodjob.cn/LoginPortal/EmailLogin.aspx?hrUsername=" + memUserName + "&hrPassword=" + memPassWord + "&LoginType=1' target='_blank'><img  src='http://image.goodjob.cn/image/email/btn01.gif' border='0' /></a>  <a href='http://hr.goodjob.cn/LoginPortal/EmailLogin.aspx?hrUsername=" + memUserName + "&hrPassword=" + memPassWord + "&LoginType=2&MyUserID=" + MyuserID + "&PosID=" + PosID + "' target='_blank'><img  src='http://image.goodjob.cn/image/email/btn02.gif' border='0' /></a>  <a href='http://hr.goodjob.cn/LoginPortal/EmailLogin.aspx?hrUsername=" + memUserName + "&hrPassword=" + memPassWord + "&LoginType=3&MyUserID=" + MyuserID + "&PosID=" + PosID + "' target='_blank'><img  src='http://image.goodjob.cn/image/email/btn03.gif' border='0' /></a>  <a href='http://hr.goodjob.cn/LoginPortal/EmailLogin.aspx?hrUsername=" + memUserName + "&hrPassword=" + memPassWord + "&LoginType=4&MyUserID=" + MyuserID + "&PosName=" + PosName + "' target='_blank'><img  src='http://image.goodjob.cn/image/email/btn04.gif' border='0' /></a>";

            return Html;
        }

        //增加推荐招聘按钮
        private string ComButtonHtml(string memUserName, string memPassWord, string MyuserID)
        {
            memUserName = DESURL(memUserName);
            memPassWord = DESURL(memPassWord);
            MyuserID = DESURL(MyuserID);
            string Html = "<a href='http://hr.goodjob.cn/LoginPortal/EmailLogin.aspx?hrUsername=" + memUserName + "&hrPassword=" + memPassWord + "&LoginType=1' target='_blank'><img  src='http://image.goodjob.cn/image/email/btn01.gif' border='0' /></a>  <a href='http://hr.goodjob.cn/LoginPortal/EmailLogin.aspx?hrUsername=" + memUserName + "&hrPassword=" + memPassWord + "&LoginType=5&MyUserID=" + MyuserID + "' target='_blank'><img  src='http://image.goodjob.cn/image/email/btn05.gif' border='0' /></a>";

            return Html;
        }

        //加密并HTML编码
        private string DESURL(string UrlValues)
        {
            return Uri.EscapeDataString(CompanyTool.EncryptDESTool(UrlValues));// Server.UrlEncode(CompanyTool.EncryptDESTool(UrlValues));
        }
        /// <summary>
        /// 替换标签里的字符串
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="begin">开始标签</param>
        /// <param name="end">结束标签</param>
        /// <param name="replacement">替换内容</param>
        /// <returns>替换后字符串</returns>
        private string ReplaceLabelContent(string input, string begin, string end, string replacement)
        {
            return Regex.Replace(input, "(" + begin + "(.|\n)*" + end + ")", replacement, RegexOptions.IgnoreCase);
        }
        /// <summary>
        /// 获取签标里的字符串
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="begin">开始标签</param>
        /// <param name="end">结束标签</param>
        /// <returns>签标里的字符串</returns>
        private string GetLabelContent(string input, string begin, string end)
        {
            Match match = Regex.Match(input, begin + "((.|\n)*)" + end, RegexOptions.IgnoreCase);
            if (!match.Success) return "";
            return match.Groups[0].ToString();
        }
        /// <summary>
        /// 格式化输出到页面的字符串，包括转换回车符
        /// </summary>
        /// <param name="htmlstr">要格式化的字符串</param>
        /// <param name="replace">是否替换换行符</param>
        /// <returns>格式化后的字符串</returns>
        private static string FormatHtmlString(string htmlstr, bool replace)
        {
            if (string.IsNullOrEmpty(htmlstr)) return "";
            var encoder = HtmlEncoder.Default;
            htmlstr = encoder.Encode(htmlstr); //HttpContext.Current.Server.HtmlEncode(htmlstr);
            htmlstr = htmlstr.Replace(" ", "&nbsp;");
            if (replace)
            {
                htmlstr = htmlstr.Replace("\r\n", "<br/>");
                htmlstr = htmlstr.Replace("\n\n", "<br/>");
                htmlstr = htmlstr.Replace("\n", "<br/>");
            }
            return htmlstr;
        }
        private async Task<string> GetResumeTextAll(ResumeInfo? resume, bool isEnabledContent)
        {
            if (resume == null) return string.Empty;
            string htmlString = string.Empty;
            string dataStr = string.Empty;
            string tempStr = string.Empty;
            string loopStr = string.Empty;
            string loopAllStr;

            #region Resume Templet
            string templet = _templetHelper.GetTemplet(Commons.Helper.TempletType.ResumeTemplet);
            templet = templet.Replace("{MyUserID}", resume.MyUserID.ToString());
            templet = templet.Replace("{UpdateDate}", resume.UpdateDateName);
            templet = templet.Replace("{HitCount}", resume.HitCount.ToString());
            templet = templet.Replace("{EngResumeFlag}", resume.EngResumeFlag.ToString());
            templet = templet.Replace("{PhotoVisitUrl}", resume.Photo.PhotoVisitUrl);
            #endregion

            # region BaseInfo(*)
            if (resume.ResumeWrite.BaseWrite == 1)
            {
                htmlString = GetLabelContent(templet, "<!--BaseInfo Begin-->", "<!--BaseInfo End-->");
                htmlString = htmlString.Replace("{Age}", resume.BaseInfo.Age.ToString());
                htmlString = htmlString.Replace("{Birthday}", resume.BaseInfo.BirthdayName);
                htmlString = htmlString.Replace("{CardNum}", resume.BaseInfo.CardNum);
                htmlString = htmlString.Replace("{CardTypeName}", resume.BaseInfo.CardTypeName);
                htmlString = htmlString.Replace("{DegreeName}", resume.BaseInfo.DegreeName);
                htmlString = htmlString.Replace("{Height}", resume.BaseInfo.Height.ToString());
                htmlString = htmlString.Replace("{Hometown}", resume.BaseInfo.Hometown);
                htmlString = htmlString.Replace("{Location}", resume.BaseInfo.Location);
                htmlString = htmlString.Replace("{MaritalName}", resume.BaseInfo.MaritalName);
                htmlString = htmlString.Replace("{NationalityName}", resume.BaseInfo.NationalityName);
                htmlString = htmlString.Replace("{PerName}", resume.BaseInfo.PerName);
                htmlString = htmlString.Replace("{SelfDescription}", FormatHtmlString(resume.BaseInfo.SelfDescription, true));
                htmlString = htmlString.Replace("{SexCN}", resume.BaseInfo.SexCN);
                htmlString = htmlString.Replace("{Weight}", resume.BaseInfo.Weight.ToString());
                htmlString = htmlString.Replace("{WorkExperience}", resume.BaseInfo.WorkExperience);
            }
            else
            {
                htmlString = string.Empty;
            }
            templet = ReplaceLabelContent(templet, "<!--BaseInfo Begin-->", "<!--BaseInfo End-->", htmlString);
            #endregion

            # region Education(*)
            htmlString = GetLabelContent(templet, "<!--Education Begin-->", "<!--Education End-->");
            switch (resume.ResumeWrite.EduWrite)
            {
                case 2:
                    dataStr = GetLabelContent(htmlString, "<!--OldData Begin-->", "<!--OldData End-->");
                    dataStr = dataStr.Replace("{EduText}", FormatHtmlString(resume.EduText, true));
                    htmlString = ReplaceLabelContent(htmlString, "<!--DataCase Begin-->", "<!--DataCase End-->", dataStr);
                    break;
                case 1:
                    dataStr = GetLabelContent(htmlString, "<!--NewData Begin-->", "<!--NewData End-->");
                    loopStr = GetLabelContent(dataStr, "<!--Loop Begin-->", "<!--Loop End-->");
                    loopAllStr = string.Empty;
                    foreach (EducationInfo education in resume.Education)
                    {
                        tempStr = loopStr;
                        tempStr = tempStr.Replace("{BeginDateName}", education.BeginDateName);
                        tempStr = tempStr.Replace("{Certificate}", education.Certificate);
                        tempStr = tempStr.Replace("{DegreeName}", education.DegreeName);
                        tempStr = tempStr.Replace("{EndDateName}", education.EndDateName);
                        tempStr = tempStr.Replace("{SchoolName}", education.SchoolName);
                        tempStr = tempStr.Replace("{Speciality}", education.Speciality);
                        tempStr = tempStr.Replace("{SpecialityMemo}", education.SpecialityMemo);
                        loopAllStr += tempStr;
                    }
                    dataStr = ReplaceLabelContent(dataStr, "<!--Loop Begin-->", "<!--Loop End-->", loopAllStr);
                    htmlString = ReplaceLabelContent(htmlString, "<!--DataCase Begin-->", "<!--DataCase End-->", dataStr);
                    break;
                default:
                    htmlString = string.Empty;
                    break;
            }
            templet = ReplaceLabelContent(templet, "<!--Education Begin-->", "<!--Education End-->", htmlString);
            #endregion

            # region WorkExp(*)
            htmlString = GetLabelContent(templet, "<!--WorkExp Begin-->", "<!--WorkExp End-->");
            switch (resume.ResumeWrite.WorkWrite)
            {
                case 2:
                    dataStr = GetLabelContent(htmlString, "<!--OldData Begin-->", "<!--OldData End-->");
                    dataStr = dataStr.Replace("{WorkText}", FormatHtmlString(resume.WorkText, true));
                    htmlString = ReplaceLabelContent(htmlString, "<!--DataCase Begin-->", "<!--DataCase End-->", dataStr);
                    break;
                case 1:
                    dataStr = GetLabelContent(htmlString, "<!--NewData Begin-->", "<!--NewData End-->");
                    loopStr = GetLabelContent(dataStr, "<!--Loop Begin-->", "<!--Loop End-->");
                    loopAllStr = string.Empty;
                    foreach (WorkExpInfo workExp in resume.WorkExp)
                    {
                        tempStr = loopStr;
                        tempStr = tempStr.Replace("{BeginDateName}", workExp.BeginDateName);
                        tempStr = tempStr.Replace("{ComCallingName}", workExp.ComCallingName);
                        tempStr = tempStr.Replace("{ComIntroduction}", FormatHtmlString(workExp.ComIntroduction, true));
                        tempStr = tempStr.Replace("{ComName}", workExp.ComName);
                        tempStr = tempStr.Replace("{ComSizeName}", workExp.ComSizeName);
                        tempStr = tempStr.Replace("{ComTypeName}", workExp.ComTypeName);
                        tempStr = tempStr.Replace("{Description}", FormatHtmlString(workExp.Description, true));
                        tempStr = tempStr.Replace("{EndDateName}", workExp.EndDateName);

                        if (workExp.LeftReason != string.Empty)
                        { tempStr = tempStr.Replace("{LeftReason}", workExp.LeftReason); }
                        else
                        { tempStr = ReplaceLabelContent(tempStr, "<!--LeftReason Begin-->", "<!--LeftReason End-->", string.Empty); }

                        tempStr = tempStr.Replace("{PosName}", workExp.PosName);
                        loopAllStr += tempStr;
                    }
                    dataStr = ReplaceLabelContent(dataStr, "<!--Loop Begin-->", "<!--Loop End-->", loopAllStr);
                    htmlString = ReplaceLabelContent(htmlString, "<!--DataCase Begin-->", "<!--DataCase End-->", dataStr);
                    break;
                default:
                    htmlString = string.Empty;
                    break;
            }
            templet = ReplaceLabelContent(templet, "<!--WorkExp Begin-->", "<!--WorkExp End-->", htmlString);
            #endregion

            # region HopeJob(*)
            if (resume.ResumeWrite.HopeWrite == 1)
            {
                htmlString = GetLabelContent(templet, "<!--HopeJob Begin-->", "<!--HopeJob End-->");
                htmlString = htmlString.Replace("{CareerDirection}", FormatHtmlString(resume.HopeJob.CareerDirection, true));
                htmlString = htmlString.Replace("{CheckinDateName}", resume.HopeJob.CheckinDateName);
                htmlString = htmlString.Replace("{EatHouseNeeded}", resume.HopeJob.EatHouseNeeded);
                htmlString = htmlString.Replace("{JobSeeking}", resume.HopeJob.JobSeeking);
                htmlString = htmlString.Replace("{JobTypeName}", resume.HopeJob.JobTypeName);
                htmlString = htmlString.Replace("{OtherNeeded}", resume.HopeJob.OtherNeeded);

                var dicSalaryNews = await _dicService.GetSalaryNew();

                //List<DicSalaryNew> dicSalaryNews = PosQueryDAL.GetSalaryNew();
                var salaryName = "";
                if (resume.HopeJob.Salary == 20)
                {
                    salaryName = "面议";
                }
                else
                {
                    salaryName = dicSalaryNews.Where(s => s.Id == resume.HopeJob.Salary).Select(s => s.Name)
                        .FirstOrDefault();
                    
                    //foreach (DicSalaryNew salaryNew in dicSalaryNews)
                    //{
                    //    if (resume.HopeJob.Salary == salaryNew.Id)
                    //    {
                    //        salaryName = salaryNew.Name;
                    //    }
                    //}
                }
                htmlString = htmlString.Replace("{SalaryName}", salaryName);
                //htmlString = htmlString.Replace("{SalaryName}", resume.HopeJob.SalaryName);

                htmlString = htmlString.Replace("{JobFunctionName}", resume.HopeJob.JobFunctionName);
                htmlString = htmlString.Replace("{JobLocationName}", resume.HopeJob.JobLocationName);
            }
            else
            {
                htmlString = string.Empty;
            }
            templet = ReplaceLabelContent(templet, "<!--HopeJob Begin-->", "<!--HopeJob End-->", htmlString);
            #endregion

            # region Contact(*)
            if (isEnabledContent)
            {
                htmlString = GetLabelContent(templet, "<!--Contact Begin-->", "<!--Contact End-->");
                loopAllStr = GetLabelContent(htmlString, "<!--Enabled Begin-->", "<!--Enabled End-->");
                loopAllStr = loopAllStr.Replace("{Address}", resume.BaseInfo.Contact.Address);
                loopAllStr = loopAllStr.Replace("{ContactPhone}", resume.BaseInfo.Contact.ContactPhone);
                loopAllStr = loopAllStr.Replace("{Email}", resume.BaseInfo.Contact.Email);
                loopAllStr = loopAllStr.Replace("{Homepage}", resume.BaseInfo.Contact.Homepage);
                loopAllStr = loopAllStr.Replace("{IMNum}", resume.BaseInfo.Contact.IMNum);
                loopAllStr = loopAllStr.Replace("{MobileNum}", resume.BaseInfo.Contact.MobileNum);
                loopAllStr = loopAllStr.Replace("{ZipCode}", resume.BaseInfo.Contact.ZipCode);
                htmlString = ReplaceLabelContent(htmlString, "<!--IsContactEnabled Begin-->", "<!--IsContactEnabled End-->", loopAllStr);
            }
            else
            {
                htmlString = string.Empty;
            }

            templet = ReplaceLabelContent(templet, "<!--Contact Begin-->", "<!--Contact End-->", htmlString);
            #endregion

            # region Fulfil
            if (resume.ResumeWrite.FulfilWrite == 1)
            {
                htmlString = GetLabelContent(templet, "<!--Fulfil Begin-->", "<!--Fulfil End-->");
                loopStr = GetLabelContent(htmlString, "<!--Loop Begin-->", "<!--Loop End-->");
                loopAllStr = string.Empty;
                foreach (FulfilInfo fulfil in resume.Fulfil)
                {
                    tempStr = loopStr;
                    tempStr = tempStr.Replace("{BeginDateName}", fulfil.BeginDateName);
                    tempStr = tempStr.Replace("{ComName}", fulfil.ComName);
                    tempStr = tempStr.Replace("{Description}", FormatHtmlString(fulfil.Description, true));
                    tempStr = tempStr.Replace("{EndDateName}", fulfil.EndDateName);
                    loopAllStr += tempStr;
                }
                htmlString = ReplaceLabelContent(htmlString, "<!--Loop Begin-->", "<!--Loop End-->", loopAllStr);
            }
            else
            {
                htmlString = string.Empty;
            }
            templet = ReplaceLabelContent(templet, "<!--Fulfil Begin-->", "<!--Fulfil End-->", htmlString);
            #endregion

            # region Training
            if (resume.ResumeWrite.TrainingWrite == 1)
            {
                htmlString = GetLabelContent(templet, "<!--Training Begin-->", "<!--Training End-->");
                loopStr = GetLabelContent(htmlString, "<!--Loop Begin-->", "<!--Loop End-->");
                loopAllStr = string.Empty;
                foreach (TrainingInfo training in resume.Training)
                {
                    tempStr = loopStr;
                    tempStr = tempStr.Replace("{BeginDateName}", training.BeginDateName);
                    tempStr = tempStr.Replace("{Certificate}", training.Certificate);
                    tempStr = tempStr.Replace("{Course}", training.Course);
                    tempStr = tempStr.Replace("{EduMemo}", training.EduMemo);
                    tempStr = tempStr.Replace("{EndDateName}", training.EndDateName);
                    tempStr = tempStr.Replace("{Organization}", training.Organization);
                    loopAllStr += tempStr;
                }
                htmlString = ReplaceLabelContent(htmlString, "<!--Loop Begin-->", "<!--Loop End-->", loopAllStr);
            }
            else
            {
                htmlString = string.Empty;
            }
            templet = ReplaceLabelContent(templet, "<!--Training Begin-->", "<!--Training End-->", htmlString);
            #endregion

            # region Language
            if (resume.ResumeWrite.LanguageWrite == 1)
            {
                htmlString = GetLabelContent(templet, "<!--Language Begin-->", "<!--Language End-->");
                loopStr = GetLabelContent(htmlString, "<!--Loop Begin-->", "<!--Loop End-->");
                loopAllStr = string.Empty;
                foreach (LanguageInfo language in resume.Language)
                {
                    tempStr = loopStr;
                    tempStr = tempStr.Replace("{LanguageLevelName}", language.LanguageLevelName);
                    tempStr = tempStr.Replace("{LanguageName}", language.LanguageName);
                    tempStr = tempStr.Replace("{LanguageDescription}", language.LanguageDescription);
                    loopAllStr += tempStr;
                }
                htmlString = ReplaceLabelContent(htmlString, "<!--Loop Begin-->", "<!--Loop End-->", loopAllStr);
            }
            else
            {
                htmlString = string.Empty;
            }
            templet = ReplaceLabelContent(templet, "<!--Language Begin-->", "<!--Language End-->", htmlString);
            #endregion

            # region Skill
            if (resume.ResumeWrite.SkillWrite == 1)
            {
                htmlString = GetLabelContent(templet, "<!--Skill Begin-->", "<!--Skill End-->");
                htmlString = htmlString.Replace("{ComputerLevelName}", resume.Skill.ComputerLevelName);
                htmlString = htmlString.Replace("{ComputerSkills}", FormatHtmlString(resume.Skill.ComputerSkills, true));
                htmlString = htmlString.Replace("{Interesting}", FormatHtmlString(resume.Skill.Interesting, true));
                htmlString = htmlString.Replace("{OtherSkills}", FormatHtmlString(resume.Skill.OtherSkills, true));
                htmlString = htmlString.Replace("{PositionName}", resume.Skill.PositionName);
            }
            else
            {
                htmlString = string.Empty;
            }
            templet = ReplaceLabelContent(templet, "<!--Skill Begin-->", "<!--Skill End-->", htmlString);
            #endregion

            # region Project
            if (resume.ResumeWrite.ProjectWrite == 1)
            {
                htmlString = GetLabelContent(templet, "<!--Project Begin-->", "<!--Project End-->");
                loopStr = GetLabelContent(htmlString, "<!--Loop Begin-->", "<!--Loop End-->");
                loopAllStr = string.Empty;
                foreach (ProjectInfo project in resume.Project)
                {
                    tempStr = loopStr;
                    tempStr = tempStr.Replace("{BeginDateName}", project.BeginDateName);
                    tempStr = tempStr.Replace("{EndDateName}", project.EndDateName);
                    tempStr = tempStr.Replace("{ProjectDuty}", project.ProjectDuty);
                    tempStr = tempStr.Replace("{ProjectMemo}", project.ProjectMemo);
                    tempStr = tempStr.Replace("{ProjectName}", project.ProjectName);
                    loopAllStr += tempStr;
                }
                htmlString = ReplaceLabelContent(htmlString, "<!--Loop Begin-->", "<!--Loop End-->", loopAllStr);
            }
            else
            {
                htmlString = string.Empty;
            }
            templet = ReplaceLabelContent(templet, "<!--Project Begin-->", "<!--Project End-->", htmlString);
            #endregion

            return templet;
        }


        protected string GetReplaceKey(string webvalues, string keyWord)
        {
            string newskeyWord = "";

            newskeyWord = keyWord.Trim().ToUpper();

            newskeyWord = keyWord.Trim().ToUpper().Replace(" ", "|").Replace(";", "|").Replace(",", "|").Replace("，", "|");

            string[] keyWordList = newskeyWord.Trim().Split('|');
            foreach (string values in keyWordList)
            {
                if (!string.IsNullOrEmpty(values))
                {
                    //"<span style=\"background-color:yellow;color:red\"><a name=\"$3\">$3</a></span>"
                    //webvalues = webvalues.Replace(values, "<b style='color:Red;'>" + values + "</b>");
                    webvalues = webvalues.ToUpper().Replace(values, "<b style='background-color:yellow;color:red;'>" + values + "</b>");
                }
            }

            return webvalues;

        }
        /// <summary>
        /// 判断销售员是否已下载指定简历
        /// </summary>
        /// <param name="myUserId">简历编号</param>
        /// <param name="salerId">销售员编号</param>
        /// <returns></returns>
        private async Task<bool> HasSalerDownloadResume(int myUserId, int salerId)
        {
            return await _resumeShareVice.SalerDownloadResumeCount(myUserId, salerId) > 0;
            //return (new SalesController()).SalerDownloadResumeCount(myUserID, salerID) > 0;
        }




        /// <summary>
        /// 读取没有联系方式的简历
        /// </summary>
        /// <param name="myUserId"></param>
        /// <returns></returns>
        private async Task<string> GetResumeTextNotContact(int myUserId)
        {
            var resume = await _resumeShareVice.GetResume(myUserId);
            if (resume == null) return string.Empty;
            string htmlString = string.Empty;
            string dataStr = string.Empty;
            string tempStr = string.Empty;
            string loopStr = string.Empty;
            string loopAllStr;
            #region Resume Templet
            string templet = _templetHelper.GetTemplet(Commons.Helper.TempletType.ResumeTemplet);
            templet = templet.Replace("{MyUserID}", resume.MyUserID.ToString());
            templet = templet.Replace("{UpdateDate}", resume.UpdateDateName);
            templet = templet.Replace("{HitCount}", resume.HitCount.ToString());
            templet = templet.Replace("{EngResumeFlag}", resume.EngResumeFlag.ToString());
            templet = templet.Replace("{PhotoVisitUrl}", resume.Photo.PhotoVisitUrl);
            #endregion

            #region BaseInfo(*)
            if (resume.ResumeWrite.BaseWrite == 1)
            {
                htmlString = GetLabelContent(templet, "<!--BaseInfo Begin-->", "<!--BaseInfo End-->");
                htmlString = htmlString.Replace("{Age}", resume.BaseInfo.Age.ToString());
                htmlString = htmlString.Replace("{Birthday}", resume.BaseInfo.BirthdayName);
                htmlString = htmlString.Replace("{CardNum}", resume.BaseInfo.CardNum);
                htmlString = htmlString.Replace("{CardTypeName}", resume.BaseInfo.CardTypeName);
                htmlString = htmlString.Replace("{DegreeName}", resume.BaseInfo.DegreeName);
                htmlString = htmlString.Replace("{Height}", resume.BaseInfo.Height.ToString());
                htmlString = htmlString.Replace("{Hometown}", resume.BaseInfo.Hometown);
                htmlString = htmlString.Replace("{Location}", resume.BaseInfo.Location);
                htmlString = htmlString.Replace("{MaritalName}", resume.BaseInfo.MaritalName);
                htmlString = htmlString.Replace("{NationalityName}", resume.BaseInfo.NationalityName);
                htmlString = htmlString.Replace("{PerName}", resume.BaseInfo.PerName);
                htmlString = htmlString.Replace("{SelfDescription}", FormatHtmlString(resume.BaseInfo.SelfDescription, true));
                htmlString = htmlString.Replace("{SexCN}", resume.BaseInfo.SexCN);
                htmlString = htmlString.Replace("{Weight}", resume.BaseInfo.Weight.ToString());
                htmlString = htmlString.Replace("{WorkExperience}", resume.BaseInfo.WorkExperience);
            }
            else
            {
                htmlString = string.Empty;
            }
            templet = ReplaceLabelContent(templet, "<!--BaseInfo Begin-->", "<!--BaseInfo End-->", htmlString);
            #endregion

            #region Education(*)
            htmlString = GetLabelContent(templet, "<!--Education Begin-->", "<!--Education End-->");
            switch (resume.ResumeWrite.EduWrite)
            {
                case 2:
                    dataStr = GetLabelContent(htmlString, "<!--OldData Begin-->", "<!--OldData End-->");
                    dataStr = dataStr.Replace("{EduText}", FormatHtmlString(resume.EduText, true));
                    htmlString = ReplaceLabelContent(htmlString, "<!--DataCase Begin-->", "<!--DataCase End-->", dataStr);
                    break;
                case 1:
                    dataStr = GetLabelContent(htmlString, "<!--NewData Begin-->", "<!--NewData End-->");
                    loopStr = GetLabelContent(dataStr, "<!--Loop Begin-->", "<!--Loop End-->");
                    loopAllStr = string.Empty;
                    foreach (EducationInfo education in resume.Education)
                    {
                        tempStr = loopStr;
                        tempStr = tempStr.Replace("{BeginDateName}", education.BeginDateName);
                        tempStr = tempStr.Replace("{Certificate}", education.Certificate);
                        tempStr = tempStr.Replace("{DegreeName}", education.DegreeName);
                        tempStr = tempStr.Replace("{EndDateName}", education.EndDateName);
                        tempStr = tempStr.Replace("{SchoolName}", education.SchoolName);
                        tempStr = tempStr.Replace("{Speciality}", education.Speciality);
                        tempStr = tempStr.Replace("{SpecialityMemo}", education.SpecialityMemo);
                        loopAllStr += tempStr;
                    }
                    dataStr = ReplaceLabelContent(dataStr, "<!--Loop Begin-->", "<!--Loop End-->", loopAllStr);
                    htmlString = ReplaceLabelContent(htmlString, "<!--DataCase Begin-->", "<!--DataCase End-->", dataStr);
                    break;
                default:
                    htmlString = string.Empty;
                    break;
            }
            templet = ReplaceLabelContent(templet, "<!--Education Begin-->", "<!--Education End-->", htmlString);
            #endregion

            # region WorkExp(*)
            htmlString = GetLabelContent(templet, "<!--WorkExp Begin-->", "<!--WorkExp End-->");
            switch (resume.ResumeWrite.WorkWrite)
            {
                case 2:
                    dataStr = GetLabelContent(htmlString, "<!--OldData Begin-->", "<!--OldData End-->");
                    dataStr = dataStr.Replace("{WorkText}", FormatHtmlString(resume.WorkText, true));
                    htmlString = ReplaceLabelContent(htmlString, "<!--DataCase Begin-->", "<!--DataCase End-->", dataStr);
                    break;
                case 1:
                    dataStr = GetLabelContent(htmlString, "<!--NewData Begin-->", "<!--NewData End-->");
                    loopStr = GetLabelContent(dataStr, "<!--Loop Begin-->", "<!--Loop End-->");
                    loopAllStr = string.Empty;
                    foreach (WorkExpInfo workExp in resume.WorkExp)
                    {
                        tempStr = loopStr;
                        tempStr = tempStr.Replace("{BeginDateName}", workExp.BeginDateName);
                        tempStr = tempStr.Replace("{ComCallingName}", workExp.ComCallingName);
                        tempStr = tempStr.Replace("{ComIntroduction}", FormatHtmlString(workExp.ComIntroduction, true));
                        tempStr = tempStr.Replace("{ComName}", workExp.ComName);
                        tempStr = tempStr.Replace("{ComSizeName}", workExp.ComSizeName);
                        tempStr = tempStr.Replace("{ComTypeName}", workExp.ComTypeName);
                        tempStr = tempStr.Replace("{Description}", FormatHtmlString(workExp.Description, true));
                        tempStr = tempStr.Replace("{EndDateName}", workExp.EndDateName);

                        if (workExp.LeftReason != string.Empty)
                        { tempStr = tempStr.Replace("{LeftReason}", workExp.LeftReason); }
                        else
                        { tempStr = ReplaceLabelContent(tempStr, "<!--LeftReason Begin-->", "<!--LeftReason End-->", string.Empty); }

                        tempStr = tempStr.Replace("{PosName}", workExp.PosName);
                        loopAllStr += tempStr;
                    }
                    dataStr = ReplaceLabelContent(dataStr, "<!--Loop Begin-->", "<!--Loop End-->", loopAllStr);
                    htmlString = ReplaceLabelContent(htmlString, "<!--DataCase Begin-->", "<!--DataCase End-->", dataStr);
                    break;
                default:
                    htmlString = string.Empty;
                    break;
            }
            templet = ReplaceLabelContent(templet, "<!--WorkExp Begin-->", "<!--WorkExp End-->", htmlString);
            #endregion

            # region HopeJob(*)
            if (resume.ResumeWrite.HopeWrite == 1)
            {
                htmlString = GetLabelContent(templet, "<!--HopeJob Begin-->", "<!--HopeJob End-->");
                htmlString = htmlString.Replace("{CareerDirection}", FormatHtmlString(resume.HopeJob.CareerDirection, true));
                htmlString = htmlString.Replace("{CheckinDateName}", resume.HopeJob.CheckinDateName);
                htmlString = htmlString.Replace("{EatHouseNeeded}", resume.HopeJob.EatHouseNeeded);
                htmlString = htmlString.Replace("{JobSeeking}", resume.HopeJob.JobSeeking);
                htmlString = htmlString.Replace("{JobTypeName}", resume.HopeJob.JobTypeName);
                htmlString = htmlString.Replace("{OtherNeeded}", resume.HopeJob.OtherNeeded);
                htmlString = htmlString.Replace("{SalaryName}", resume.HopeJob.SalaryName);
                htmlString = htmlString.Replace("{JobFunctionName}", resume.HopeJob.JobFunctionName);
                htmlString = htmlString.Replace("{JobLocationName}", resume.HopeJob.JobLocationName);
            }
            else
            {
                htmlString = string.Empty;
            }
            templet = ReplaceLabelContent(templet, "<!--HopeJob Begin-->", "<!--HopeJob End-->", htmlString);
            #endregion

            # region Contact(*)
            //htmlString = GetLabelContent(templet, "<!--Contact Begin-->", "<!--Contact End-->");

            //loopAllStr = GetLabelContent(htmlString, "<!--Enabled Begin-->", "<!--Enabled End-->");
            //loopAllStr = loopAllStr.Replace("{Address}", resume.BaseInfo.Contact.Address);
            //loopAllStr = loopAllStr.Replace("{ContactPhone}", resume.BaseInfo.Contact.ContactPhone);
            //loopAllStr = loopAllStr.Replace("{Email}", resume.BaseInfo.Contact.Email);
            //loopAllStr = loopAllStr.Replace("{Homepage}", resume.BaseInfo.Contact.Homepage);
            //loopAllStr = loopAllStr.Replace("{IMNum}", resume.BaseInfo.Contact.IMNum);
            //loopAllStr = loopAllStr.Replace("{MobileNum}", resume.BaseInfo.Contact.MobileNum);
            //loopAllStr = loopAllStr.Replace("{ZipCode}", resume.BaseInfo.Contact.ZipCode);

            //htmlString = ReplaceLabelContent(htmlString, "<!--IsContactEnabled Begin-->", "<!--IsContactEnabled End-->", loopAllStr);
            templet = ReplaceLabelContent(templet, "<!--Contact Begin-->", "<!--Contact End-->", string.Empty);
            #endregion

            # region Fulfil
            if (resume.ResumeWrite.FulfilWrite == 1)
            {
                htmlString = GetLabelContent(templet, "<!--Fulfil Begin-->", "<!--Fulfil End-->");
                loopStr = GetLabelContent(htmlString, "<!--Loop Begin-->", "<!--Loop End-->");
                loopAllStr = string.Empty;
                foreach (FulfilInfo fulfil in resume.Fulfil)
                {
                    tempStr = loopStr;
                    tempStr = tempStr.Replace("{BeginDateName}", fulfil.BeginDateName);
                    tempStr = tempStr.Replace("{ComName}", fulfil.ComName);
                    tempStr = tempStr.Replace("{Description}", FormatHtmlString(fulfil.Description, true));
                    tempStr = tempStr.Replace("{EndDateName}", fulfil.EndDateName);
                    loopAllStr += tempStr;
                }
                htmlString = ReplaceLabelContent(htmlString, "<!--Loop Begin-->", "<!--Loop End-->", loopAllStr);
            }
            else
            {
                htmlString = string.Empty;
            }
            templet = ReplaceLabelContent(templet, "<!--Fulfil Begin-->", "<!--Fulfil End-->", htmlString);
            #endregion

            # region Training
            if (resume.ResumeWrite.TrainingWrite == 1)
            {
                htmlString = GetLabelContent(templet, "<!--Training Begin-->", "<!--Training End-->");
                loopStr = GetLabelContent(htmlString, "<!--Loop Begin-->", "<!--Loop End-->");
                loopAllStr = string.Empty;
                foreach (TrainingInfo training in resume.Training)
                {
                    tempStr = loopStr;
                    tempStr = tempStr.Replace("{BeginDateName}", training.BeginDateName);
                    tempStr = tempStr.Replace("{Certificate}", training.Certificate);
                    tempStr = tempStr.Replace("{Course}", training.Course);
                    tempStr = tempStr.Replace("{EduMemo}", training.EduMemo);
                    tempStr = tempStr.Replace("{EndDateName}", training.EndDateName);
                    tempStr = tempStr.Replace("{Organization}", training.Organization);
                    loopAllStr += tempStr;
                }
                htmlString = ReplaceLabelContent(htmlString, "<!--Loop Begin-->", "<!--Loop End-->", loopAllStr);
            }
            else
            {
                htmlString = string.Empty;
            }
            templet = ReplaceLabelContent(templet, "<!--Training Begin-->", "<!--Training End-->", htmlString);
            #endregion

            # region Language
            if (resume.ResumeWrite.LanguageWrite == 1)
            {
                htmlString = GetLabelContent(templet, "<!--Language Begin-->", "<!--Language End-->");
                loopStr = GetLabelContent(htmlString, "<!--Loop Begin-->", "<!--Loop End-->");
                loopAllStr = string.Empty;
                foreach (LanguageInfo language in resume.Language)
                {
                    tempStr = loopStr;
                    tempStr = tempStr.Replace("{LanguageLevelName}", language.LanguageLevelName);
                    tempStr = tempStr.Replace("{LanguageName}", language.LanguageName);
                    tempStr = tempStr.Replace("{LanguageDescription}", language.LanguageDescription);
                    loopAllStr += tempStr;
                }
                htmlString = ReplaceLabelContent(htmlString, "<!--Loop Begin-->", "<!--Loop End-->", loopAllStr);
            }
            else
            {
                htmlString = string.Empty;
            }
            templet = ReplaceLabelContent(templet, "<!--Language Begin-->", "<!--Language End-->", htmlString);
            #endregion

            # region Skill
            if (resume.ResumeWrite.SkillWrite == 1)
            {
                htmlString = GetLabelContent(templet, "<!--Skill Begin-->", "<!--Skill End-->");
                htmlString = htmlString.Replace("{ComputerLevelName}", resume.Skill.ComputerLevelName);
                htmlString = htmlString.Replace("{ComputerSkills}", FormatHtmlString(resume.Skill.ComputerSkills, true));
                htmlString = htmlString.Replace("{Interesting}", FormatHtmlString(resume.Skill.Interesting, true));
                htmlString = htmlString.Replace("{OtherSkills}", FormatHtmlString(resume.Skill.OtherSkills, true));
                htmlString = htmlString.Replace("{PositionName}", resume.Skill.PositionName);
            }
            else
            {
                htmlString = string.Empty;
            }
            templet = ReplaceLabelContent(templet, "<!--Skill Begin-->", "<!--Skill End-->", htmlString);
            #endregion

            # region Project
            if (resume.ResumeWrite.ProjectWrite == 1)
            {
                htmlString = GetLabelContent(templet, "<!--Project Begin-->", "<!--Project End-->");
                loopStr = GetLabelContent(htmlString, "<!--Loop Begin-->", "<!--Loop End-->");
                loopAllStr = string.Empty;
                foreach (ProjectInfo project in resume.Project)
                {
                    tempStr = loopStr;
                    tempStr = tempStr.Replace("{BeginDateName}", project.BeginDateName);
                    tempStr = tempStr.Replace("{EndDateName}", project.EndDateName);
                    tempStr = tempStr.Replace("{ProjectDuty}", project.ProjectDuty);
                    tempStr = tempStr.Replace("{ProjectMemo}", project.ProjectMemo);
                    tempStr = tempStr.Replace("{ProjectName}", project.ProjectName);
                    loopAllStr += tempStr;
                }
                htmlString = ReplaceLabelContent(htmlString, "<!--Loop Begin-->", "<!--Loop End-->", loopAllStr);
            }
            else
            {
                htmlString = string.Empty;
            }
            templet = ReplaceLabelContent(templet, "<!--Project Begin-->", "<!--Project End-->", htmlString);
            #endregion

            return templet;
        }




        #endregion

    }
}