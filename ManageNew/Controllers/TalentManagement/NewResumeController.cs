using Commons.Helper;
using IService.TalentManagement;
using ManageNew.Tool;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.enums;
using Model;
using Model.TalentManagement;
using IService.SalesDepartment;
using Commons.Send;
using IService.ResumeSearch;

namespace ManageNew.Controllers.TalentManagement
{
    /// <summary>
    /// NewResumeList.aspx,新注册不完整简历
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "人才管理")]
    public class NewResumeController : ControllerBase
    {

        //private readonly CheckPermission _checkPermission;
        private readonly INewResumeService _newResumeService;
        private readonly IResumeShareVice _resumeShareVice;
        private readonly HtmlHelper _htmlHelper;
        private readonly Mails _mail;
        private readonly IExtranetResumeCheck _extranetResumeCheck;
        /// <summary>
        /// 构造方法
        /// </summary>
        public NewResumeController(INewResumeService newResumeService, IResumeShareVice resume, Mails mail, HtmlHelper htmlHelper, IExtranetResumeCheck extranet)
        {
            _newResumeService = newResumeService;
            _resumeShareVice = resume;
            _mail = mail;
            _htmlHelper = htmlHelper;
            _extranetResumeCheck = extranet;
        }
       
        /// <summary>
        /// 获取数据，原路径已有参数 allWrite为0，fullType为2
        /// </summary>
        /// <param name="keyType">搜索选项</param>
        /// <param name="keyWord">搜索关键字</param>
        /// <param name="isQd">是否去电，默认未去电0,已去电1</param>
        /// <param name="rdPerName">是否排除姓名未填写</param>
        /// <param name="year">查询年份</param>
        /// <param name="month">月份</param>
        /// <param name="source">注册来源</param>
        /// <param name="userId">去电人Id</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetData(int? keyType=1, string? keyWord = "", int isQd = 0,
            bool rdPerName = true, int? year =0, int? month =0, int? source =0, int? userId =0)
        {
            string filter = " and My_Resume.MyUserID not in (SELECT  [MyUserID] FROM [Goodjob].[dbo].[Sys_RegisterFrom] group by MyUserID)  AND (My_Resume.AllWrite = 0) AND (My_Resume.CheckFlag = 0) and (My_Resume.ResumeStatus<>3) and HopeWrite=0 and (datediff(day,My_Resume.RegisterDate,getdate())>-1) and MobileNum<>'' ";
            if (rdPerName)
            {
                filter += " and My_Resume.PerName != '' ";
            }

            if (year != null && year != 0)
            {
                filter += $" and YEAR(My_Users.RegisterDate) ={year} ";
            }
            if (month != null && month != 0)
            {
                filter += $"  and MONTH(My_Users.RegisterDate) = {month} ";
            }
            if (source != null && source != 0)
            {
                filter += $"  and  My_Users.RegisterFrom= {source} ";
            }

            if (isQd == 1 || isQd == 0)
            {
                filter += $"  and IsdqStatus = {isQd} ";
            }

            if (userId != 0)
            {
                filter += $"  and My_Resume.MyUserID in (select MyUserId from [Goodjob].[dbo].[Pub_UserqdRecord] where UserId = 53 ) {userId} ";
            }
            if (!string.IsNullOrEmpty(keyWord))
            {
                switch (keyType)
                {
                    case 1:
                        filter += $" and My_Resume.PerName = '{keyWord}' ";
                        break;
                    case 2:
                        filter += $" and My_Users.UserName = '{keyWord}' ";
                        break;
                    case 3:
                        try
                        {
                            int id = int.Parse(keyWord);
                            filter += " and My_Resume.MyUserID=" + id + " and YEAR(My_Resume.RegisterDate) = '" + DateTime.Today.Year + "'";
                        }
                        catch (Exception)
                        {
                            return BadRequest("选择编号时，请输入纯数字");
                        }
                        break;
                    case 4:
                        filter += $" and My_Users.Email = '{keyWord}' ";
                        break;
                    case 5:
                        filter += $" and My_Resume.MobileNum = '{keyWord}' ";
                        break;
                }
            }

            var s = await _newResumeService.GetNewResumeListCollection(filter, "mr.RegisterDate", "DESC");
            return Ok(ResultMode<Object>.Success(new { s.Count, Data = s }));
        }
        
        /// <summary>
        /// 获取账号密码,返回字符串，，审核不通过完整简历使用到
        /// </summary>
        /// <param name="myUserId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetUserPass(int myUserId)
        {
            //权限在过滤器做
            //var userId = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            //var result =
            //    await _checkPermission.CheckPersonViewPermission(PersonViewEnum.ViewPersonUsername,
            //        Convert.ToInt32(userId));
            //if (!result)
            //    return Forbid();

            var dictionary = await _extranetResumeCheck.GetUserPass(myUserId);
            string key=string.Empty;
            string value = string.Empty;
            if (dictionary.Key != null)
                key = dictionary.Key.ToString();
            if (dictionary.Value != null)
                value = dictionary.Value.ToString();
            if (key == "" || value == "")
                return NotFound(ResultMode<string>.Failed("没找着"));
            return Ok(ResultMode<string>.Success($"{key},{value}"));
        }
        /// <summary>
        /// 获取账号密码,返回json 
        /// </summary>
        /// <param name="myUserId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetUserPassN(int myUserId)
        {
            var dictionary = await _extranetResumeCheck.GetUserPass(myUserId);
            string key = string.Empty;
            string value = string.Empty;
            if (dictionary.Key != null)
                key = dictionary.Key.ToString();
            if (dictionary.Value != null)
                value = dictionary.Value.ToString();
            if (key == "" || value == "")
                return NotFound(ResultMode<string>.Failed("没找着"));
            return Ok(ResultMode<object>.Success(new { account = key, passWord = value }));
        }
        /// <summary>
        /// SendMessage.aspx 给人才留言,留言模板暂时没有接口
        /// </summary>
        /// <param name="intInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddMsg([FromBody] MessageInfo intInfo)
        {
            string userIdStr = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            int.TryParse(userIdStr, out int id);
            AddMessageInfo message= new AddMessageInfo();
            message.Id = intInfo.Id;
            message.Subject = intInfo.Subject;
            message.Content = intInfo.Content;
            message.UserId = id;
            message.UserName = User.Claims.FirstOrDefault(c => c.Type == "userName")?.Value;
            await _newResumeService.InsertPersonMessage(message);
            string email = string.Join(",", await _resumeShareVice.GetPersonEmail(new []{ intInfo.Id }));
            if (!string.IsNullOrEmpty(email))
            {
                string body = _htmlHelper.FormatHtmlString(intInfo.Content, true);
                _mail.SendMail(email, "俊才网简历注册提醒：" + intInfo.Subject, body);
            }
            return Ok(ResultMode<string>.Success("", "留言成功"));
        }
        /// <summary>
        /// 删除简历,未测试
        /// </summary>
        /// <param name="myUserId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Del(int myUserId)
        {
            //var userId = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            //var result = await _checkPermission.CheckPersonPermission(PersonEnum.DeleteResume, Convert.ToInt32(userId));
            //if (result) //有权限
            //{
                
            //}
            await _newResumeService.DeleteResume(myUserId);
            return Ok(ResultMode<string>.Success("", "删除成功"));

        }

        #region 发送短信
        /// <summary>
        /// SendSMS.aspx发送短信前，获取用户信息,返回字符串 姓名,手机号,短信内容
        /// 待优化，应该返回json
        /// </summary>
        /// <param name="myUserId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetInfo(int myUserId)
        {
            var result = await _newResumeService.GetMySMSInfo(myUserId);
            if(result==null)
                return NotFound($"简历{myUserId}不存在");
            string userName = !string.IsNullOrEmpty(result.PerName) ? result.PerName : result.UserName;
            string content = string.Empty;
            var list = await _resumeShareVice.GetResume(myUserId);
            if (list.ResumeWrite.BaseWrite == 0 || list.ResumeWrite.HopeWrite == 0)
            {
                content = "您的求职信息还未完善，点此 http://jcw2.cn/py 10秒完善简历，与企业直聊，好工作轻松找！";
            }
            return Ok(ResultMode<string>.Success($"{userName}-{result.MobileNum}-{content}"));

        }
        /// <summary>
        /// SendSMS.aspx ,SentSMSNew.aspx,添加记录,当添加成功后，由前端调用发送短信的接口进行发短信
        /// </summary>
        /// <param name="myUserId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> AddRecordInfo(int myUserId)
        {
            var result = await _newResumeService.GetMySMSInfo(myUserId);
            if (result == null)
                return NotFound($"简历{myUserId}不存在");
            string userName = result.UserName;
            MySMSInfo mySMSInfo = new MySMSInfo();
            mySMSInfo.UserName = userName;
            mySMSInfo.Password=result.Password;
            mySMSInfo.MyUserId = myUserId;
            bool yn = await _newResumeService.CreateMySMS(mySMSInfo);
            if (yn)
            {
                return Ok(ResultMode<string>.Success("", "添加成功"));
            }

            return Ok(ResultMode<string>.Failed("添加失败"));
        }
        #endregion

        /// <summary>
        /// 设置为去电
        /// </summary>
        /// <param name="myUserId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> SetQd(int myUserId)
        {
            string userIdStr = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            int.TryParse(userIdStr, out int userId);
            if (await _newResumeService.UpdateResumeIsqd(myUserId))
            {
                if (await _newResumeService.AddqdRecord(myUserId, userId))
                {
                    return Ok(ResultMode<string>.Success("", "设置成功"));
                }
            }
            return Ok(ResultMode<string>.Failed("设置失败"));
        }
    }
}
