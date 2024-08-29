using IService.SalesDepartment;
using Microsoft.AspNetCore.Mvc;
using Model;
using Model.SalesDepartment;

namespace ManageNew.Controllers.SalesDepartment
{
    /// <summary>
    /// 留言列表
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "销售部专用")]
    public class MessageListController : ControllerBase
    {
        private readonly IMessageLis _messageLis;
        /// <summary>
        /// 构造方法
        /// </summary>
        public MessageListController(IMessageLis messageLis)
        {
            _messageLis = messageLis;
        }
        /// <summary>
        /// 留言信息列表,获取数据,只能查自己发送的？
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetData([FromQuery] SearchMessageLis searchMessageLis)
        {
            searchMessageLis.SalerID = " and {0}=" + User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            var result = await _messageLis.GetData(searchMessageLis);
            return Ok(ResultMode<object>.Success(result));
        }
        /// <summary>
        /// 公共企业留言
        /// </summary>
        /// <param name="searchMessageLis"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetPublicMessage([FromQuery] SearchMessageLis searchMessageLis)
        {
            //searchMessageLis.SalerID = " and {0}=" + User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            var result = await _messageLis.GetData(searchMessageLis);
            return Ok(ResultMode<object>.Success(result));
        }
        /// <summary>
        /// 删除留言信息
        /// </summary>
        /// <param name="infoId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> DeleteDeleteMessage(int infoId)
        {
            string userIdStr = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            int.TryParse(userIdStr, out int id);
            var result = await _messageLis.DeleteDeleteMessage(infoId,id);
            return Ok(ResultMode<bool>.Success(result));
        }
    }
}
