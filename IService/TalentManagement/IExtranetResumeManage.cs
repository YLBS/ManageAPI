using Model.TalentManagement;

namespace IService.TalentManagement
{
    /// <summary>
    /// 外网简历导入管理接口
    /// </summary>
    public interface IExtranetResumeManage
    {
        Task<List<ResumeNewListInfo>> GetData(string filter);
        /// <summary>
        /// 获取简历部分信息
        /// 原方法 GetUserNamePassWordEmail GetUserNamePassWordPhone
        /// </summary>
        /// <param name="myUserIds"></param>
        /// <returns></returns>
        Task<List<MyUserInfo>> GetMyUserInfoAsync(int[] myUserIds);
    }
}
