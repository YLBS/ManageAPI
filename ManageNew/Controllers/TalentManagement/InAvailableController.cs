using IService.TalentManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using Newtonsoft.Json.Linq;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace ManageNew.Controllers.TalentManagement
{
    /// <summary>
    /// MobileInavailable.aspx 简历手机号码无效记录
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "人才管理")]
    public class InAvailableController : ControllerBase
    {
        private readonly IInAvailable _available;
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="available"></param>
        public InAvailableController(IInAvailable available)
        {
            _available = available;
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="keyType">搜索类型</param>
        /// <param name="keyWord">搜索关键字</param>
        /// <param name="deal">是否已处理，0不限，1 未处理，2 已处理</param>
        /// <param name="beginDate">更新时间</param>
        /// <param name="endDate">更新时间</param>
        /// <param name="orderField">排序</param>
        /// <param name="orderType">排序类型</param>
        /// <param name="hasUpdate">是否已变更号码，0 不限，1 未变更，2 已变更</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetData(int keyType, string? keyWord, int deal, DateTime? beginDate,
            DateTime? endDate, int hasUpdate, int orderField, string orderType = "desc")
        {
            string filter=string.Empty;
            string order=string.Empty;
            if (!string.IsNullOrEmpty(keyWord))
            {
                switch (keyType)
                {
                    case 1:
                        try
                        {
                            int id =  int.Parse(keyWord);
                            filter = " AND [My_PhoneDisabled].[MyUserID] = " + id + " ";
                        }
                        catch (Exception e)
                        {
                            return BadRequest("选择编号时，请输入纯数字");
                        }
                       
                        break;
                    case 2:
                        filter = " AND DisabledPhone link '%" + keyWord + "%' ";
                        break;
                    case 3:
                        try
                        {
                            int id = int.Parse(keyWord);
                            filter = " AND [My_PhoneDisabled].[MemID] = " + id + " ";
                        }
                        catch (Exception e)
                        {
                            return BadRequest("选择编号时，请输入纯数字");
                        }
                       
                        break;
                    case 4:
                        filter = " AND MemName like '%" + keyWord + "%' ";
                        break;
                    case 5:
                        filter = " AND PerName like '%" + keyWord + "%' ";
                        break;
                }
            }

            switch (deal)
            {
                case 1:
                    filter += " AND HasDealWith = 0  ";
                    break;
                case 2:
                    filter += " AND HasDealWith = 1 ";
                    break;
            }
            switch (deal)
            {
                case 1:
                    filter += " AND [DisabledPhone]=[My_Resume].[MobileNum] ";
                    break;
                case 2:
                    filter += " AND [DisabledPhone]<>[My_Resume].[MobileNum] ";
                    break;
            }
            if(beginDate!=null)
                filter += "  AND SetDate > CONVERT(DATETIME,'" + beginDate + "') ";
            if (endDate != null)
                filter += " AND SetDate < DATEADD(DAY, 1,CONVERT(DATETIME, '" + endDate + "')) ";

            orderType = orderType.ToLower();
            if (orderType != "asc") //避免前端乱传参数报错
                orderType = "desc";

            switch (orderField)
            {
                case 0:
                    order = " ORDER BY [My_PhoneDisabled].[ID] " + orderType + " ";
                    break;
                case 1:
                    order = " ORDER BY [My_PhoneDisabled].[MemID] " + orderType + " ";
                    break;
                case 2:
                    order = "ORDER BY SetDate " + orderType + " ";
                    break;
            }
            var result = await _available.GetData(filter, order);
            return Ok(ResultMode<object>.Success(new { Count = result.Count(), Data = result }));
        }
        /// <summary>
        /// 更新处理状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> UpdateDealStatue(int id,string content)
        {
            var s = await _available.UpdateDealStatue(id, content);
            if (s>0)
                return Ok(ResultMode<string>.Success("","操作成功"));
            return Ok(ResultMode<string>.Failed("操作失败"));
        }
    }
}
