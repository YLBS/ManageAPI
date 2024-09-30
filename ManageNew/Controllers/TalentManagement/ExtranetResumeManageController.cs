using Commons.Send;
using Goodjob.Common;
using IService.TalentManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using Model.Common;
using System.Xml;

namespace ManageNew.Controllers.TalentManagement
{
    /// <summary>
    /// 外网导入简历管理 ResumeListNew.aspx
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "人才管理")]
    public class ExtranetResumeManageController : ControllerBase
    {
        private readonly IExtranetResumeManage _extranetResumeManage;
        private readonly Mails _mail;
        private readonly IConfiguration _configuration;
        /// <summary>
        /// 构造
        /// </summary>
        public ExtranetResumeManageController(IExtranetResumeManage extranetResumeManage, Mails mail, IConfiguration configuration)
        {
            _extranetResumeManage = extranetResumeManage;
            _mail = mail;
            _configuration = configuration;
        }


        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="keyType">搜索关键字类型，1姓名，2用户名，3简历编号，4电子邮箱，5手机号</param>
        /// <param name="keyWord">关键字</param>
        /// <param name="isQd">是否去电，1是，0否。默认0</param>
        /// <param name="qdPeople">去电人Id</param>
        /// <param name="qdType">去电结果 1已联系已推荐 2关机 3空号 4拒接 5隐藏简历 6待删除 7未接停</param>
        /// <param name="userFlag">用户是否激活，-1不限，1是，0否，默认0</param>
        /// <param name="sendSms">是否已发送短信，-1不限，1是，0否,默认-1</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetData(int keyType,string? keyWord,int isQd,int qdPeople,int qdType,int userFlag,int sendSms=-1)
        {
            string filter = string.Empty;
            if (!string.IsNullOrEmpty(keyWord))
            {
                switch (keyType)
                {
                    case 1:
                        filter = " and My_Resume.PerName='" + keyWord + "'";
                        break;
                    case 2:
                        filter = " and My_Users.UserName='" + keyWord + "'";
                        break;
                    case 3:
                        try
                        {
                            int id = int.Parse(keyWord);
                            filter = " and My_Resume.MyUserId=" + id;
                        }
                        catch (Exception)
                        {
                            return BadRequest("选择编号时，请输入纯数字");
                        }
                        break;
                    case 4:
                        filter = " andMy_Users.Email='" + keyWord + "'";
                        break;
                    case 5:
                        filter = " and My_Resume.MobileNum='" + keyWord + "'";
                        break;
                }
            }

            if (sendSms == 1)
                filter += "  and m2.SendCount >0";
            else if(sendSms ==0 )
                filter += " and m2.SendCount = 0 ";
            
            if (isQd == 1)
                filter += " And IsdqStatus=1 ";
            else
                filter += " And IsdqStatus=0 ";
            if (qdPeople != 0)
                filter += " And m3.UserId= " + qdPeople;
            if (qdType!=0)  
                filter += " and QdType=" + qdType;
            if (userFlag == 1) //判断激活
            {
                if (isQd == 0 && sendSms == -1)//默认为 登录时间大于去电时间 或者 登录时间大于短信发送时间
                {
                    filter += " And ((LastLoginDate>qdTime) or (LastLoginDate>FirstSendTime) )";
                }
                if (isQd != 0) //是否去电为否时， 选择查询去电条件时 登录时间就大于去电时间，
                {
                    filter += " And (LastLoginDate>qdTime)";
                }
                if (sendSms != -1) // 是否已发送短信不为 不限， 选择查询发送短信件时 登录时间大于短信发送时间
                {
                    filter += " And (LastLoginDate>FirstSendTime)";
                }
            }
            var result = await _extranetResumeManage.GetData(filter);
            return Ok(ResultMode<object>.Success(result));
        }

        /// <summary>
        /// 发邮件，未测试
        /// </summary>
        /// <param name="resumeIds">简历Id集合  1,2,3,4</param>
        /// <param name="subject">主题不传默认为 俊才网简历激活,后续可由调用者定义</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> SendMail(string resumeIds,string? subject)
        {
            if (string.IsNullOrEmpty(subject))
                subject = "俊才网简历激活";
            int y = 0;
            int n = 0;
            string failedMsg = string.Empty;
            string noEmail=string.Empty; 
            string successId = string.Empty;
            string[] resumeId= resumeIds.Split(',');
            int[] ids = resumeId.Select(m => Convert.ToInt32(m)).ToArray();
            var info = await _extranetResumeManage.GetMyUserInfoAsync(ids);
            var tempPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "wwjhjl.html");
            string body = Commons.Helper.FileHelper.GetFileText(tempPath);
            foreach (var item in info)
            {
                if (string.IsNullOrEmpty(item.Email))
                {
                    if (string.IsNullOrEmpty(noEmail))
                        noEmail = item.MyUserId.ToString();
                    else
                        noEmail += $",{item.MyUserId}";
                    n++;
                    continue;
                }

                string boys = body.Replace("$MyuserID", item.MyUserId.ToString()).Replace("$UerName", item.UserName).Replace("$passWord", item.Password);
                bool yn = _mail.SendMail(item.Email, subject, boys);
                if (yn)
                {
                    y++;
                    if (string.IsNullOrEmpty(successId))
                        successId = item.MyUserId.ToString();
                    else
                        successId += $",{item.MyUserId}";
                }
                else
                {
                    n++;
                    if (string.IsNullOrEmpty(failedMsg))
                        failedMsg = $"{item.MyUserId}";
                    else
                        failedMsg += $",{item.MyUserId}";
                }
            }
            if (n != 0)
            {
                return Ok(ResultMode<string>.Success("",$"成功发送{y}条邮件,失败{n}条,,[{successId}]发送成功,[{failedMsg}]发送失败，[{noEmail}]邮箱编号为空"));
            }

            return Ok(ResultMode<string>.Success("", $"成功发送{y}条邮件"));
        }

        /// <summary>
        /// 发送短信，未测试
        /// </summary>
        /// <param name="resumeIds"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> SendMessage(string resumeIds,string msg)
        {
            string[] resumeId = resumeIds.Split(',');
            int[] ids = resumeId.Select(m => Convert.ToInt32(m)).ToArray();
            var info = await _extranetResumeManage.GetMyUserInfoAsync(ids);
            string noPhone=string.Empty;
            string failedMsg=string.Empty;
            string successId = string.Empty;
            int y = 0;
            int n = 0;
            foreach (var item in info)
            {
                if (string.IsNullOrEmpty(item.Phone))
                {
                    if (string.IsNullOrEmpty(noPhone))
                        noPhone = item.MyUserId.ToString();
                    else
                        noPhone += $",{item.MyUserId}";
                    n++;
                    continue;
                }

                msg = msg.Replace("{username}", item.UserName).Replace("{passWord}", item.Password).ToString();
                var userId = _configuration["SentSMSConfig:userId"];
                var passName = _configuration["SentSMSConfig:passName"];

                string ret = SendMessageTool.SendMessage(userId, passName, item.Phone, msg, "");
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(ret);
                string code = xmlDoc.SelectSingleNode("/root/@return").Value;
                if (code.Equals("0"))
                {
                    if (string.IsNullOrEmpty(successId))
                        successId = item.MyUserId.ToString();
                    else
                        successId += $",{item.MyUserId}";
                    y++;
                }
                else
                {
                    n++;
                    if (string.IsNullOrEmpty(failedMsg))
                        failedMsg = $"{item.MyUserId}";
                    else
                        failedMsg += $",{item.MyUserId}";
                }
            }
            if (n != 0)
            {
                return Ok(ResultMode<string>.Success("", $"成功发送{y}条邮件,失败{n}条,[{successId}]发送成功,[{failedMsg}]发送失败，[{noPhone}]手机号为空"));
            }
            return Ok(ResultMode<string>.Success("", $"成功发送{y}条邮件"));
        }




    }
}
