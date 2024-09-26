using IService.SalesDepartment;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace ManageNew.Controllers.SalesDepartment
{
    /// <summary>
    /// 下属企业
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "销售部专用")]
    public class DeptCompanyController : ControllerBase
    {
        private readonly IDeptCompany _dept;
        /// <summary>
        /// 构造方法
        /// </summary>
        public DeptCompanyController(IDeptCompany dept)
        {
            _dept= dept;

        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetData(string filter)
        {
            var result = await _dept.GetCompanyList(filter);
            return Ok(ResultMode<object>.Success(new { Data = result.item, count = result.totalRecords }));
        }
    }
}
