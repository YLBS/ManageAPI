using IService.TalentManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace ManageNew.Controllers.TalentManagement
{
    /// <summary>
    /// 留言人才记录
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "人才管理")]
    public class MessageListController : ControllerBase
    {
        private readonly IMessageList _messageList;
        /// <summary>
        /// 构造方法
        /// </summary>
        public MessageListController(IMessageList messageList)
        {
            _messageList = messageList;
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetDatas(string? keyType="",string? read="", string? keyText = "",string? type="Search")
        {
            string filter = string.Empty;
            if (!string.IsNullOrEmpty(type))
            {
                if (!string.IsNullOrEmpty(read))
                    filter += " and [Read]=" + read;
                if (!string.IsNullOrEmpty(keyText))
                {
                    switch (keyType)
                    {
                        case "0":
                            try
                            {
                                int id= Int32.Parse(keyText);
                                filter += " and myUserId=" + id;
                            }
                            catch (Exception e)
                            {
                                return BadRequest("选择编号时，请输入纯数字");
                            }
                           
                            break;
                        case "1":
                            filter += " and myUserId in (select MyUserID from My_Resume where PerName  like '%" + keyText + "%')";
                            break;
                        case "2":
                            filter+= " and Subject like '%" + keyText + "%'";
                            break;
                    }
                }
            }

            var result = await _messageList.GetSalerMessCollection(filter);
            return Ok(ResultMode<object>.Success(result));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="infoId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Del(int infoId)
        {
            var yn = await _messageList.DeleteSalerMessage(infoId);
            if(yn) return Ok(ResultMode<string>.Success("","删除成功"));
            return Ok(ResultMode<string>.Failed("删除失败"));
        }
    }
}
