using IService.SalesDepartment;
using Model.SalesDepartment;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Entity.Goodjob;
using Microsoft.EntityFrameworkCore;

namespace Service.SalesDepartment
{
    public class MyServiceOrder : IMyServiceOrder
    {
        private readonly GoodjobContext _context;

        public MyServiceOrder(GoodjobContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ServiceOrderInfo>> GetCompanyServiceDateCollection(string userId)
        {
            string filter = $" AND MemID IN (SELECT MemID FROM Mem_Users WHERE SalerUserID = {userId})";
            var parameters = new { Filter = filter };
            var result = await _context.Database.GetDbConnection().QueryAsync<ServiceOrderInfo>("Mng_GetServiceOrderList", parameters, commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<IEnumerable<PaymentOrderInfo>> GetPaymentCollection(string userId)
        {
            string filter = $"  AND MemID IN (SELECT MemID FROM Mem_Users WHERE SalerUserID = 0)";
            var parameters = new { Filter = filter };
            var result = await _context.Database.GetDbConnection().QueryAsync<PaymentOrderInfo>("Mng_GetPaymentOrderList", parameters, commandType: CommandType.StoredProcedure);
            return result;
        }
    }
}
