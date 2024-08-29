using Entity.Goodjob;
using IService.SalesDepartment;
using Model.SalesDepartment;
using System.Data;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace Service.SalesDepartment
{
    public class DeptCompany : IDeptCompany
    {

        private readonly GoodjobContext _context;
        public DeptCompany(GoodjobContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<SalesCompanyListInfo1> item, int totalRecords)> GetCompanyList(string filter)
        {
            var parm = new { Filter = filter };
            var multiReader = await _context.Database.GetDbConnection().QueryMultipleAsync("Sales_GetCompanyList", parm, commandType: CommandType.StoredProcedure);
            var count = multiReader.Read<int>().ToList().FirstOrDefault();
            if (count == 0)
            {
                multiReader.Dispose();
                return (null, 0);
            }
            var result = multiReader.Read<SalesCompanyListInfo1>();
            return (result, count);

            throw new NotImplementedException();
        }
    }
}
