using IService.SalesDepartment;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace ManageNew.Controllers.SalesDepartment
{
    /// <summary>
    /// 我的服务申请订单
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "销售部专用")]
    public class MyServiceOrderController : ControllerBase
    {
        private readonly IMyServiceOrder _myServiceOrder;

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="myServiceOrder"></param>
        public MyServiceOrderController(IMyServiceOrder myServiceOrder)
        {
            _myServiceOrder = myServiceOrder;
        }
        /// <summary>
        ///   获取数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            var result = await _myServiceOrder.GetCompanyServiceDateCollection(userId);
            return Ok(ResultMode<object>.Success(result));
        }
    }
}
