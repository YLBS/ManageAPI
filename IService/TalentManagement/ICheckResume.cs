using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.TalentManagement;

namespace IService.TalentManagement
{
    public interface ICheckResume
    {
        Task<IEnumerable<CheckingResumeInfo>> GetCheckResume(string filter);
        Task<int> InitData();
        Task<int> Count();
        Task<bool> CheckResume(int myUserId, int checkFlag);
        /// <summary>
        /// 获取账号密码
        /// </summary>
        /// <param name="myUserId"></param>
        /// <returns></returns>
        Task<(string userName, string passWord)> GetUserNameAndPassWord(int myUserId);
        Task<int> InsertReComMyUserId(int myUserId);
    }
}
