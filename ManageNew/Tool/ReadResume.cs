using Goodjob.Resume;
using IService.Common;
using IService.SalesDepartment;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using Model;
using Model.enums;
using ServiceStack;
using System.Data;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using Commons.Helper;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace ManageNew.Tool
{
    /// <summary>
    /// 读取简历
    /// </summary>
    public class ReadResume
    {
        private readonly IResumeShareVice _resumeShareVice;
        private readonly Commons.Helper.TempletHelper _templetHelper;
        private readonly IDicService _dicService;
        private readonly CheckPermission _checkPermission;
        private readonly Commons.Helper.HtmlHelper _htmlHelper;
        /// <summary>
        /// 构造方法
        /// </summary>
        public ReadResume(IResumeShareVice resumeShareVice, Commons.Helper.TempletHelper templetHelper,IDicService dic, CheckPermission checkPermission, Commons.Helper.HtmlHelper htmlHelper)
        {
            _resumeShareVice = resumeShareVice;
            _templetHelper = templetHelper;
            _dicService = dic;
            _checkPermission = checkPermission;
            _htmlHelper = htmlHelper;
        }

        /// <summary>
        /// 读取带关键字搜索的简历(限制联系方式输出)
        /// </summary>
        /// <param name="myUserId"></param>
        /// <param name="keyWord"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<string> GetResumeKeyWordText(int myUserId, string keyWord,int userId)
        {
            ResumeInfo resume = await _resumeShareVice.GetResume(myUserId);
            if (resume == null)
            {
                return "";
            }
            //简历不完整的不显示
            //if (resume.ResumeWrite.AllWrite == 0) return null;

            string htmlString = string.Empty;
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

            templet = GetBaseInfo(templet, resume);

            #endregion

            #region Education(*)

            templet = GetEducation(templet, resume, keyWord);

            #endregion

            #region WorkExp(*)

            templet = GetWorkExp(templet, resume, keyWord); 

            #endregion

            #region HopeJob(*)
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
                    salaryName = dicSalaryNews.Where(s => s.Id == resume.HopeJob.Salary).Select(s => s.Name).FirstOrDefault();
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
            
            if (await HasSalerDownloadResume(myUserId, userId))
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

            return templet;
        }
        /// <summary>
        /// 读取简历(限制联系方式输出)
        /// </summary>
        /// <param name="myUserId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<string> GetResumeText(int myUserId,int userId)
        {
            ResumeInfo resume = await _resumeShareVice.GetResume(myUserId);
            if (resume == null) return "";

            string htmlString = string.Empty;
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

            templet = GetBaseInfo(templet, resume);


            #endregion

            #region Education(*)

            templet = GetEducation(templet, resume); 
            
            #endregion

            # region WorkExp(*)
            templet = GetWorkExp(templet, resume);
            #endregion

            #region HopeJob(*)
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
            if (!await _checkPermission.CheckCompanyViewPermission(CompanyViewEnum.ViewCompanyUsername, Convert.ToInt32(userId)))
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
            return templet;
        }
        /// <summary>
        /// 读取英文简历
        /// </summary>
        /// <param name="myUserId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<string> GetResumeTextEn(int myUserId)
        {
            ResumeInfo resume = await _resumeShareVice.GetResume(myUserId);
            if (resume == null) return "";


            //简历不完整的不显示
            //if (resume.ResumeWrite.AllWrite == 0) return null;

            string htmlString = string.Empty;
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
            templet = GetBaseInfo(templet, resume);

            #endregion

            #region Education(*)

            templet = GetEducation(templet, resume);

            #endregion

            #region WorkExp(*)
            templet = GetWorkExp(templet, resume);
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
            return templet;

            #endregion

        }

        /// <summary>
        /// 读取没有联系方式的简历
        /// </summary>
        /// <param name="myUserId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<string> GetResumeTextNotContact(int myUserId)
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

        public async Task<string> GetResumeTextAll(ResumeInfo? resume, bool isEnabledContent)
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

            templet = GetBaseInfo(templet, resume);
            #endregion

            # region Education(*)

            templet = GetEducation(templet, resume);

            #endregion

            #region WorkExp(*)

            templet = GetWorkExp(templet, resume);

            
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

        /// <summary>
        /// 获取待审核简历信息(不限制联系方式输出)
        /// </summary>
        /// <param name="myUserId"></param>
        /// <returns></returns>
        public async Task<string> GetResumeTextForChecking(int myUserId)
        {
            var resume = await _resumeShareVice.GetResume(myUserId);
            if (resume == null) return string.Empty;
            string htmlString = string.Empty;
            string dataStr = string.Empty;
            string tempStr = string.Empty;
            string loopStr = string.Empty;
            string loopAllStr;

            #region Resume Templet
            string templet = _templetHelper.GetTemplet(TempletType.ResumeTemplet);
            templet = templet.Replace("{MyUserID}", resume.MyUserID.ToString());
            templet = templet.Replace("{UpdateDate}", resume.UpdateDateName);
            templet = templet.Replace("{HitCount}", resume.HitCount.ToString());
            templet = templet.Replace("{EngResumeFlag}", resume.EngResumeFlag.ToString());
            templet = templet.Replace("{PhotoVisitUrl}", resume.Photo.PhotoVisitUrl);
            #endregion
            var recommendId = await _resumeShareVice.GetUserRecommendId(myUserId);
            if (recommendId > 0)
            {
                templet = templet.Replace("{RecommendText}", "此简历为注册推荐简历");

            }
            else
            {
                templet = templet.Replace("{RecommendText}", "");

            }
            #region BaseInfo(*)

            templet = GetBaseInfo(templet, resume);


            #endregion

            #region Education(*)

            templet = GetEducation(templet, resume);

            #endregion

            # region WorkExp(*)

            templet = GetWorkExp(templet, resume);
            
            #endregion

            # region HopeJob(*)
            if (resume.ResumeWrite.HopeWrite == 1)
            {
                //需重新查询 意向岗位
                htmlString = GetLabelContent(templet, "<!--HopeJob Begin-->", "<!--HopeJob End-->");
                htmlString = htmlString.Replace("{CareerDirection}", FormatHtmlString(resume.HopeJob.CareerDirection, true));
                htmlString = htmlString.Replace("{CheckinDateName}", resume.HopeJob.CheckinDateName);
                htmlString = htmlString.Replace("{EatHouseNeeded}", resume.HopeJob.EatHouseNeeded);
                htmlString = htmlString.Replace("{JobSeeking}", resume.HopeJob.JobSeeking);
                htmlString = htmlString.Replace("{JobTypeName}", resume.HopeJob.JobTypeName);
                htmlString = htmlString.Replace("{OtherNeeded}", resume.HopeJob.OtherNeeded);
                try
                {

                    htmlString = htmlString.Replace("{SalaryName}", resume.HopeJob.SalaryName);
                }
                catch (Exception)
                {

                    htmlString = htmlString.Replace("{SalaryName}", "面议");

                }
                htmlString = htmlString.Replace("{JobLocationName}", resume.HopeJob.JobLocationName);


                string hl = "";
                string nameStr = "";
                try
                {
                    var jobfs = await _resumeShareVice.GetJobFunction_s(myUserId);
                    if (jobfs.Count > 0)
                    {
                        hl += "jobs " + nameStr;
                        foreach (var item in jobfs)
                        {
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
                                nameStr = await _resumeShareVice.GetJobFunctionListBySmallID(Convert.ToInt32(arrid[i]));
                            }
                            else
                                nameStr += " , " + await _resumeShareVice.GetJobFunctionListBySmallID(Convert.ToInt32(arrid[i]));
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

        #region 私有方法
        

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
        private string GetReplaceKey(string webvalues, string keyWord)
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


        private string GetBaseInfo(string template, ResumeInfo resume)
        {

            string htmlString = string.Empty;
            if (resume.ResumeWrite.BaseWrite == 1)
            {
                htmlString = GetLabelContent(template, "<!--BaseInfo Begin-->", "<!--BaseInfo End-->");
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
            template = ReplaceLabelContent(template, "<!--BaseInfo Begin-->", "<!--BaseInfo End-->", htmlString);
            return template;
        }

        private string GetEducation(string template, ResumeInfo resume,string keyWord)
        {
             
            string dataStr = string.Empty;
            string htmlString = GetLabelContent(template, "<!--Education Begin-->", "<!--Education End-->");
            switch (resume.ResumeWrite.EduWrite)
            {
                case 2:
                    dataStr = GetLabelContent(htmlString, "<!--OldData Begin-->", "<!--OldData End-->");
                    dataStr = dataStr.Replace("{EduText}", GetReplaceKey(resume.EduText, keyWord));
                    htmlString = ReplaceLabelContent(htmlString, "<!--DataCase Begin-->", "<!--DataCase End-->", dataStr);
                    break;
                case 1:
                    dataStr = GetLabelContent(htmlString, "<!--NewData Begin-->", "<!--NewData End-->");
                    string loopStr = GetLabelContent(dataStr, "<!--Loop Begin-->", "<!--Loop End-->");
                    string loopAllStr = string.Empty;
                    foreach (EducationInfo education in resume.Education)
                    {
                        string tempStr = loopStr;
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
            template = ReplaceLabelContent(template, "<!--Education Begin-->", "<!--Education End-->", htmlString);
            return template;
        }


        private string GetEducation(string template, ResumeInfo resume)
        {
            string htmlString = GetLabelContent(template, "<!--Education Begin-->", "<!--Education End-->");
            string dataStr;
            switch (resume.ResumeWrite.EduWrite)
            {
                case 2:
                    dataStr = GetLabelContent(htmlString, "<!--OldData Begin-->", "<!--OldData End-->");
                    dataStr = dataStr.Replace("{EduText}", FormatHtmlString(resume.EduText, true));
                    htmlString = ReplaceLabelContent(htmlString, "<!--DataCase Begin-->", "<!--DataCase End-->", dataStr);
                    break;
                case 1:
                    dataStr = GetLabelContent(htmlString, "<!--NewData Begin-->", "<!--NewData End-->");
                    string loopStr = GetLabelContent(dataStr, "<!--Loop Begin-->", "<!--Loop End-->");
                    string loopAllStr = string.Empty;
                    foreach (EducationInfo education in resume.Education)
                    {
                        string tempStr = loopStr;
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
            template = ReplaceLabelContent(template, "<!--Education Begin-->", "<!--Education End-->", htmlString);
            return template;
        }

        private string GetWorkExp(string template, ResumeInfo resume,string keyWord)
        {
            string htmlString = GetLabelContent(template, "<!--WorkExp Begin-->", "<!--WorkExp End-->");
            string dataStr;
            switch (resume.ResumeWrite.WorkWrite)
            {
                case 2:
                    dataStr = GetLabelContent(htmlString, "<!--OldData Begin-->", "<!--OldData End-->");
                    dataStr = dataStr.Replace("{WorkText}", GetReplaceKey(resume.WorkText, keyWord));
                    htmlString = ReplaceLabelContent(htmlString, "<!--DataCase Begin-->", "<!--DataCase End-->", dataStr);
                    break;
                case 1:
                    dataStr = GetLabelContent(htmlString, "<!--NewData Begin-->", "<!--NewData End-->");
                    string loopStr = GetLabelContent(dataStr, "<!--Loop Begin-->", "<!--Loop End-->");
                    string loopAllStr = string.Empty;
                    foreach (WorkExpInfo workExp in resume.WorkExp)
                    {
                        try
                        {

                            string tempStr = loopStr;
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
            template = ReplaceLabelContent(template, "<!--WorkExp Begin-->", "<!--WorkExp End-->", htmlString);
            return template;
        }

        private string GetWorkExp(string template, ResumeInfo resume)
        { 
            string dataStr;
            string htmlString = GetLabelContent(template, "<!--WorkExp Begin-->", "<!--WorkExp End-->");
            switch (resume.ResumeWrite.WorkWrite)
            {
                case 2:
                    dataStr = GetLabelContent(htmlString, "<!--OldData Begin-->", "<!--OldData End-->");
                    dataStr = dataStr.Replace("{WorkText}", FormatHtmlString(resume.WorkText, true));
                    htmlString = ReplaceLabelContent(htmlString, "<!--DataCase Begin-->", "<!--DataCase End-->", dataStr);
                    break;
                case 1:
                    dataStr = GetLabelContent(htmlString, "<!--NewData Begin-->", "<!--NewData End-->");
                    string loopStr = GetLabelContent(dataStr, "<!--Loop Begin-->", "<!--Loop End-->");
                    string loopAllStr = string.Empty;
                    foreach (WorkExpInfo workExp in resume.WorkExp)
                    {
                        try
                        {
                            string tempStr = loopStr;
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
            template = ReplaceLabelContent(template, "<!--WorkExp Begin-->", "<!--WorkExp End-->", htmlString);
            return template;
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

        #endregion

    }
}
