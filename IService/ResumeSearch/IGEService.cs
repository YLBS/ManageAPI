﻿using Model.ResumeSearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IService.ResumeSearch
{
    /// <summary>
    /// 普工简历
    /// </summary>
    public interface IGEService
    {
        /// <summary>
        /// 普工简历搜索
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        Task<IEnumerable<GEResumeInfo>> GE_GetResumesList(string filter, string order);
        /// <summary>
        /// 获取普工简历
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<GEResumeInfo> GE_GetResumeInfoById(int id);
        /// <summary>
        /// 根据条件，获取简历数量
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>

        Task<int> GetCountByFilter(string filter);
        /// <summary>
        /// 删除普工简历
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> Del_GEResumeByIds(string ids);
    }
}
