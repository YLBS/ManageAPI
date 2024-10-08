﻿using Commons.Tool;
using Entity.Goodjob;
using IService.SalesDepartment;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using Model.Common;
using System.Globalization;

namespace ManageNew.Controllers.SalesDepartment
{
    /// <summary>
    /// 职位列表
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "销售部专用")]
    public class PosController : ControllerBase
    {
        private readonly IPosService _posService;
        private readonly IConfiguration _configuration;
        /// <summary>
        /// 构造方法
        /// </summary>
        public PosController(IPosService posService, IConfiguration configuration)
        {
            _posService = posService;
            _configuration = configuration;
        }

        /// <summary>
        /// 获取数据
        /// </summary> 
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetData(int memId,int pageSize,int pageIndex)
        {
            var result = await _posService.GetData(memId, pageSize, pageIndex);
            return Ok(ResultMode<object>.Success(new{Data= result.item, Count= result.totalRecords, Name=result.memName }));
        }
        /// <summary>
        /// 刷新所有发布中的职位，根据企业ID查发布中的职位，再执行 RefreshSelect。
        /// </summary>
        /// <param name="memId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> RefreshAll(int memId)
        {
            string posName = await _posService.RefreshAll(memId);
            if (posName == "无发布中职位")
            {
                return Ok(ResultMode<string>.Failed(posName));
            }
            return Ok(ResultMode<string>.Success(posName));
        }
        /// <summary>
        /// 刷新所有选中的职位
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> RefreshSelect(string posIds)
        {
            var id=posIds.Split(',');
            var ids = Array.ConvertAll(id, int.Parse);
            string posName = await _posService.RefreshSelect(ids);
            return Ok(ResultMode<string>.Success(posName));
        }
        /// <summary>
        /// 生成微信分享文字/链接
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> CreateWxShareStr(string strPosId)
        {
            var id = strPosId.Split(',');
            var ids = Array.ConvertAll(id, int.Parse);
            var result = await _posService.CreateWxShareStr(ids);
            if (result.count == 0)
            {
                return Ok(ResultMode<string>.Failed(result.str));
            }
            return Ok(ResultMode<string>.Success(result.str));
        }
        /// <summary>
        /// 生成兼职微信分享文字/链接
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> CreatePtWxShareStr(int memId)
        {
            var result = await _posService.CreatePtWxShareStr(memId);
            if (result.count == 0)
            {
                return Ok(ResultMode<string>.Failed(result.str));
            }
            return Ok(ResultMode<string>.Success(result.str));
        }
        /// <summary>
        /// 获取还在排队推送中的总数
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetWxPushingCount()
        {
            var result = await _posService.Get_WxPushingCount("");
            return Ok(ResultMode<int>.Success(result));
        }
        /// <summary>
        /// 根据企业ID返回推送的总数，以及手动和自动推送的数量,以及判斷 微信发送 按钮是否可见
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetWxPusRecordCount(int memId)
        {
            string userIdStr = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            int.TryParse(userIdStr, out int id);
            var result = await _posService.GetWxPusRecordCount(memId,id);
            return Ok(ResultMode<object>.Success(new
            {
                result.count,result.syscount,result.sdcount,
                wxfsssVisible= result.tf
            }));
        }
        /// <summary>
        /// 判断是否推送
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> EstimatePush(int memId)
        {
            var result = await _posService.EstimatePush(memId);
            return Ok(ResultMode<object>.Success(result));
        }
        /// <summary>
        /// 推送取反，如当企业是禁止推送，调用该API后启用推送
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> PushNegation(int memId)
        {
            var result = await _posService.PushNegation(memId);
            return Ok(ResultMode<object>.Success(result));
        }
        /// <summary>
        /// 刷新发布中的兼职职位
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> RefreshPtAll(int memId)
        {
            var str = await _posService.RefreshPtAll(memId);
            if (str == "刷新成功")
            {
                return Ok(ResultMode<string>.Success(str));
            }
            return Ok(ResultMode<string>.Failed(str));
        }
        /// <summary>
        /// 模拟选中的职位（第二天执行）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> SimulationSelect(string posIds, int memId)
        {
            var id = posIds.Split(',');
            var ids = Array.ConvertAll(id, int.Parse);
            var result = await _posService.AddSimulationPosition(ids, memId);
            if (result.result)
            {
                return Ok(ResultMode<string>.Success(result.msg, result.msg));
            }
            return Ok(ResultMode<string>.Failed(result.msg));
        }
        
        /// <summary>
        /// 微信发送，未测试
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> WxPushPos([FromForm] KeyValue keyValue)
        {
            string userIdStr = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            int.TryParse(userIdStr, out int id);
            var result = await _posService.GetWxPusRecordCount(keyValue.Id, id);
            if (!result.tf) //按钮不可见的情况下，调用了
            {
                return Ok(ResultMode<string>.Failed(result.msg));
            }

            
            var wxPushPosUrl = _configuration["WxPushPosUrl"];
            string userName = User.Claims.FirstOrDefault(c => c.Type == "userName")?.Value;
            string qstr = "WordKey=" + keyValue.Name + "&EplName=" + userName;
            qstr += "&Action=SysWxPushPos&MemID=" + keyValue.Id + "&EplId=" + userIdStr;
            var pushResult= "pushResult";//Ticke.GetPage(wxPushPosUrl, qstr);
            return Ok(ResultMode<string>.Success(pushResult, pushResult));
        }


        /// <summary>
        /// 职位发布效果 当前登录人，刷新所有发布中的岗位
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> RefreshAllBySalerId()
        {
            string userIdStr = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            int.TryParse(userIdStr, out int id);
            var s = await _posService.RefreshAllPosBySalerId(id);
            return Ok(ResultMode<string>.Success(s,s));
        }


    }
}
