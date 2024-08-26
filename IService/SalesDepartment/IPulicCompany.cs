using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.SalesDepartment;

namespace IService.SalesDepartment
{
    public interface IPulicCompany
    {
        Task<IEnumerable<SalesCompanyListInfo>> GetSalesCompanyListInfo(SearchPulicCompany search);
        /// <summary>
        /// 获取返回企业招聘效果
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<EffectCompanyAdInfo>> GetCompanyAd(int memId);
        /// <summary>
        /// 获取企业信息
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<EffectCompanyInfo>> GetCompanyInfo(int memId);
    }
}
