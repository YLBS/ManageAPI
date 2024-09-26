using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.TalentManagement
{
    public class MySMSInfo
    {
        public MySMSInfo(){}
        public int ID { get; set; }
        public int MyUserId { get; set; }
        public string PerName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string MobileNum { get; set; }
        public DateTime SentTime { get; set; }
    }
}
