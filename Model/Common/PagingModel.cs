using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Common
{
    /// <summary>
    /// 分页
    /// </summary>
    public class PagingModel
    {
        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { get; set; } = 1;
        /// <summary>
        /// 每页数量
        /// </summary>
        public int PageSize { get; set; } = 10;
    }
}
