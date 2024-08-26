using System;
using System.Collections.Generic;

namespace Entity.GoodjobResum
{
    public partial class MySendPhoneCount
    {
        public int MyUserId { get; set; }
        public int SendCount { get; set; }
        public bool? IsSendPhone { get; set; }
        public DateTime? FirstSendTime { get; set; }
        public DateTime? LastSendTime { get; set; }
    }
}
