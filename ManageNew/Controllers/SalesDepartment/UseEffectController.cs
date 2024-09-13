using IService.SalesDepartment;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using Model.Common;
using System.Data;
using System.Net.NetworkInformation;

namespace ManageNew.Controllers.SalesDepartment
{
    /// <summary>
    /// 企业使用效果
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "销售部专用")]
    public class UseEffectController : ControllerBase
    {
        private readonly IUseEffectService _effectService;
        public UseEffectController(IUseEffectService effectService)
        {
            _effectService = effectService;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="memName"></param>
        /// <param name="memId"></param>
        /// <param name="commendType"></param>
        /// <param name="paging"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetData([FromQuery] PagingModel paging,DateTime? beginDate, DateTime? endDate, string? memName, int memId, int commendType)
        {
            var r= await _effectService.GetPerApplyPosInfos(beginDate,endDate,memName,memId,commendType,paging);
            return Ok(ResultMode<object>.Success(r));
        }
        /// <summary>
        /// 获取不同应聘类型的总数
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="memName"></param>
        /// <param name="memId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetPerApplyPosInfos(DateTime? beginDate, DateTime? endDate, string? memName,
            int memId)
        {
            var s = await _effectService.GetPerApplyPosInfos(beginDate, endDate, memName, memId);
            string ss = string.Join(",",s);
            return Ok(ResultMode<string>.Success(ss));
        }
        /// <summary>
        /// 单个发布中岗位的企业,获取数据
        /// </summary>
        /// <param name="paging"></param>
        /// <param name="posId"></param>
        /// <param name="memId"></param>
        /// <param name="salerId"></param>
        /// <param name="posName"></param>
        /// <param name="memName"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetOneJobFunctionCollection([FromQuery] PagingModel paging,int posId,int memId,int salerId, string? posName,string? memName)
        {
            string filter=string.Empty;
            if (posId != 0)
                filter += " and PosID = " + posId;
            if (memId != 0)
                filter += " and memId = " + memId;
            if (!string.IsNullOrEmpty(posName))
                filter += " and posName like '% " + posName + "%' ";
            if (!string.IsNullOrEmpty(memName))
                filter += " and memName like '% " + memName + "%' ";
            if (salerId != 0)
                filter += " AND (MemID IN (SELECT MemID FROM Mem_Users WHERE SalerUserID = " + salerId + "))";
            var s = await _effectService.GetOneJobFunctionCollection(paging, filter);
            return Ok(ResultMode<object>.Success(new { Data = s.effect, Count = s.count }));
        }
    }
}
