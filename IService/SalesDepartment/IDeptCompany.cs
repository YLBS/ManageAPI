
using Model.SalesDepartment;

namespace IService.SalesDepartment
{
    public interface IDeptCompany
    {
        Task<(IEnumerable<SalesCompanyListInfo1> item,int totalRecords)> GetCompanyList(string filter);
    }
}
