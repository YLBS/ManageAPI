using Model.SalesDepartment;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IService.SalesDepartment
{
    public interface IMyServiceOrder
    {
        Task<IEnumerable<ServiceOrderInfo>> GetCompanyServiceDateCollection(string userId);
        Task<IEnumerable<PaymentOrderInfo>> GetPaymentCollection(string userId); 
    }
}
