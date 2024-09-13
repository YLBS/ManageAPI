using IService.SalesDepartment;
using ManageNew.Tool;
using Microsoft.AspNetCore.Mvc;
using Model;
using Model.Common;

namespace ManageNew.Controllers.SalesDepartment
{
    /// <summary>
    /// 职位投诉列表
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "销售部专用")]
    public class UserComplaintController : ControllerBase
    {
        private readonly IUserComplaint _iUserComplaint;
        private readonly ManageUserCache _userCache;

        /// <summary>
        /// 构造方法
        /// </summary>
        public UserComplaintController(IUserComplaint userComplaint, ManageUserCache cache)
        {
            _iUserComplaint = userComplaint;
            _userCache = cache;
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="salerId">服务代表</param>
        /// <param name="radDealWith">处理状态</param>
        /// <param name="totalCount">被投诉次数大于多少</param>
        /// <param name="radType">处理类别</param>
        /// <param name="top">获取前多少条数据</param>
        /// <param name="order">按哪个字段排序，</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetData(int radDealWith, int totalCount, int radType, int top = 500, string? order="",string? salerId="")
        {
            string filter = string.Empty;
            if (string.IsNullOrEmpty(salerId))
            {
                var account = User.Claims.FirstOrDefault(c => c.Type == "account")?.Value;
                var ss = await _userCache.GetDeptUsers(0, account);
                var ids = string.Join(",", ss.Select(m=>m.ClerkID));
                filter += "  AND SalerUserID  IN (    " + ids + ") ";
            }
            else
            {
                int.TryParse(salerId, out int id);
                filter += "  and SalerUserID =  " + id;
            }
            if(radDealWith!=-1)
                filter += "  AND HasDealWith =  " + radDealWith;
            if (radType != 0)
                filter += "  AND Type =  " + radType;
            var result = await _iUserComplaint.GetUserComplaintListNews(filter, order, top, totalCount);
            return Ok(ResultMode<object>.Success(result.item));
        }
        /// <summary>
        /// 通过职位ID获取投诉信息列表
        /// </summary>
        /// <param name="posId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetUserComplaintListByPosId(int posId)
        {
            var result = await _iUserComplaint.GetUserComplaintListByPosId(posId);
            return Ok(ResultMode<object>.Success(new{Data= result ,Count= result.Count()}));
        }
        /// <summary>
        /// 通过职位ID获取投诉信息，返回的data是判断投诉是否有处理，false 是控件不可见，为已处理
        /// </summary>
        /// <param name="posId"></param>
        /// <param name="sqlType"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetUserComplaintByPosId(int posId, int sqlType)
        {
            var result = await _iUserComplaint.GetUserComplaintByPosId(posId, sqlType);
            return Ok(ResultMode<bool>.Success(result.tf, result.msg));
        }
        /// <summary>
        /// 更改投诉信息处理状况
        /// </summary>
        /// <param name="posId"></param>
        /// <param name="processingDetail"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> DealWithUserComplaint(int posId, string processingDetail)
        {
            var i = await _iUserComplaint.DealWithUserComplaint(posId, processingDetail);
            return Ok(ResultMode<string>.Success($"成功修改了{i}条数据"));
        }
        /// <summary>
        /// ExplosiveComEffect.aspx 获取爆款企业效果
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetExplosiveCompanies([FromQuery]PagingModel paging,  int memId,int memClass,int loginDay,int endTimeDay,int applyNum,int simulate,  string? memName)
        {
            string filter=string.Empty;
            if (memId != 0)
                filter += " and mi.memId=" + memId;
            if (!string.IsNullOrEmpty(memName))
                filter += " and mi.memName like '%" + memName + "'%";
            if (loginDay != -1) 
                filter = "and mu.LastLoginDate >= '" + DateTime.Now.AddDays(-loginDay).Date + "' ";
            if (endTimeDay != -1)
                filter += " and DateDiff(DD, mu.EndValidDate,getdate())  <= " + endTimeDay;//+ " and DateDiff(DD, mu.EndValidDate,getdate()) >0 ";
            if (memClass != -2)
                filter += " and  mrr.MemState=" + memClass ;
            var s = await _iUserComplaint.GetExplosiveCompanies(filter);
            if (applyNum != -1 && applyNum != 0)
            {
                s = s.Where(w => w.ApplyCount <= applyNum).ToList();
            }
            if (simulate != -1 && simulate != 0) 
            {
                s = s.Where(w => w.SimulateCount <= simulate).ToList();
            }
            return Ok(ResultMode<object>.Success(s));
        }
    }

}
