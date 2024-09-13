using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Common;
using Model.SalesDepartment;

namespace IService.SalesDepartment
{
    public interface IJobEffect
    {
        Task<(IList<EffectPositionInfo> item, int count)> GetJobEffectCollection(string filter, string order);
        Task<IList<KeyValue>> CooperativeList(int userId, int state);
        Task<(IList<EffectPositionInfo> item, int count)> NewGetJobEffectCollection(string filter, string order);
    }
}
