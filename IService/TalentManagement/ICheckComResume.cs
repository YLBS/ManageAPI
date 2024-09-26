using Model.ResumeSearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IService.TalentManagement
{
    public interface ICheckComResume
    {
        Task<IEnumerable<ResumeListInfo>> GetNewResumeListCollection(string filter, string orderField, string orderType);
    }
}
