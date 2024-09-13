using Commons.Send;
using Microsoft.AspNetCore.Mvc;
using Model;
using Model.Common;
using System.Text;
using System.Xml;
//using Commons.Send;

namespace ManageNew.Controllers.Common
{
    /// <summary>
    /// 公共接口
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "共享接口")]
    public class CommonsController : ControllerBase
    {
        public readonly IConfiguration _configuration;
        public readonly Mail _mail;
        /// <summary>
        /// 构造方法
        /// </summary>
        public CommonsController(IConfiguration configuration, Mail mail)
        {
            _configuration = configuration;
            _mail = mail;
        }
        
        /// <summary>
        /// 短信提醒
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult SendMessageByPhone([FromBody] SendSms sendSms)
        {
            try
            {
                if (sendSms.Phone == "15077490574")
                {
                    return Ok(ResultMode<string>.Failed("对不起，短信发送失败"));
                }
                return Ok(ResultMode<string>.Success("发送短信通知成功", "发送短信通知成功"));

                var userId = _configuration["SentSMSConfig:userId"];
                var passName = _configuration["SentSMSConfig:passName"];

                string ret = SendMessageTool.SendMessage(userId, passName, sendSms.Phone, sendSms.Content, sendSms.SendTime);
                //ret = <root return="0" info="成功" msgid="3074688625" numbers="1" messages="1" />
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(ret);
                string code = xmlDoc.SelectSingleNode("/root/@return").Value;
                //string infos = xmlDoc.SelectSingleNode("/root/@info").Value;
                //string messages = xmlDoc.SelectSingleNode("/root/@messages").Value;
                if (code.Equals("0"))
                {
                    return Ok(ResultMode<string>.Success("发送短信通知成功", "发送短信通知成功"));
                }
                return Ok(ResultMode<string>.Failed("对不起，短信发送失败"));
            }
            catch (Exception e)
            {
                return Ok(ResultMode<string>.Failed("出错了"));

            }
            
        }

        /// <summary>
        /// 通用发送邮件
        /// </summary>
        /// <param name="subject">主题</param>
        /// <param name="boys">内容</param>
        /// <param name="emails">接收邮箱</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult SendMail(string subject,string boys,string emails= "@qq.com")
        {
            //var mail = _mail.GetMailInfo(mailType);
            //mail.Body = string.Format(mail.Body, "pos", "posId");
            bool yn = _mail.SendMail(emails, subject, boys);
            return Ok(yn);
        }
    }
}
