using Model.TalentManagement;

namespace IService.TalentManagement
{
    public interface IReCommendResume
    {
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="getTop"></param>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="searchType"></param>
        /// <returns></returns>
        Task<IEnumerable<ReCommendResumeInfo>> GetRecommendResumeList(string getTop, string filter, string orderBy, bool searchType);
        /// <summary>
        /// 推荐简历
        /// </summary>
        /// <param name="myUserId"></param>
        /// <returns></returns>
        Task<bool> InsertReComMyUserId(int myUserId);
        /// <summary>
        /// 取消推荐
        /// </summary>
        /// <param name="myUserIds"></param>
        /// <returns></returns>
        Task<int> DeleteReComMyUserId(string myUserIds);
    }
}
