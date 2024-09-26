using Commons.Tool;
using IService.ResumeSearch;
using IService.TalentManagement;
using Microsoft.AspNetCore.Mvc;
using Model;
using Model.TalentManagement;

namespace ManageNew.Controllers.TalentManagement
{
    /// <summary>
    /// 简历完整度查询 ResumeComplete.aspx
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "人才管理")]
    public class ResumeCompleteController : ControllerBase
    {
        private readonly ICheckComResume _checkComResume;
        private readonly IExtranetResumeCheck _check;
        private readonly IResumeComplete _complete;
        /// <summary>
        /// 构造方法
        /// </summary>
        public ResumeCompleteController(ICheckComResume checkComResume, IExtranetResumeCheck check, IResumeComplete complete)
        {
            _checkComResume = checkComResume;
            _check = check;
            _complete = complete;
        }

        /// <summary>
        /// ResumeComplete.aspx 获取数据
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
        /// <param name="fullType"></param>
        /// <param name="baseWrite">基本资料状态</param>
        /// <param name="eduWrite">教育经历状态</param>
        /// <param name="workWrite">工作经历状态</param>
        /// <param name="hopeWrite">求职意向状态</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetData(int keyWordType, int provinceId, int cityId, int townId, DateTime? beginDate, DateTime? endDate, string? orderByType, string orderField = "UpdateDate", string? keyWord = "", int siteType = 1, string? registerFrom = "",int fullType=0, int baseWrite = 1, int eduWrite = -1, int workWrite = -1, int hopeWrite = 0)
        {
            string filter = " and  My_Resume.MyUserID not in ( select mc.MyUserID from  Goodjob.dbo.Mng_CompleteQd as mc) and   DateDiff(DAY,UpdateDate,getDate()) > 1 ";
            if (!string.IsNullOrEmpty(registerFrom))
            {
                filter += " and My_Users.RegisterFrom = " + registerFrom;
            }
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
            if (fullType == 2)
            {
                filter += "  AND (My_Resume.AllWrite = 0) AND (My_Resume.CheckFlag = 0) AND (BaseWrite = 1) AND (HopeWrite = 1) AND (EduWrite = 1) ";
            }
            else
            {
                filter += " AND My_Resume.AllWrite = 0 AND ((My_Resume.CheckFlag = 0) OR (My_Resume.CheckFlag = 3)) ";
                if (baseWrite!=-1)
                    filter += "  AND BaseWrite= " + baseWrite;
                if (eduWrite != -1)
                    filter += "  AND EduWrite= " + eduWrite;
                if (workWrite != -1)
                    filter += "  AND WorkWrite= " + workWrite;
                if (hopeWrite != -1)
                    filter += "  AND HopeWrite= " + hopeWrite;
            }
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
            foreach (var item in s)
            {
                var dictionary = await _check.GetUserPass(item.MyUserID);
                if (dictionary.Key != null && dictionary.Value != null)
                {
                    item.MyLoginString = "http://my.goodjob.cn/IndexLogin/?user=" + EnAndDecryption.DESURL(dictionary.Key.ToString()) + "&pass=" + EnAndDecryption.DESURL(dictionary.Value.ToString()) + "&LoginType=1";
                }
            }
            return Ok(ResultMode<object>.Success(s));
        }
        /// <summary>
        /// 设置去电
        /// </summary>
        /// <param name="myUserId">简历Id</param>
        /// <param name="describe">描述</param>
        /// <param name="qdType">去电类型</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> SetQd(int myUserId,string describe,int qdType)
        {
            var yn = await _complete.InsertCompleteQd(myUserId, describe, qdType);
            if (yn)
                return Ok(ResultMode<bool>.Success(true, "设置成功"));
            return Ok(ResultMode<string>.Failed("设置失败"));
        }
    }
}
