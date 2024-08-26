using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.SalesDepartment;

namespace IService.SalesDepartment
{
    public interface IMyCompany
    {
        Task<(IEnumerable<MySalesCompanyListInfo> item,int count)> GetData(string filter);
        Task<bool> AddPubInformation(InputPubInformationDto dto);
        Task<bool> EnableViewCompany(int memId, int userId, int[] departId);
    }
}
