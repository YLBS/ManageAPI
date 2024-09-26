using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IService.TalentManagement
{
    public interface IResumeComplete
    {
        /// <summary>
        /// 添加去电
        /// </summary>
        /// <param name="myUserId"></param>
        /// <param name="describe"></param>
        /// <param name="qdType"></param>
        /// <returns></returns>
        Task<bool> InsertCompleteQd(int myUserId, string describe, int qdType);
    }
}
