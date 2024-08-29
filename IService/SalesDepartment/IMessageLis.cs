using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.SalesDepartment;

namespace IService.SalesDepartment
{
    public interface IMessageLis
    {
        Task<IEnumerable<SalesMessageInfo>> GetData(SearchMessageLis searchMessageLis);
        Task<bool> DeleteDeleteMessage(int infoId, int userId);


    }
}
