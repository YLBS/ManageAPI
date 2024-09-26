using Commons.Send;
using IService.TalentManagement;
using ManageNew.Tool;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using Model.Common;
using Model.enums;
using Model.TalentManagement;

namespace ManageNew.Controllers.TalentManagement
{
    /// <summary>
    /// 审核简历
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "人才管理")]
    public class CheckResumeController : ControllerBase
    {
        private readonly ICheckResume _check;
        private readonly Mails _mail;
        /// <summary>
        /// 构造方法
        /// </summary>
        public CheckResumeController(ICheckResume check,Mails mail)
        {
            _check = check;
            _mail = mail;
        }
       
        /// <summary>
        /// 检查权限
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult CheckPersonPermission()
        {
            //var userId = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            //var result = await _checkPermission.CheckPersonPermission(PersonEnum.CheckResume, Convert.ToInt32(userId));
            return Ok(ResultMode<bool>.Success(true));
        }
      
        /// <summary>
        /// 获取搜索选项
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetKeyWordType()
        {
            List<KeyValue> keyValueList = new List<KeyValue>();
            keyValueList.Add(new KeyValue() { Id = (int)ResumeKeywordTypes.PerName, Name = "姓名" });
            keyValueList.Add(new KeyValue() { Id = (int)ResumeKeywordTypes.UserName, Name = "用户名" });
            keyValueList.Add(new KeyValue() { Id = (int)ResumeKeywordTypes.MyUserId, Name = "简历编号" });
            keyValueList.Add(new KeyValue() { Id = (int)ResumeKeywordTypes.Email, Name = "电子邮箱" });
            keyValueList.Add(new KeyValue() { Id = (int)ResumeKeywordTypes.MobileNum, Name = "手机" });
            return Ok(ResultMode<object>.Success(keyValueList));
        }

        /// <summary>
        /// 新注册待审核简历,ResumeList.aspx 获取数据 siteType 这里没有用，不需要了
        /// </summary>
        /// <param name="checkType">简历状态，新注册待审核简历获取数据传1，复审更新简历 传4</param>
        /// <param name="keyWordType">搜索关键字类型，上一个接口返回</param>
        /// <param name="provinceId">省Id</param>
        /// <param name="cityId">市Id</param>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="orderByType">升序降序，默认降序</param>
        /// <param name="orderField">排序字段，应由后端处理，这里不安全</param>
        /// <param name="keyWord">关键字</param>
        /// <param name="siteType">站内1，站外2</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetResumeList(int checkType, int keyWordType, int provinceId, int cityId,
            DateTime? beginDate, DateTime? endDate, string? orderByType, string orderField = "UpdateDate",
            string? keyWord = "", int siteType = 1)
        {
            //orderField 由前端传，不安全，待优化
            string filter = string.Empty;
            if (!string.IsNullOrEmpty(keyWord))
            {
                if (!Enum.IsDefined(typeof(ResumeKeywordTypes), keyWordType))
                    return NotFound("参数keyWordType错误");
                ResumeKeywordTypes k = (ResumeKeywordTypes)keyWordType;
                switch (k)
                {
                    case ResumeKeywordTypes.PerName:
                        filter += " and PerName='" + keyWord + "'";
                        break;
                    case ResumeKeywordTypes.Email:
                        filter += " and Email='" + keyWord + "'";
                        break;
                    case ResumeKeywordTypes.MobileNum:
                        filter += " and MobileNum='" + keyWord + "'";
                        break;
                    case ResumeKeywordTypes.UserName:
                        filter += " and UserName='" + keyWord + "'";
                        break;
                    case ResumeKeywordTypes.MyUserId:
                        try
                        {
                            int id = int.Parse(keyWord);
                            filter += " and MyUserId=" + id;
                        }
                        catch (Exception)
                        {
                            return BadRequest("选择编号时，请输入纯数字");
                        }
                        break;
                }
            }

            //if (siteType != 1)
            //    filter += " and siteType>100000000 ";
            //else
            //    filter += " and siteType<100000000 ";
            if (cityId != 0)
                filter += " and Location_C=" + cityId;
            else
            {
                if (provinceId != 0)
                    filter += " and Location_P =" + provinceId;
            }

            if (beginDate != null && endDate != null)
                filter += " and UpdateDate between '" + beginDate + "' and '" +
                          endDate.Value.AddDays(1).AddSeconds(-1) + "'";
            else if (beginDate != null)
                filter += " and UpdateDate >= '" + beginDate + "'";
            else if (endDate != null)
                filter += " and UpdateDate >= '" + endDate.Value.AddDays(1).AddSeconds(-1) + "'";
            if (checkType != 0)
                filter += " and CheckFlag=" + checkType;
            string order = "asc";
            if (orderByType != null && (orderByType == "DESC" || orderByType == "desc"))
                order = orderByType;
            if (!string.IsNullOrEmpty(orderField))
            {
                switch (orderField)
                {
                    case "ResumeID":
                        filter += $" order by MyUserID {order}";
                        break;
                    case "Location":
                        filter += $" order by Location_P {order},Location_C {order}";
                        break;
                    case "RegisterDate":
                        filter += $" order by RegisterDate {order}";
                        break;
                    case "UpdateDate":
                        filter += $" order by UpdateDate {order}";
                        break;
                    case "CheckFlag":
                        filter += $" order by CheckFlag {order}";
                        break;
                }
            }

            var result = await _check.GetCheckResume(filter);

            return Ok(ResultMode<object>.Success(result));
        }
        /// <summary>
        /// 获取 待审核、更新待复审 的简历数量，不为0，隐藏初始化按钮，已注释，
        /// 只判断显示的不同文字
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetResumeCount()
        {
            int count = await _check.Count();
            return Ok(ResultMode<int>.Success(count));
        }

        /// <summary>
        /// 点击审核选中简历，跳 CheckResume.aspx 进行审核简历，通过，未测试
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> CheckResumePass(int myUserId, string perName, string email, bool sendMail, bool recommended)
        {
            
            if (myUserId == 0)
                return Ok(ResultMode<bool>.Success(false));
            bool yn = await _check.CheckResume(myUserId, 2);
            if (yn && sendMail)
            {
                var s = await _check.GetUserNameAndPassWord(myUserId);
                if (s.passWord != "" && s.userName != "")
                {
                    var mail = _mail.GetMailInfo(Mails.MailType.GoodjobCheckResume);
                    mail.Body = mail.Body.Replace("{0}", perName).Replace("{1}", s.userName).Replace("{2}", s.passWord);
                    _mail.SendMail(mail,email);
                }
            }
            if (yn && recommended)
            {
                _check.InsertReComMyUserId(myUserId);
            }
            return Ok(ResultMode<bool>.Success(yn));
        }
        /// <summary>
        /// 点击审核选中简历，跳 CheckResume.aspx 进行审核简历，不通过,未测试
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> CheckResumeNotPass(int myUserId, string mailContent, string email, bool sendMail)
        {
            if (myUserId == 0)
                return Ok(ResultMode<bool>.Success(false));
            bool yn = await _check.CheckResume(myUserId, 3);
            if (yn && sendMail)
            {
                var s = await _check.GetUserNameAndPassWord(myUserId);
                if (s.passWord != "" && s.userName != "")
                {
                    var mail = _mail.GetMailInfo(Mails.MailType.ResumeNotFull);
                    mail.Body = mailContent.Replace("{1}", s.userName).Replace("{2}", s.passWord);
                    _mail.SendMail(mail, email);
                }
            }
            return Ok(ResultMode<bool>.Success(yn));
        }

        /// <summary>
        /// 点击初始化代审核简历，谨慎调用，会先删除原来数据再写入新的数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> InitData()
        {
            int i = await _check.InitData();
            return Ok(ResultMode<string>.Success($"成功初始化{i}条数据"));
        }
    }
}
