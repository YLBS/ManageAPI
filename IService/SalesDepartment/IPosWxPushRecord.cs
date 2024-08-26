using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.SalesDepartment;

namespace IService.SalesDepartment
{
    public interface IPosWxPushRecord
    {
        Task<IEnumerable<WxPusRecordInfo>> GetPusRecordInfosAsync(string filter, string elpName);
    }
}
