using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.SalesDepartment
{
    /// <summary>
    /// 搜索留言信息列表
    /// </summary>
    public class SearchMessageLis
    {
        /// <summary>
        /// 当前登录人ID
        /// </summary>
        public string? SalerID { get; set; }
        /// <summary>
        /// 企业ID
        /// </summary>
        public string? MemID { get; set; }
        /// <summary>
        /// 企业名称
        /// </summary>
        public string? MemName { get; set; }
        /// <summary>
        /// 主题，标题
        /// </summary>
        public string? Subject { get; set; }
        /// <summary>
        /// 阅读状态
        /// </summary>
        public string? Read { get; set; }
        /// <summary>
        /// 发送方
        /// </summary>
        public string? Sender { get; set; }
    }
}
