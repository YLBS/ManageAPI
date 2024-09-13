
using Commons.Helper;
using Commons.Tool;
using Microsoft.Extensions.Caching.Memory;
using System.Collections;
using System.Net.Mail;
using System.Text;
using System.Xml;

namespace Commons.Send;
/// <summary>
/// 通过依赖注入使用发邮件
/// </summary>
public class Mail
{
    private readonly IMemoryCache _cache;
    private static MailInfo? info;
    public Mail(IMemoryCache cache)
    {
        _cache = cache;
    }
    #region enum MailType
    public enum MailType
    {
        /// <summary>
        /// 个人注册时发送
        /// </summary>
        PersonRegister = 1,

        /// <summary>
        /// 企业注册时发送
        /// </summary>
        MemRegister = 2,

        /// <summary>
        /// 企业邀请求职者面试时发送
        /// </summary>
        InviteInterview = 3,

        /// <summary>
        /// 业务员给企业推荐简历时发送
        /// </summary>
        GoodjobRecommandResume = 4,

        /// <summary>
        /// 企业收藏求简历时给求职者发送
        /// </summary>
        AddResumeFavorite = 5,

        /// <summary>
        /// 个人外发简历
        /// </summary>
        PersonSendResume = 6,

        /// <summary>
        /// 个人取回密码时发送
        /// </summary>
        PersonGetBackPassword = 7,

        /// <summary>
        /// 企业取回密码时发送
        /// </summary>
        MemGetBackPassword = 8,

        /// <summary>
        /// 个人应聘职位时发送
        /// </summary>
        PersonApplyPosition = 9,

        /// <summary>
        /// 后台审核不规范简历时发送
        /// </summary>
        ResumeNotFull = 10,

        /// <summary>
        /// 简历审核通过时发送
        /// </summary>
        GoodjobCheckResume = 11,

        /// <summary>
        /// 企业转发简历
        /// </summary>
        CompanySendResume = 12,
        /// <summary>
        /// 后台客户问题回复
        /// </summary>
        FeedBackQuestion = 13,
        /// <summary>
        /// 介绍职位给个人
        /// </summary>
        IntroducePosToPerson = 14,
        /// <summary>
        /// 后台发送邮件给个人
        /// </summary>
        SendEmailToPerson = 15,

        /// <summary>
        /// 求职者给朋友发送职位
        /// </summary>
        PersonSendPosition = 16,
    }
    #endregion
    /// <summary>
    /// 获取初始值
    /// </summary>
    /// <param name="mailType"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public MailInfo GetMailInfo(MailType mailType)
    {
         Hashtable mailTable = GetResource();
        if (mailTable != null)
        {
            if (mailTable[(int)mailType] == null)
            {
                throw new Exception("非法邮件编号");
            }

            info=(MailInfo)mailTable[(int)mailType];
        }
        return info;
    }
    
    /// <summary>
    /// 发送邮箱,先获取模板
    /// </summary>
    /// <param name="mailInfo"></param>
    /// <param name="mailto">接收方邮箱，多个以逗号分开</param>
    /// <returns></returns>
    public bool SendMail(MailInfo mailInfo,string mailto)
    {
        System.Net.Mail.SmtpClient mail = new System.Net.Mail.SmtpClient();
        mail.Host = mailInfo.Server;
        mail.Credentials = new System.Net.NetworkCredential(mailInfo.UserName, mailInfo.Password);

        MailMessage message = new MailMessage();
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        Encoding enCod = Encoding.GetEncoding("GB2312");
        message.From = new MailAddress(mailInfo.UserName, "俊才招聘网", enCod);

        //
        //处理多个邮件发送
        string[] addressList = mailto.Split(',');
        foreach (string address in addressList)
        {
            if (!string.IsNullOrEmpty(address))
                message.To.Add(new MailAddress(address));
        }

        message.Body = mailInfo.Body;
        message.Subject = mailInfo.Subject;
        message.SubjectEncoding = Encoding.GetEncoding("gb2312");
        message.BodyEncoding = Encoding.GetEncoding("gb2312");
        message.IsBodyHtml = true;
        try
        {
            mail.Send(message);
            return true;
        }
        catch
        {
            return false;
        }
    }
    /// <summary>
    /// 通用发邮件
    /// </summary>
    /// <param name="mailto">接收方</param>
    /// <param name="subject">主题</param>
    /// <param name="body">内容</param>
    /// <returns></returns>
    public bool SendMail(string mailto, string subject, string body)
    {
        var mailInfo = GetMailInfo(0);
        SmtpClient mail = new SmtpClient();
        mail.Host = mailInfo.Server;
        mail.Credentials = new System.Net.NetworkCredential(mailInfo.UserName, mailInfo.Password);

        MailMessage message = new MailMessage();
        Encoding enCod = Encoding.GetEncoding("GB2312");
        message.From = new MailAddress(mailInfo.UserName, "俊才招聘网", enCod);

        //
        //处理多个邮件发送
        string[] addressList = mailto.Split(',');
        foreach (string address in addressList)
        {
            if (!string.IsNullOrEmpty(address))
                message.To.Add(new MailAddress(address));
        }

        message.Body = body;
        message.Subject = subject;
        message.SubjectEncoding = Encoding.GetEncoding("gb2312");
        message.BodyEncoding = Encoding.GetEncoding("gb2312");
        message.IsBodyHtml = true;
        try
        {
            mail.Send(message);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// 根据邮件编号发送邮件
    /// </summary>
    /// <param name="mailId">发送邮件编号</param>
    /// <param name="mailto">接收地址</param>
    /// <returns></returns>
    public bool SendMail(int mailId, string mailto)
    {
        var mailInfo = GetMailInfo(mailId);
        SmtpClient mail = new SmtpClient();
        mail.Host = mailInfo.Server;
        mail.Credentials = new System.Net.NetworkCredential(mailInfo.UserName, mailInfo.Password);

        MailMessage message = new MailMessage();
        Encoding enCod = Encoding.GetEncoding("GB2312");
        message.From = new MailAddress(mailInfo.UserName, "俊才招聘网", enCod);

        //
        //处理多个邮件发送
        string[] addressList = mailto.Split(',');
        foreach (string address in addressList)
        {
            if (!string.IsNullOrEmpty(address))
                message.To.Add(new MailAddress(address));
        }

        message.Body = mailInfo.Body;
        message.Subject = mailInfo.Subject;
        message.SubjectEncoding = Encoding.GetEncoding("gb2312");
        message.BodyEncoding = Encoding.GetEncoding("gb2312");
        message.IsBodyHtml = true;
        try
        {
            mail.Send(message);
            return true;
        }
        catch
        {
            return false;
        }
    }


    #region 私有方法，读取XML文件，进行初始化

    private Hashtable GetResource()
    {
        string cacheKey = "mail.xml";
        if (!_cache.TryGetValue(cacheKey, out Hashtable result))
        {
            Hashtable resources = new Hashtable();
            result = LoadResource(resources);
            _cache.Set(cacheKey, result);
        }
        return result;
    }

    private static Hashtable LoadResource(Hashtable target)
    {
        string filePath = AppSet.ResourcesPath + "mail.xml";
        XmlDocument doc = new XmlDocument();
        try
        {
            doc.Load(filePath);
        }
        catch (Exception e)
        {
            LogConfig.TestSetConfig($"错误信息{e.Message},{e.InnerException}", "发送邮件");
            throw new Exception("加载mail.xml失败");
        }

        foreach (XmlNode node in doc.SelectSingleNode("root").ChildNodes)
        {
            if (node.NodeType != XmlNodeType.Comment)
            {
                MailInfo mailInfo = new MailInfo(node);
                target[mailInfo.ID] = mailInfo;
            }
        }
        return target;
    }
    private MailInfo GetMailInfo(int mailId)
    {
        Hashtable mailTable = GetResource();
        if (mailTable != null)
        {
            if (mailTable[mailId] == null)
            {
                throw new Exception("非法邮件编号");
            }

            info = (MailInfo)mailTable[mailId];
        }
        return info;
    }
    #endregion


}
