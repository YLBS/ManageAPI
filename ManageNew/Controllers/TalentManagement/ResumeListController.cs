using IService.TalentManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.TalentManagement;
using Model;
using ManageNew.Tool;
using Model.enums;
using IService.ResumeSearch;
using Commons.Tool;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using IService.SalesDepartment;
using ThirdParty.Json.LitJson;

namespace ManageNew.Controllers.TalentManagement
{

    /// <summary>
    /// 待删除简历
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "人才管理")]
    public class ResumeListController : ControllerBase
    {
        //private readonly CheckPermission _checkPermission;
        private readonly ICheckComResume _checkComResume;
        private readonly IExtranetResumeCheck _extranetResumeCheck;
        private readonly INewResumeService _newResumeService;
        private readonly ReadResume _readResume;
        private readonly IResumeShareVice _resumeShareVice;
        /// <summary>
        /// 构造方法
        /// </summary>
        public ResumeListController(ICheckComResume checkComResume, IExtranetResumeCheck extranetResume, INewResumeService newResumeService, ReadResume readResume, IResumeShareVice resumeShare)
        {
            _checkComResume = checkComResume;
            _extranetResumeCheck = extranetResume;
            _newResumeService = newResumeService;
            _readResume = readResume;
            _resumeShareVice = resumeShare;
        }

        /// <summary>
        /// 待删除简历,ResumeList.aspx 获取数据 人才搜索，同一个页面，待删除简历传参数 ResumeStatus=3
        /// </summary>
        /// <param name="siteType">站内1，站外2</param>
        /// <param name="keyWordType">搜索关键字类型</param>
        /// <param name="provinceId">省Id</param>
        /// <param name="cityId">市Id</param>
        /// <param name="townId">区或者镇街Id</param>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="orderByType">升序降序，默认降序</param>
        /// <param name="orderField">排序字段，应由后端处理，这里不安全</param>
        /// <param name="keyWord">关键字</param>
        /// <param name="registerFrom">来源</param>
        /// <param name="resumeStatus">简历状态</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetData(int keyWordType, int provinceId, int cityId, int townId,
            DateTime? beginDate, DateTime? endDate, string? orderByType, string orderField = "UpdateDate",
            string? keyWord = "", int siteType = 1, string? registerFrom = "", int resumeStatus = 0)
        {
            string filter = string.Empty;
            if (resumeStatus == 3)
                filter = " and My_Users.ResumeStatus = 3 ";
            
            if (!string.IsNullOrEmpty(keyWord))
            {
                if (!Enum.IsDefined(typeof(ResumeKeywordTypes), keyWordType))
                    return NotFound("参数keyWordType错误");
                ResumeKeywordTypes k = (ResumeKeywordTypes)keyWordType;
                switch (k)
                {
                    case ResumeKeywordTypes.PerName:
                        filter += " and My_Resume.PerName='" + keyWord + "'";
                        break;
                    case ResumeKeywordTypes.Email:
                        filter += " and My_Users.Email='" + keyWord + "'";
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
                            filter += " and My_Resume.MyUserId=" + id;
                        }
                        catch (Exception)
                        {
                            return BadRequest("选择编号时，请输入纯数字");
                        }
                        break;
                }
            }
            if (siteType != 1)
                filter += " and My_Resume.myuserid >100000000 ";
            else
                filter += " and My_Resume.myuserid <100000000 ";

            if (cityId != 0)
                filter += " and My_Resume.Location_C=" + cityId;
            else
            {
                if (provinceId != 0)
                    filter += " and My_Resume.Location_P =" + provinceId;
            }

            if (beginDate != null && endDate != null)
                filter += " and My_Resume.UpdateDate between '" + beginDate + "' and '" +
                          endDate.Value.AddDays(1).AddSeconds(-1) + "'";
            else if (beginDate != null)
                filter += " and My_Resume.UpdateDate >= '" + beginDate + "'";
            else if (endDate != null)
                filter += " and My_Resume.UpdateDate >= '" + endDate.Value.AddDays(1).AddSeconds(-1) + "'";
            //if (checkType==3)
            //filter += " and My_Resume.CheckFlag=3";


            int.TryParse(registerFrom, out int form);
            if (form != 0)
                filter += " and My_Users.RegisterFrom=" + form;

            string order = "asc";
            if (orderByType != null && (orderByType == "DESC" || orderByType == "desc"))
                order = orderByType;
            if (!string.IsNullOrEmpty(orderField))
            {
                switch (orderField)
                {
                    case "ResumeID":
                        filter += $" order by My_Resume.MyUserID {order}";
                        break;
                    case "Location":
                        filter += $" order by My_Resume.Location_P {order},My_Resume.Location_C {order}";
                        break;
                    case "RegisterDate":
                        filter += $" order by My_Resume.RegisterDate {order}";
                        break;
                    case "UpdateDate":
                        filter += $" order by My_Resume.UpdateDate {order}";
                        break;
                    case "CheckFlag":
                        filter += $" order by My_Resume.CheckFlag {order}";
                        break;
                }
            }


            var s = await _checkComResume.GetNewResumeListCollection(filter, orderField, order);
            return Ok(ResultMode<object>.Success(s));
        }


        /// <summary>
        /// 获取未加密和加密的账号密码,返回字符串
        /// </summary>
        /// <param name="myUserId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetUserPass(int myUserId)
        {
            //权限在过滤器做了
            //var userId = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            //var result =
            //    await _checkPermission.CheckPersonViewPermission(PersonViewEnum.ViewPersonUsername,
            //        Convert.ToInt32(userId));
            //if (!result)
            //    return Forbid();

            var dictionary = await _extranetResumeCheck.GetUserPass(myUserId);
            string key = string.Empty;
            string value = string.Empty;
            if (dictionary.Key != null)
            {
                key = dictionary.Key.ToString();
                dictionary.Key = EnAndDecryption.DESURL(dictionary.Key.ToString());
            }

            if (dictionary.Value != null)
            {
                value = dictionary.Value.ToString();
                dictionary.Value = EnAndDecryption.DESURL(dictionary.Value.ToString());
            }
            if (key == "" || value == "")
                return NotFound(ResultMode<string>.Failed("没找着"));
            return Ok(ResultMode<string>.Success($"{key},{value},{dictionary.Key},{dictionary.Value}"));
        }
        /// <summary>
        /// 获取未加密和加密的账号密码,返回json
        /// </summary>
        /// <param name="myUserId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetUserPassN(int myUserId)
        {
            //var userId = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            //var result =
            //    await _checkPermission.CheckPersonViewPermission(PersonViewEnum.ViewPersonUsername,
            //        Convert.ToInt32(userId));
            //if (!result)
            //    return Forbid();

            var dictionary = await _extranetResumeCheck.GetUserPass(myUserId);
            string key = string.Empty;
            string value = string.Empty;
            if (dictionary.Key != null)
            {
                key = dictionary.Key.ToString();
                dictionary.Key = EnAndDecryption.DESURL(dictionary.Key.ToString());
            }

            if (dictionary.Value != null)
            {
                value = dictionary.Value.ToString();
                dictionary.Value = EnAndDecryption.DESURL(dictionary.Value.ToString());
            }
            if (key == "" || value == "")
                return NotFound(ResultMode<string>.Failed("没找着"));
            return Ok(ResultMode<object>.Success(new
            {
                userName = key,
                passWord = value,
                userNames = dictionary.Key,
                passWords = dictionary.Value
            }));
        }




        #region 发送短信
        /// <summary>
        /// SentSMSNew.aspx,发送短信前，获取用户信息,返回字符串 姓名,手机号,发送状态，短信内容
        /// 待优化，返回json
        /// </summary>
        /// <param name="myUserId">简历Id </param>
        /// <param name="type">返回短信内容类型，传1或者不传</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetInfo(int myUserId, int type)
        {
            //var userId = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            //var result2 =
            //    await _checkPermission.CheckPersonViewPermission(PersonViewEnum.ViewPersonUsername,
            //        Convert.ToInt32(userId));
            //if (!result2)
            //    return Forbid();

            var result = await _newResumeService.GetMySMSInfo(myUserId);
            if (result == null)
                return NotFound($"简历{myUserId}不存在");
            string userName = !string.IsNullOrEmpty(result.PerName) ? result.PerName : result.UserName;
            string content = string.Empty;
            if (type == 1)
            {
                var entry = await _extranetResumeCheck.GetUserPass(myUserId);
                if (entry.Key != null && entry.Value != null)
                {
                    string eu = "m.goodjob.cn";
                    string result1 = string.Empty;
                    try
                    {
                        string url = "http://apis.baidu.com/chazhao/shorturl/shorturl";
                        string a = Goodjob.Encryption.Encryption.EncryptDES(entry.Key.ToString());
                        string v = Goodjob.Encryption.Encryption.EncryptDES(entry.Value.ToString());
                        string d = Goodjob.Encryption.Encryption.EncryptDES(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        string access_token = "2.008EA_uBCRzfdCbd1f1487bcUYpJsD";
                        a = Uri.EscapeDataString(a);
                        v = Uri.EscapeDataString(v);
                        d = Uri.EscapeDataString(d);
                        string uuus = "http://m.goodjob.cn/MsmLog.aspx?Type=3&a=" + a + "&v=" + v + "&d=";
                        StringBuilder sb = new StringBuilder("{");
                        sb.Append("\"type\":1,");
                        sb.Append("\"url\":[\"" + uuus + "\"]}");
                        string es = Uri.EscapeDataString(uuus);
                        string xinlangurl = "https://api.weibo.com/2/short_url/shorten.json?url_long=" + es + "&access_token=" + access_token;
                        result1 = xinlangrequest(xinlangurl);
                        JsonData jsonData = JsonMapper.ToObject(result1);
                        if (jsonData["urls"] != null)
                        {
                            JsonData jdItems = jsonData["urls"];
                            foreach (JsonData item in jdItems)
                            {
                                urls list = JsonMapper.ToObject<urls>(item.ToJson());
                                eu = list.url_short.Replace("http://", "");
                            }
                        }
                    }
                    catch (Exception e)
                    {
                    }
                    content += userName + "，您好！现有企业想了解您的求职意向，请点击 " + eu + " 来完善您的求职信息!";
                }
            }
            else
            {
                content += "找工作？俊才招聘网(goodjob.cn)帮您，您的帐号:" + result.UserName + ",密码:" + result.Password + ",手机登录m.goodjob.cn或下载安卓客户端http://i.goodjob.cn,好工作,让您随时随地身边找!";
            }
            bool yn = await _newResumeService.GetSendingLog(myUserId);
            string msg = yn ? "已发送" : "未发送";

            return Ok(ResultMode<string>.Success($"{userName}-{result.MobileNum}-{content}-{msg}"));

        }


        #endregion

        /// <summary>
        /// 打印简历 PrintContent.aspx 获取数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetDataForPrint(int myUserId, bool isChinese, bool isEnglish, bool isRemark)
        {

            var resume = await _resumeShareVice.GetResume(myUserId);
            if (resume == null)
                return NotFound(ResultMode<string>.Failed($"简历{myUserId}不存在"));
            string result = string.Empty;
            if (isChinese)
            {
                result = Regex.Replace(await _readResume.GetResumeTextForChecking(myUserId),
                    "<!--ResumeButton Begin-->(.|\n)*<!--ResumeButton End-->", string.Empty);
                if (!isRemark)
                {
                    result = Regex.Replace(result, "<!--Contact Begin-->(.|\n)*<!--Contact End-->", string.Empty);
                }
            }

            if (isEnglish && resume.EngResumeFlag == 1)
            {
                Regex.Replace(await _readResume.GetResumeTextEn(myUserId), "<!--ResumeButton Begin-->(.|\n)*<!--ResumeButton End-->", string.Empty);
                if (!isRemark)
                {
                    result = Regex.Replace(result, "<!--Contact Begin-->(.|\n)*<!--Contact End-->", string.Empty);
                }
            }
            return Ok(ResultMode<string>.Success(result));
        }


        /// <summary>
        /// 新浪发送HTTP请求
        /// </summary>
        /// <param name="url">请求的URL</param>
        /// <param name="param">请求的参数</param>
        /// <returns>请求结果</returns>
        private static string xinlangrequest(string url)
        {
            string strURL = url;
            System.Net.HttpWebRequest request;
            request = (System.Net.HttpWebRequest)WebRequest.Create(strURL);
            request.Method = "GET";
            System.Net.HttpWebResponse response;
            response = (System.Net.HttpWebResponse)request.GetResponse();
            System.IO.Stream s;
            s = response.GetResponseStream();
            string StrDate = "";
            string strValue = "";
            StreamReader Reader = new StreamReader(s, Encoding.UTF8);
            while ((StrDate = Reader.ReadLine()) != null)
            {
                strValue += StrDate;
            }
            return strValue;
        }
        private class urls
        {
            public string Object_type;
            public bool Result;
            public string Url_short;
            public string Object_id;
            public string Url_long;
            public int Type;

            public string object_id
            {
                get
                {
                    return Object_id;
                }
                set
                {
                    this.Object_id = value;
                }
            }
            public int type
            {
                get
                {
                    return Type;
                }

                set
                {
                    this.Type = value;
                }
            }

            public bool result
            {
                get
                {
                    return Result;
                }

                set
                {
                    this.Result = value;
                }
            }

            public string object_type
            {
                get
                {
                    return Object_type;
                }

                set
                {
                    this.Object_type = value;
                }
            }

            public string url_short
            {
                get
                {
                    return Url_short;
                }

                set
                {
                    this.Url_short = value;
                }
            }

            public string url_long
            {
                get
                {
                    return Url_long;
                }

                set
                {
                    this.Url_long = value;
                }
            }
        }
    }
}
