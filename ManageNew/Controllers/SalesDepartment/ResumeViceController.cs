using IService.SalesDepartment;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using System.Collections.Generic;
using Model.Common;

namespace ManageNew.Controllers.SalesDepartment
{
    /// <summary>
    /// 我的简历库
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "销售部专用")]
    public class ResumeViceController : ControllerBase
    {
        private readonly IResumeVice _resumeVice;
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="resumeVice"></param>
        public ResumeViceController(IResumeVice resumeVice)
        {
            _resumeVice = resumeVice;
        }
        
        /// <summary>
        /// 获取数据,get请求居然出错了。。AND  ShareEpl_Id like '%568%'
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetData([FromBody] Singles filter)
        {
            var list = await _resumeVice.GetViceResume(filter.filter);
            return Ok(ResultMode<object>.Success(list));
        }
        /// <summary>
        /// 点击查看推荐岗位
        /// </summary>
        /// <param name="vid"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetVicePos(int vid)
        {
            var result =await _resumeVice.GetVicePos(vid);
            return Ok(ResultMode<object>.Success(result));

        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetData_N(string keyWord, int depId, int depUserId)
        {
            string userIdStr = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            int.TryParse(userIdStr, out int id);
            if (depId == 0 && depUserId == 0)
            {
                depUserId = id;
            }
            var list = await _resumeVice.GetData_N(keyWord,depId,depUserId);
            return Ok(ResultMode<object>.Success(list));
        }
        /// <summary>
        /// 添加推荐到企业，添加副简历
        /// </summary>
        /// <param name="memIdsPosIds">memIds 传的是 企业Id-岗位Id,企业Id-岗位Id,</param>
        /// <param name="viceIds">visId</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> AddMemLibCommendVice(string memIdsPosIds, string viceIds)
        {

            string userIdStr = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            int.TryParse(userIdStr, out int id);
            int i = await _resumeVice.AddMemLibCommendVice(memIdsPosIds, viceIds, id);
            return Ok(ResultMode<string>.Success($"受影响行{i}"));
        }
        /// <summary>
        /// 企业搜索查询
        /// </summary>
        /// <param name="salerUserId"></param>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetSalerCompanyByPosWord(string salerUserId, string keyWord)
        {
            var list = await _resumeVice.GetSalerCompanyByPosWord(salerUserId, keyWord);
            return Ok(ResultMode<object>.Success(list));
        }
    }
}
