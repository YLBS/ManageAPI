using System;
using System.Collections.Generic;

namespace Entity.GoodjobResum
{
    public partial class PubServiceHelp
    {
        public int Id { get; set; }
        /// <summary>
        /// 用户类型,0不是用户,1企业用户,2个人用户
        /// </summary>
        public byte UserType { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }
        public int HelpType { get; set; }
        public string HelpTitle { get; set; } = null!;
        public string HelpMemo { get; set; } = null!;
        public string Tel { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string ReplyMemo { get; set; } = null!;
        public bool IsReply { get; set; }
        public bool IsEnd { get; set; }
        public DateTime InsertDate { get; set; }
    }
}
