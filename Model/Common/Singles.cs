using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Common
{
    /// <summary>
    /// 單個
    /// </summary>
    public class Singles: PagingModel
    {
        /// <summary>
        /// 條件
        /// </summary>
        public string filter { get; set; } = null!;
    }
}
