using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Common
{
    /// <summary>
    /// 键值对
    /// </summary>
    public class KeyValue
    {
        /// <summary>
        /// 键
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string Name { get; set; } = null!;

    }
}
