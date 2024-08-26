using Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IService.Common
{
    /// <summary>
    /// 返回一些字典型
    /// </summary>
    public interface IDicService
    {
        /// <summary>
        /// 获取薪资
        /// </summary>
        /// <returns></returns>
        Task<List<KeyValue>> GetSalaryNew();
    }
}
