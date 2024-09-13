using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.ResumeSearch;

namespace IService.ResumeSearch
{
    /// <summary>
    /// 外部简历推荐
    /// </summary>
    public interface IResumeRecommend
    {
        Task<IEnumerable<ResumeListQueryInfo>> GetExtranetResumeRecommendList(string idStr);
        /// <summary>
        /// 添加特定
        /// </summary>
        /// <param name="memId"></param>
        /// <param name="myUserId"></param>
        /// <param name="operType">1为特定 0为本站</param>
        /// <returns></returns>
        Task<int> AddMemLibCommendNew(int memId, string myUserId, int operType, int userId);
    }
}
