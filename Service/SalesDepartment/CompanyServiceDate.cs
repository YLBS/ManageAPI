using IService.SalesDepartment;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Entity.Goodjob;
using Microsoft.EntityFrameworkCore;
using Model.SalesDepartment;

namespace Service.SalesDepartment
{
    public class CompanyServiceDate : ICompanyServiceDate
    {
        private readonly GoodjobContext _context;
        public CompanyServiceDate(GoodjobContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<CompanyServiceDateInfo> item,int count)> GetCompanyServiceDateCollection(string filter)
        {
            var parm = new { Filter = filter };
            var multiReader = await _context.Database.GetDbConnection().QueryMultipleAsync("Sales_CompanyServiceDate_Count", parm, commandType: CommandType.StoredProcedure);
            var count = multiReader.Read<int>().ToList().FirstOrDefault();
            if (count == 0)
            {
                return (null, 0);
            }
            var result = multiReader.Read<CompanyServiceDateInfo>();
            multiReader.Dispose();
            return (result, count);
        }
    }
}
