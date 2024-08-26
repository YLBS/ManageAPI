using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Common
{
    /// <summary>
    /// 发送短信
    /// </summary>
    public class SendSms
    {
        public string Phone { get; set; }
        public string Content { get; set; }
        public string SendTime { get; set; } = "";
    }
}
