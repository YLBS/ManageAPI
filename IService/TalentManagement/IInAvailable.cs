using Model.TalentManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IService.TalentManagement
{
    public interface IInAvailable
    {
        Task<IEnumerable<InAvailableMobileInfo>> GetData(string filter, string order = "desc");
        /// <summary>
        /// 更新处理状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="contact"></param>
        /// <returns></returns>
        Task<int> UpdateDealStatue(int id, string contact);
    }
}
