using IService.ResumeSearch;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace ManageNew.Controllers.ResumeSearch
{
    /// <summary>
    /// 普工简历搜索 ,控制器名称和 普工简历管理 重复
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "简历搜索")]
    public class GeneralWorkerController : ControllerBase
    {
        private readonly IGEService _geService;
        /// <summary>
        /// 构造方法
        /// </summary>
        public GeneralWorkerController(IGEService geService)
        {
            _geService = geService;
        }

        /// <summary>
        /// 普工简历搜索獲取数据,NesGENYResumeList.aspx
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetDataForNesGENYResumeList(int? type, int? addressP, int? addressC, int? addressD, int? addressT, string? keyWord, DateTime? beginDate, DateTime? endDate)
        {
            string filter = string.Empty;
            if (type != 0 && !string.IsNullOrEmpty(keyWord))
            {
                switch (type)
                {
                    case 1:
                        filter = " and id=" + keyWord;
                        break;
                    case 2:
                        filter = " and Mobile = '" + keyWord + "' ";
                        break;
                    case 3:
                        filter = " and [Goodjob].[dbo].[GE_Resume].MemID = " + keyWord;
                        break;
                    case 4:
                        filter = " and [MemName] like '%" + keyWord + "%' ";
                        break;
                    case 5:
                        filter = " and [Goodjob].[dbo].[GE_Resume].PosID = " + keyWord;
                        break;
                    case 6:
                        filter = " and Goodjob.dbo.Mem_Position.[PosName] like '%" + keyWord + "%' ";
                        break;
                    case 7:
                        filter = " and Goodjob.dbo.GE_Resume.[EngWork]  like '%" + keyWord + "%' ";
                        break;
                    case 8:
                        filter = " and Goodjob.dbo.GE_Resume.[Skills] like '%" + keyWord + "%' ";
                        break;
                }
            }

            if (addressP != 0 && addressP.HasValue)
                filter += " and [Goodjob].[dbo].[GE_Resume].Address_P = " + addressP;

            if (addressD == 0 && addressD.HasValue) //区没有，选市
            {
                if (addressC != 0 && addressC < 1000)
                {
                    string cityStr = addressC.ToString();
                    cityStr = "," + cityStr + ",";
                    cityStr = cityStr.Replace(",724,", ",724,1,2,28,25,26,27,").Replace(",725,", ",725,3,4,45,46,47,").Replace(",10,", ",10,726,727,728,729,730,731,732,").Replace(",1,", ",1,724,");
                    cityStr = cityStr.Substring(1, cityStr.Length - 2);
                    filter += " and [Goodjob].[dbo].[GE_Resume].Address_C in( " + cityStr + ")";
                }

            }
            else
            {
                if (addressD < 1000 && addressD.HasValue)
                {
                    string cityStr = addressD.ToString();
                    cityStr = "," + cityStr + ",";
                    cityStr = cityStr.Replace(",724,", ",724,1,2,28,25,26,27,").Replace(",725,", ",725,3,4,45,46,47,").Replace(",10,", ",10,726,727,728,729,730,731,732,").Replace(",1,", ",1,724,");
                    cityStr = cityStr.Substring(1, cityStr.Length - 2);
                    filter += " and [Goodjob].[dbo].[GE_Resume].Address_C in( " + cityStr + ")";
                }
            }

            if (addressT != 0 && addressT.HasValue)
                filter += " and [Goodjob].[dbo].[GE_Resume].Address_T = " + addressT;
            if (beginDate != null)
            {
                filter += " and ReceiveDate > '" + beginDate + "'";
            }

            if (endDate != null)
                filter += " and ReceiveDate < '" + endDate + "'";
            string order = "ReceiveDate Desc";
            var s = await _geService.GE_GetResumesList(filter, order);
            return Ok(ResultMode<Object>.Success(new { Data = s, Count = s.Count() }));
        }
        /// <summary>
        /// 根据id获取普工简历详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetDataById(int id)
        {
            var s = await _geService.GE_GetResumeInfoById(id);
            return Ok(ResultMode<object>.Success(s));
        }
        
    }
}
