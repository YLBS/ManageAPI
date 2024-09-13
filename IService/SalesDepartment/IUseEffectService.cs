using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Common;
using Model.SalesDepartment;

namespace IService.SalesDepartment
{
    public interface IUseEffectService
    {
        /// <summary>
        /// 获取各种投递简历的数量
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="memName"></param>
        /// <param name="memId"></param>
        /// <returns></returns>
        Task<int[]> GetPerApplyPosInfos(DateTime? beginDate, DateTime? endDate, string? memName, int memId);
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="memName"></param>
        /// <param name="memId"></param>
        /// <param name="commendType"></param>
        /// <param name="paging"></param>
        /// <returns></returns>
        Task<IEnumerable<PubPerApplyPosInfo>> GetPerApplyPosInfos(DateTime? beginDate, DateTime? endDate, string? memName, int memId,int commendType,PagingModel paging);
        /// <summary>
        /// 单个职位类别的职位
        /// </summary>
        /// <returns></returns>
        Task<(IEnumerable<EffectPositionInfo> effect,int count)> GetOneJobFunctionCollection(PagingModel paging,string filter);
    }
}
