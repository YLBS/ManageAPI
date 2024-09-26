using Model.TalentManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Common;

namespace IService.TalentManagement
{
    public interface ISentHistroy
    {
        Task<(IEnumerable<MySMSInfo> data, int count)> GetSentSMSUsersList(PagingModel paging, string filter, string order);
    }
}
