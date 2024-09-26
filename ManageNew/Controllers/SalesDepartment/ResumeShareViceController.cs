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
using Mails = Commons.Send.Mails;

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
        private readonly IConfiguration _configuration;
        private readonly ReadResume _readResume;

        private readonly Mails _mail;
        //private readonly IHostingEnvironment _hostingEnvironment;
        /// <summary>
        /// 构造方法
        /// </summary>

        public ResumeShareViceController(IResumeShareVice resumeShareVice, IConfiguration iConfiguration, Mails mail, ReadResume readResume)
        {
            _resumeShareVice = resumeShareVice;
            _configuration = iConfiguration;
            _mail = mail;
            _readResume = readResume;
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
            return Ok(ResultMode<bool>.Success(true));
            //实际在权限过滤器做
            //var userId = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            //var result = await _checkPermission.CheckPersonViewPermission(PersonViewEnum.ViewResume, Convert.ToInt32(userId));
            //return Ok(ResultMode<bool>.Success(result));
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
            var userId = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            int.TryParse(userId, out int id);
            string template = await _readResume.GetResumeKeyWordText(myUserId, keyWord, id);
            return Ok(ResultMode<string>.Success(template));
        }
        /// <summary>
        /// ViewResumeOne.aspx 读取简历(限制联系方式输出)
        /// </summary>
        /// <param name="myUserId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetResumeText(int myUserId)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            int.TryParse(userId, out int id);
            var template = await _readResume.GetResumeText(myUserId, id);
            return Ok(ResultMode<string>.Success(template));
        }

        /// <summary>
        /// ViewResumeOne.aspx 读取简历(限制联系方式输出)
        /// </summary>
        /// <param name="myUserId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetResumeTextForChecking(int myUserId)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            int.TryParse(userId, out int id);
            var template = await _readResume.GetResumeTextForChecking(myUserId);
            return Ok(ResultMode<string>.Success(template));
        }
        /// <summary>
        /// ViewResumeOne.aspx 读取英文简历
        /// </summary>
        /// <param name="myUserId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetResumeTextEn(int myUserId)
        {
            var template = await _readResume.GetResumeTextEn(myUserId);
            return Ok(ResultMode<string>.Success(template));
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
                string resumeTemplate = Regex.Replace(await _readResume.GetResumeTextNotContact(Convert.ToInt32(value)), "<!--ResumeButton Begin-->(.|\n)*<!--ResumeButton End-->", string.Empty);
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
                    var mail = _mail.GetMailInfo(Mails.MailType.IntroducePosToPerson);
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
                string resumeContent = await _readResume.GetResumeTextAll(resumeInfo, false);
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
                                var mail = _mail.GetMailInfo(Mails.MailType.GoodjobRecommandResume);
                                mail.Subject = string.Format(mail.Subject, l.posName, appPosition.PerName);
                                mail.Body = string.Format(mail.Body, l.contactPerson, appPosition.PerName, appPosition.ResumeID, l.posName, null, resumeContent, EnAndDecryption.DESURL(posId.ToString()), EnAndDecryption.DESURL(appPosition.ResumeID.ToString()), "10", ButtonHtmls);
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
                string resumeContent = await _readResume.GetResumeTextAll(resumeInfo, s.memberClass == 2);
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
                                var mail = _mail.GetMailInfo(Mails.MailType.PersonApplyPosition);
                                mail.Subject = string.Format(mail.Subject, l.posName, appPosition.PerName);
                                mail.Body = string.Format(mail.Body, l.contactPerson, appPosition.PerName, appPosition.ResumeID, l.posName, null, resumeContent, EnAndDecryption.DESURL(posId.ToString()), EnAndDecryption.DESURL(appPosition.ResumeID.ToString()), "0", ButtonHtmls);
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
            memUserName = EnAndDecryption.DESURL(memUserName);
            memPassWord = EnAndDecryption.DESURL(memPassWord);
            MyuserID = EnAndDecryption.DESURL(MyuserID);
            PosName = Uri.EscapeDataString(PosName); //替换Server.UrlEncode，在.net6中已被移除 //Server.UrlEncode(PosName);
            PosID = EnAndDecryption.DESURL(PosID);
            string Html = "<a href='http://hr.goodjob.cn/LoginPortal/EmailLogin.aspx?hrUsername=" + memUserName + "&hrPassword=" + memPassWord + "&LoginType=1' target='_blank'><img  src='http://image.goodjob.cn/image/email/btn01.gif' border='0' /></a>  <a href='http://hr.goodjob.cn/LoginPortal/EmailLogin.aspx?hrUsername=" + memUserName + "&hrPassword=" + memPassWord + "&LoginType=2&MyUserID=" + MyuserID + "&PosID=" + PosID + "' target='_blank'><img  src='http://image.goodjob.cn/image/email/btn02.gif' border='0' /></a>  <a href='http://hr.goodjob.cn/LoginPortal/EmailLogin.aspx?hrUsername=" + memUserName + "&hrPassword=" + memPassWord + "&LoginType=3&MyUserID=" + MyuserID + "&PosID=" + PosID + "' target='_blank'><img  src='http://image.goodjob.cn/image/email/btn03.gif' border='0' /></a>  <a href='http://hr.goodjob.cn/LoginPortal/EmailLogin.aspx?hrUsername=" + memUserName + "&hrPassword=" + memPassWord + "&LoginType=4&MyUserID=" + MyuserID + "&PosName=" + PosName + "' target='_blank'><img  src='http://image.goodjob.cn/image/email/btn04.gif' border='0' /></a>";

            return Html;
        }

        //增加推荐招聘按钮
        private string ComButtonHtml(string memUserName, string memPassWord, string MyuserID)
        {
            memUserName = EnAndDecryption.DESURL(memUserName);
            memPassWord = EnAndDecryption.DESURL(memPassWord);
            MyuserID = EnAndDecryption.DESURL(MyuserID);
            string Html = "<a href='http://hr.goodjob.cn/LoginPortal/EmailLogin.aspx?hrUsername=" + memUserName + "&hrPassword=" + memPassWord + "&LoginType=1' target='_blank'><img  src='http://image.goodjob.cn/image/email/btn01.gif' border='0' /></a>  <a href='http://hr.goodjob.cn/LoginPortal/EmailLogin.aspx?hrUsername=" + memUserName + "&hrPassword=" + memPassWord + "&LoginType=5&MyUserID=" + MyuserID + "' target='_blank'><img  src='http://image.goodjob.cn/image/email/btn05.gif' border='0' /></a>";

            return Html;
        }

        //加密并HTML编码
        //private string EnAndDecryption.DESURL(string UrlValues)
        //{
        //    return Uri.EscapeDataString(CompanyTool.EncryptDESTool(UrlValues));// Server.UrlEncode(CompanyTool.EncryptDESTool(UrlValues));
        //}
        

        #endregion

    }
}