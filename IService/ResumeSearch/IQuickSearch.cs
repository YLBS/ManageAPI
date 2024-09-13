using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Model.ResumeSearch;
using Model.SalesDepartment;
using ResumeListQueryInfo = Model.ResumeSearch.ResumeListQueryInfo;

namespace IService.ResumeSearch
{
    public interface IQuickSearch
    {

        Task<IList<ResumeListQueryInfo>> GetData(string filter,string keyword, int orderBy);
        /// <summary>
        /// 排除推荐过的简历
        /// </summary>
        /// <param name="myUserId"></param>
        /// <param name="posId"></param>
        /// <returns></returns>
        Task<string> ExcludeRecommendResumeId(int[] myUserId, int posId);
        /// <summary>
        /// 排除下载过的简历
        /// </summary>
        /// <param name="myUserId"></param>
        /// <param name="posId"></param>
        /// <returns></returns>
        Task<string> ExcludeDownResumeId(int[] myUserId, int posId);

        Task<List<ResumeCheckingInfo>> GetResumeCheckingCollection(int[] myUserId);
        Task<int[]> JobFunctionSmall(int posId);
        /// <summary>
        /// 判断职位是否存在
        /// </summary>
        /// <param name="posId"></param>
        /// <returns></returns>
        Task<bool> CheckPosExist(int posId);
        
    }
}
