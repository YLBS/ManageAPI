using IService.SalesDepartment;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace ManageNew.Controllers.SalesDepartment
{
    /// <summary>
    /// 职位发布效果新，旧的没写
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "销售部专用")]
    public class JobEffectController : ControllerBase
    {
        private readonly IJobEffect _effect;
        /// <summary>
        /// 构造方法
        /// </summary>
        public JobEffectController(IJobEffect effect)
        {
            _effect= effect;
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetData(string filter,string order= "TotalApply")
        {
            var s = await _effect.GetJobEffectCollection(filter, order);
            return Ok(ResultMode<object>.Success(new { Count = s.count, Data = s.item }));
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="memId">企业ID</param>
        /// <param name="memName">企业名称</param>
        /// <param name="memberClass">会员类型</param>
        /// <param name="CooperativeId">合作代表</param>
        /// <param name="salerId">服务代表</param>
        /// <param name="posState">职位状态</param>
        /// <param name="refreshRange">刷新时间</param>
        /// <param name="applyNum">应聘人数</param>
        /// <param name="deadlineRange">截止日期</param>
        /// <param name="ddlHits">查看次数</param>
        /// <param name="order">排序字段</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetData_N(int memId,string memName,int memberClass,int CooperativeId,int salerId,int posState,int refreshRange,int applyNum,int deadlineRange,int ddlHits, string order = "TotalApply")
        {


            return Ok(123);
            //var s = await _effect.GetJobEffectCollection(filter, order);
            //return Ok(ResultMode<object>.Success(new { Count = s.count, Data = s.item }));
        }

        /// <summary>
        /// 旧的职位发布效果，获取合作代表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> CooperativeList()
        {
            string userIdStr = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            int.TryParse(userIdStr, out int id);
            var kv = await _effect.CooperativeList(id, 1);
            return Ok(ResultMode<object>.Success(kv));
        }

        /// <summary>
        /// 企业发布效果新 获取数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> NewGetJobEffectCollection(string filter,string order)
        {
            var list = await _effect.NewGetJobEffectCollection(filter, order);
            return Ok(ResultMode<object>.Success(new { Data = list.item, Count = list.count }));
        }
        /// <summary>
        /// 企业发布效果新 获取数据,新传参方法
        /// </summary>
        /// <param name="salerId">服务代表</param>
        /// <param name="memClass">会员类型</param>
        /// <param name="day">成交时间</param>
        /// <param name="applyNum">应聘人数</param>
        /// <param name="orderF">第一个排序方式</param>
        /// <param name="orderTypeF">升序降序</param>
        /// <param name="orderS">第二个排序方式</param>
        /// <param name="orderTypeS"></param>
        /// <param name="orderT">第三个排序方式</param>
        /// <param name="orderTypeT"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> NewGetJobEffectCollectionN(int salerId,int memClass,int day,int applyNum, string orderF,string orderTypeF, string orderS, string orderTypeS, string orderT, string orderTypeT)
        {
            string filter = " AND PosState = 2 ";
            if (memClass != -1)
            {
                filter += "  AND (P.MemID IN (SELECT MemID FROM Mem_Users WHERE (SalerUserID = " + salerId + ") AND MemberClass = " + memClass + ")) ";
            }
            else
            {
                filter += "  AND (P.MemID IN (SELECT MemID FROM Mem_Users WHERE (SalerUserID = " + salerId + ") AND MemberClass in(1,2,3))) ";
            }

            if (day > 0)
            {
                filter +=
                    "  and MemID in ( select mu.MemID from Goodjob.dbo.Mem_Users as mu where  DATEDIFF(DD,mu.BeginValidDate,GETDATE())  < " +
                    day + ") ";
            }
            filter += "   and ISNULL(TotalApply,0) <= "+ applyNum;

            string orderSql = " order by " + orderF + " " + orderTypeF;
            if (orderS != orderF)
            {
                orderSql+= " ," + orderS + " " + orderTypeS;
            }
            if (orderT != orderF && orderT != orderS)
            {
                orderSql += " ," + orderT + " " + orderTypeT;
            }
            var list = await _effect.NewGetJobEffectCollection(filter, orderSql);
            return Ok(ResultMode<object>.Success(new { Data = list.item, Count = list.count }));
        }
    }
}
