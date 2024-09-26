using IService.TalentManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using Model.TalentManagement;

namespace ManageNew.Controllers.TalentManagement
{
    /// <summary>
    /// 审核不通过完整简历，CheckComResumeList.aspx
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "人才管理")]
    public class CheckComResumeController : ControllerBase
    {
        private readonly ICheckComResume _checkComResume;
        /// <summary>
        /// 构造方法
        /// </summary>
        public CheckComResumeController(ICheckComResume checkComResume)
        {
            _checkComResume = checkComResume;
        }

        /// <summary>
        /// 审核不通过完整简历,CheckComResumeList.aspx 获取数据
        /// </summary>
        /// <param name="siteType">站内1，站外2</param>
        /// <param name="checkType">简历状态，3</param>
        /// <param name="keyWordType">搜索关键字类型</param>
        /// <param name="provinceId">省Id</param>
        /// <param name="cityId">市Id</param>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="orderByType">升序降序，默认降序</param>
        /// <param name="orderField">排序字段，应由后端处理，这里不安全</param>
        /// <param name="keyWord">关键字</param>
        /// <param name="allWrite">站内1，站外2</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetResumeList(int checkType, int keyWordType, int provinceId, int cityId,
            DateTime? beginDate, DateTime? endDate, string? orderByType, string orderField = "UpdateDate",
            string? keyWord = "", int siteType = 1,int allWrite=1)
        {
            //checkType = 3;
            string filter = string.Empty;
            if (allWrite == 1)
                filter += " AND (My_Resume.AllWrite = 1) ";
            else
                filter += " AND (My_Resume.AllWrite = 0) ";
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
             filter += " and My_Resume.CheckFlag=3";
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
    }
}
