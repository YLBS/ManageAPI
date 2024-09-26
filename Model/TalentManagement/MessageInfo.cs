using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.TalentManagement
{
    /// <summary>
    /// 给人才留言模板
    /// </summary>
    public class MessageInfo
    {
        /// <summary>
        /// 人才Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 主题
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
    }
}
