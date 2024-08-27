using System.Collections;
using IService.SalesDepartment;
using Microsoft.AspNetCore.Mvc;
using Model;
using Model.SalesDepartment;

namespace ManageNew.Controllers.SalesDepartment
{
    /// <summary>
    /// 公共库企业
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "销售部专用")]
    public class PulicCompanyController : ControllerBase
    {
        private readonly IPulicCompany _company;
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="company"></param>
        public PulicCompanyController(IPulicCompany company)
        {
            _company = company;
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetData([FromQuery]SearchPulicCompany searchKey)
        {
            var result = await _company.GetSalesCompanyListInfo(searchKey);
            return Ok(ResultMode<Object>.Success(result));
        }
        /// <summary>
        /// 招聘数据,获取企业信息
        /// </summary>
        /// <param name="memId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetCompanyInfo(int memId)
        {
            var result = await _company.GetCompanyInfo(memId);
            string memName = "";
            if (result.Count() > 0)
            {
                memName = result.First().MemName;
            }
            return Ok(ResultMode<Object>.Success(new { Data = result, memName }));
        }
        /// <summary>
        /// 招聘数据,获取企业招聘效果
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetCompanyAd(int memId)
        {
            var result = await _company.GetCompanyAd(memId);
            return Ok(ResultMode<Object>.Success(result));
        }

    }
}
