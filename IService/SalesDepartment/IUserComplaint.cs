using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.SalesDepartment;

namespace IService.SalesDepartment
{
    public interface IUserComplaint
    {
        /// <summary>
        /// 通过筛选条件获取投诉信息列表最新
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="order"></param>
        /// <param name="top"></param>
        /// <param name="totalCount">总投诉次数</param>
        /// <returns></returns>
        Task<(IEnumerable<UserComplaint> item, int count)> GetUserComplaintListNews(string filter,
            string order, int top,int totalCount);
        /// <summary>
        /// 通过职位ID获取投诉信息列表
        /// </summary>
        /// <param name="posId"></param>
        /// <returns></returns>
        Task<IEnumerable<UserComplaint>> GetUserComplaintListByPosId(int posId);
        Task<(bool tf,string msg)> GetUserComplaintByPosId(int posId,int sqlType);
        Task<int> DealWithUserComplaint(int posId, string processingDetail);
        Task<IEnumerable<ExplosiveCompany>> GetExplosiveCompanies(string filter);
    }
}
