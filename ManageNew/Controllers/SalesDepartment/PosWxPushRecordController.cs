using IService.SalesDepartment;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace ManageNew.Controllers.SalesDepartment
{
    /// <summary>
    /// 职位微信推送记录
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "销售部专用")]
    public class PosWxPushRecordController : ControllerBase
    {
        private readonly IPosWxPushRecord _posWxPush;
        /// <summary>
        /// 构造方法
        /// </summary>
        public PosWxPushRecordController(IPosWxPushRecord posWxPush)
        {
            _posWxPush = posWxPush;
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetData(string filter, string? elpName)
        {
            var result=await _posWxPush.GetPusRecordInfosAsync(filter, elpName);
            return Ok(ResultMode<Object>.Success(result));
        }
    }
}
