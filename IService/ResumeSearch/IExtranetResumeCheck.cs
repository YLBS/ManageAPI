using Model.ResumeSearch;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IService.ResumeSearch
{
    /// <summary>
    /// 外网简历核对
    /// </summary>
    public interface IExtranetResumeCheck
    {
        /// <summary>
        /// 获取外网简历核对表
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        Task<IEnumerable<ResumeListInfo>> CreateNewsResumeListCollection(string filter, string orderBy);
        /// <summary>
        /// 刷新简历
        /// </summary>
        /// <param name="myUserIds">简历Id,多个以逗号分割</param>
        /// <returns></returns>
        Task<bool> RefreshResume(string myUserIds);
        /// <summary>
        /// 更新简历公开设置
        /// </summary>
        /// <param name="myUserId">人才编号</param>
        /// <param name="resumeStatus">简历的状态</param>
        /// <param name="partComName">屏蔽公司关键字(以,隔开)</param>
        /// <param name="engResumeFlag">是否显示英文简历</param>
        /// <returns></returns>
        Task<bool> UpdateResumeStatus(int myUserId, short resumeStatus, string partComName, short engResumeFlag);
        /// <summary>
        /// 获取工作经历
        /// </summary>
        /// <param name="myUserId"></param>
        /// <returns></returns>
        Task<string> GetWorkText(int myUserId);
        /// <summary>
        /// 获取账号密码
        /// </summary>
        /// <param name="myUserId"></param>
        /// <returns></returns>
        Task<DictionaryEntry> GetUserPass(int myUserId);
        /// <summary>
        /// 修改工作经历
        /// </summary>
        /// <returns></returns>
        Task<bool> UpdateWorkText(int myUserId, string workText); 
    }
}
