using Microsoft.AspNetCore.Mvc;
using Model;
using IService.TalentManagement;
using Model.Common;

namespace ManageNew.Controllers.TalentManagement
{
    /// <summary>
    /// 已短信通知的用户
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "人才管理")]
    public class SentHistroyController : ControllerBase
    {
        
        private readonly ISentHistroy _sentHistroy;
        /// <summary>
        /// 构造
        /// </summary>
        public SentHistroyController(ISentHistroy sent)
        {
            _sentHistroy = sent;
        }


        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="p"></param>
        /// <param name="myUserId">简历Id</param>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="orderType">排序类型，升序或降序</param>
        /// <param name="perName">姓名</param>
        /// <param name="mobileNum">手机号</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetData([FromQuery] PagingModel p, int myUserId,
            DateTime? beginDate, DateTime? endDate, string? orderType="desc", string? perName = "", string? mobileNum = "")
        {
            string filter=string.Empty;
            if (myUserId != 0)
            {
                filter += " and [Goodjob].[dbo].[My_SMS].[MyUserID]= " + myUserId;
            }
            if (!string.IsNullOrEmpty(perName))
            {
                filter += " and [Goodjob].[dbo].[My_Resume].[PerName] like '%" + perName + "%' ";
            }
            if (!string.IsNullOrEmpty(mobileNum))
            {
                filter += " and [Goodjob].[dbo].[My_Resume].[MobileNum]=  '" + mobileNum + "' ";
            }
            if(beginDate != null)
                filter += " and [Goodjob].[dbo].[My_SMS].[SentTime]>= Convert(DateTime,'" + beginDate + "') ";
            if (endDate != null)
                filter += " and [Goodjob].[dbo].[My_SMS].[SentTime]<= Convert(DateTime,'" + endDate + "') ";
            if (string.IsNullOrEmpty(orderType))
                orderType = "desc";
            var s = await _sentHistroy.GetSentSMSUsersList(p, filter, orderType);
           return Ok(ResultMode<object>.Success(new { Count = s.count, Data = s.data }));
            
        }
    }
}
