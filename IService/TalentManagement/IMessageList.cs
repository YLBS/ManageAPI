using Model.TalentManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IService.TalentManagement
{
    public interface IMessageList
    {
        Task<IEnumerable<PersonMessageInfo>> GetSalerMessCollection(string filter);
        Task<bool> DeleteSalerMessage(int infoId);
    }
}
