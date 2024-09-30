using System.Runtime.CompilerServices;
using Entity.GoodjobResum;
using IService.TalentManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace ManageNew.Controllers.TalentManagement
{
    /// <summary>
    /// 推荐简历管理 ReCommendResume.aspx
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "人才管理")]
    public class ReCommendResumeController : ControllerBase
    {
        private readonly IReCommendResume _commendResume;
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="commendResume"></param>
        public ReCommendResumeController(IReCommendResume commendResume)
        {
            _commendResume = commendResume;
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="keyType">搜索选项</param>
        /// <param name="keyWord">关键字</param>
        /// <param name="workYear">工作年限</param>
        /// <param name="degree">学历</param>
        /// <param name="areaP">所在地区省</param>
        /// <param name="areaC">所在地区市</param>
        /// <param name="searchType">搜索目标,0 已推荐简历，1未推荐简历 </param>
        /// <param name="hopeWordId">期望岗位类型</param>
        /// <param name="hopeJobId">期望岗位</param>
        /// <param name="hopeAreaP">期望工作地区省</param>
        /// <param name="hopeAreaC">期望工作地区市</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetData(int keyType, string? keyWord, int? workYear, int? degree, int? areaP,
            int? areaC, int? searchType, int? hopeWordId, int? hopeJobId, int? hopeAreaP, int? hopeAreaC)
        {
            string filter=string.Empty;
            if (!string.IsNullOrEmpty(keyWord))
            {
                switch (keyType)
                {
                    case 1:
                        try
                        {
                            int id = int.Parse(keyWord);
                            filter = " AND MyUserID = " + id + " ";
                        }
                        catch (Exception)
                        {
                            return BadRequest("选择编号时，请输入纯数字");
                        }
                        break;
                    case 2:
                        filter = " AND MyUserID IN(SELECT MyUserID FROM [Goodjob].[dbo].[My_Resume] WHERE MobileNum LIKE '%" + keyWord + "%') ";
                        break;
                    case 3:
                        filter = " AND MyUserID IN(SELECT MyUserID FROM [Goodjob].[dbo].[My_Resume] WHERE PerName LIKE '%" + keyWord + "%') ";
                        break;
                }
            }
            if(workYear>0)
                filter+= " AND MyUserID IN(SELECT MyUserID FROM [Goodjob].[dbo].[My_Resume] WHERE WorkedYear >= " + workYear + ") ";
            if(degree>0)
                filter+= " AND MyUserID IN(SELECT MyUserID FROM [Goodjob].[dbo].[My_Resume] WHERE DegreeID >= " + degree + ") ";
            if (areaP != 0 && areaP.HasValue)
            {
                filter += " AND MyUserID IN(SELECT MyUserID FROM [Goodjob].[dbo].[My_Resume] WHERE Location_P = " + areaP;
                if (areaC != 0 && areaC.HasValue)
                {
                    string locC = areaC.ToString();
                    switch (areaC)
                    {
                        case 724:
                            locC = "1,2,25,26,27,28,724";
                            break;
                        case 725:
                            locC = "3,4,45,46,47,725";
                            break;
                        case 10:
                            locC = "10,726,727,728,729,730,731,732,733";
                            break;
                    }
                    filter+= " AND Location_C IN(" + locC + ") ";
                }

                filter += ") ";
            }

            if (hopeWordId != 0 && hopeWordId.HasValue)
            {
                if(hopeJobId>0)
                    filter += " AND MyUserID IN(SELECT MyUserID FROM [Goodjob].[dbo].[My_JobFunction] WHERE JobFunction_small = " + hopeJobId + ") ";
                else
                    filter+= " AND MyUserID IN(SELECT MyUserID FROM [Goodjob].[dbo].[My_JobFunction] WHERE JobFunction_big = " + hopeWordId + ") ";
            }

            if (hopeAreaP!= 0 && hopeAreaP.HasValue)
            {
                
                if (hopeAreaC != 0 && hopeAreaC.HasValue)
                {
                    string locC = hopeAreaC.ToString();
                    switch (areaC)
                    {
                        case 724:
                            locC = "1,2,25,26,27,28,724";
                            break;
                        case 725:
                            locC = "3,4,45,46,47,725";
                            break;
                        case 10:
                            locC = "10,726,727,728,729,730,731,732,733";
                            break;
                    }
                    filter += "  AND MyUserID IN(SELECT MyUserID FROM [Goodjob].[dbo].[My_JobLocation] WHERE JobLocation_C IN( " + locC + ")) ";
                }
                else
                    filter += " AND MyUserID IN(SELECT MyUserID FROM [Goodjob].[dbo].[My_JobLocation] WHERE JobLocation_P = " + hopeAreaP + ") ";
            }


            string orderBy = " ORDER BY ComDate DESC  ";
            string top = string.Empty;
            if (searchType == 1)
            {
                orderBy = " ORDER BY UpdateDate DESC ";
                top = " top 1000";
            }

            var s = await _commendResume.GetRecommendResumeList(top, filter, orderBy, searchType == 1);
            return Ok(ResultMode<object>.Success(new { Data = s, Count = s.Count() }));
        }

        /// <summary>
        /// 推荐,Code为1 推荐失败，0 推荐成功
        /// </summary>
        /// <param name="ids">简历集合 1,2,3</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> InsertReCommend(string ids)
        {
            bool yn = false;
            int i = 0;
            string[] ss = ids.Split(",");
            foreach (var s in ss)
            {
                int.TryParse(s.Trim(), out int id);
                if (id != 0)
                {
                    bool y = await _commendResume.InsertReComMyUserId(id);
                    if (yn)
                    {
                        i++;
                    }
                    if (!yn && y)
                    {
                        yn = y;
                    }
                }
            }
            if (yn)
                return Ok(ResultMode<string>.Success(string.Empty, $"{i}条数据推荐成功"));
            return Ok(ResultMode<string>.Failed("推荐失败"));
        }
        /// <summary>
        /// 取消推荐
        /// </summary>
        /// <param name="ids">简历集合 1,2,3</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> CancelReCommend(string ids)
        {
            try
            {
                string[] ss = ids.Split(",");
                int[] id = Array.ConvertAll(ids.Split(","), int.Parse);
                string myUserIds=string.Join(",", id);
                int i = await _commendResume.DeleteReComMyUserId(myUserIds);
                return Ok(ResultMode<string>.Success(string.Empty, $"{i}条数据取消推荐成功"));
            }
            catch (Exception e)
            {
                return BadRequest("参数错误");
            }
        }
    }
}
