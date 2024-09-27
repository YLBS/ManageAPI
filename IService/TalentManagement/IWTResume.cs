using System.Data;
using Entity.Goodjob;
using Model.TalentManagement;

namespace IService.TalentManagement
{
    public  interface IWTResume
    {
        Task<(IEnumerable<HtrApplyListDto> table, int count)> GetTableAsync(string where, string orderBy);
        /// <summary>
        /// 获取委托注册的简历的账号信息
        /// </summary>
        /// <param name="mobileNum"></param>
        /// <returns></returns>
        Task<(string userName, string password)> GetMyUserAccount(string mobileNum);

        Task<bool> UpdateDealWithStatus(int id, int myUserId);
        /// <summary>
        /// 返回手机号码
        /// </summary>
        /// <param name="myUserId"></param>
        /// <returns></returns>
        Task<string> GetAllWriteName(int myUserId);

        Task<string> GetResumeInfoById(int id);
    }
}
