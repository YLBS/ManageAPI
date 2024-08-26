using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.SalesDepartment
{
    /// <summary>
    /// 添加
    /// </summary>
    public class InputPubInformationDto
    {
        /// <summary>
        /// 企业ID
        /// </summary>
        public int MemId { get; set; }
        /// <summary>
        /// 发送人ID
        /// </summary>
        public int SalerId { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Subject { get; set; } = null!;
        public string Content { get; set; } = null!;
        /// <summary>
        /// 发送人姓名
        /// </summary>
        public string Issuer { get; set; } ="";
        /// <summary>
        /// 来源0客服留言，１企业留言
        /// </summary>
        public byte IssuedFrom { get; set; }

        public DateTime IssuedDate { get; set; } = DateTime.Now;
        public bool Read { get; set; } = false;
    }
}
