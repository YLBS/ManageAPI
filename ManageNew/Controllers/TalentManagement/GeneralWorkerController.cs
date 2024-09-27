using IService.ResumeSearch;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace ManageNew.Controllers.TalentManagement
{
    /// <summary>
    /// 普工简历管理,控制器名称和 普工简历搜索 重复
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "人才管理")]
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
        /// 获取数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="addressP"></param>
        /// <param name="addressC"></param>
        /// <param name="addressD"></param>
        /// <param name="addressT"></param>
        /// <param name="keyWord"></param>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="orderBy"></param>
        /// <param name="orderType"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetData(int? type, int? addressP, int? addressC,
            int? addressD, int? addressT, string? keyWord, DateTime? beginDate, DateTime? endDate,int orderBy=1,int orderType=1)
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
                filter += " AND DATEDIFF(DAY, ReceiveDate, CONVERT(DATETIME,'" + beginDate + "')) < 0 ";
            }

            if (endDate != null)
                filter += " AND DATEDIFF(DAY, ReceiveDate, CONVERT(DATETIME,'" + endDate + "')) > -1 ";

            string order = orderBy == 1 ? " ID " : " ReceiveDate ";
            order += orderType == 1 ? "desc" : "asc";
            var s = await _geService.GE_GetResumesList(filter, order);

            int totalCount = await _geService.GetCountByFilter(filter);
            int readCount = await _geService.GetCountByFilter(" and readFlag=1 " + filter);
            int interview =  await _geService.GetCountByFilter(" and IsInterview=1 " + filter);
            return Ok(ResultMode<Object>.Success(new { Data = s, MemName = $"{totalCount},{s.Count()},{readCount},{interview}" }));
        }
        /// <summary>
        /// 获取数据，返回json
        /// </summary>
        /// <param name="type"></param>
        /// <param name="addressP"></param>
        /// <param name="addressC"></param>
        /// <param name="addressD"></param>
        /// <param name="addressT"></param>
        /// <param name="keyWord"></param>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="orderBy"></param>
        /// <param name="orderType"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetDataNew(int? type, int? addressP, int? addressC,
            int? addressD, int? addressT, string? keyWord, DateTime? beginDate, DateTime? endDate, int orderBy = 1, int orderType = 1)
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

            if (addressT != 0 && addressD.HasValue)
                filter += " and [Goodjob].[dbo].[GE_Resume].Address_T = " + addressT;
            if (beginDate != null)
            {
                filter += " AND DATEDIFF(DAY, ReceiveDate, CONVERT(DATETIME,'" + beginDate + "')) < 0 ";
            }

            if (endDate != null)
                filter += " AND DATEDIFF(DAY, ReceiveDate, CONVERT(DATETIME,'" + endDate + "')) > -1 ";

            string order = orderBy == 1 ? " ID " : " ReceiveDate ";
            order += orderType == 1 ? "desc" : "asc";
            var s = await _geService.GE_GetResumesList(filter, order);

            int totalCount = await _geService.GetCountByFilter(filter);
            int readCount = await _geService.GetCountByFilter(" and readFlag=1 " + filter);
            int interview = await _geService.GetCountByFilter(" and IsInterview=1 " + filter);
            return Ok(ResultMode<Object>.Success(new { Data = s, totalCount, resultCount = s.Count(),readCount,interview }));
        }
        /// <summary>
        /// 删除普工简历
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> DelResume(string ids)
        {
            try
            {
                string[] idss = ids.Split(',');
                Array.ConvertAll(idss, id => Convert.ToInt32(id));
            }
            catch
            {
                return BadRequest("参数错误");
            }

            int i = await _geService.Del_GEResumeByIds(ids);
            if(i == 0)
                return Ok(ResultMode<string>.Failed($"一共删除{i}份简历"));
            return Ok(ResultMode<string>.Success("",$"一共删除{i}份简历"));
        }
    }
}
