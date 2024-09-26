using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.TalentManagement
{
    /// <summary>
    /// 添加留言信息
    /// </summary>
    public class AddMessageInfo: MessageInfo
    {
        /// <summary>
        /// 留言者Id
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 留言者姓名
        /// </summary>
        public string UserName { get; set; }
    }
}
