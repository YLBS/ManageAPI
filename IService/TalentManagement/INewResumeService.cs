using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.ResumeSearch;
using Model.TalentManagement;

namespace IService.TalentManagement
{
    public interface INewResumeService
    {
        /// <summary>
        /// 获取新简历集合
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderField"></param>
        /// <param name="orderType"></param>
        /// <returns></returns>
        Task<IList<ResumeListInfo>> GetNewResumeListCollection(string filter,string orderField,string orderType);
        /// <summary>
        /// SendMessage.aspx 给人才留言
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        Task InsertPersonMessage(AddMessageInfo info);
        /// <summary>
        /// 新注冊不完整简历，删除简历
        /// </summary>
        /// <param name="myUserId"></param>
        /// <returns></returns>
        Task DeleteResume(int myUserId);
        /// <summary>
        /// 通过用户ID获取用户短讯信息,包括(PerName,UserName,Password)
        /// </summary>
        /// <param name="myUserId"></param>
        /// <returns></returns>
        Task<MySMSInfo> GetMySMSInfo(int myUserId);
        /// <summary>
        /// 添加新的用户短讯信息,包括(MyUserID,UserName,Psssword,SentTime)
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        Task<bool> CreateMySMS(MySMSInfo info);
        /// <summary>
        /// 更新简历为已去电
        /// </summary>
        /// <param name="myUserId"></param>
        /// <returns></returns>
        Task<bool> UpdateResumeIsqd(int myUserId);
        /// <summary>
        /// 增加简历去电记录
        /// </summary>
        /// <param name="myUserId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> AddqdRecord(int myUserId,int userId);
        /// <summary>
        /// 判断是否发送过短信
        /// </summary>
        /// <param name="myUserId"></param>
        /// <returns></returns>

        Task<bool> GetSendingLog(int myUserId);
    }
}
