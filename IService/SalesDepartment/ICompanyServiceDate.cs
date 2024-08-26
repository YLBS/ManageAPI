using Model.SalesDepartment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IService.SalesDepartment
{
    public interface ICompanyServiceDate
    {
        /// <summary>
        /// 获取企业服务期集合
        /// </summary>
        /// <returns></returns>
        Task<(IEnumerable<CompanyServiceDateInfo> item, int count)> GetCompanyServiceDateCollection(string filter);
    }
}
