using Commons.Tool;
using IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Model;
using Model.Common;
using System.Xml;

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
        /// <summary>
        /// 构造方法
        /// </summary>
        public CommonsController(IConfiguration configuration)
        {
            _configuration = configuration;
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
                //if (sendSms.Phone == "15077490574")
                //{
                //    return Ok(ResultMode<string>.Failed("对不起，短信发送失败"));
                //}
                //return Ok(ResultMode<string>.Success("发送短信通知成功，扣除两条短信"));

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
                    return Ok(ResultMode<string>.Success("发送短信通知成功，扣除两条短信"));
                }
                return Ok(ResultMode<string>.Failed("对不起，短信发送失败"));
            }
            catch (Exception e)
            {
                return Ok(ResultMode<string>.Failed("出错了"));

            }
            
        }
    }
}
