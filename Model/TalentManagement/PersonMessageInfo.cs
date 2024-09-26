using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.TalentManagement
{
    /// <summary>
    /// 留言人才记录
    /// </summary>
    public class PersonMessageInfo
    {
        private string PerName;
        public int InfoId { get; set; }
        public int ClerkID { get; set; }
        public int MyUserID { get; set; }
        public string PersonName
        {
            get { return PerName; }
            set { PerName = value; }
        }
        public string Subject { get; set; }
        public string IsSuer { get; set; }
        public DateTime IssuedDate { get; set; }
        public string IssuedDateStr
        {
            get { return IssuedDate.ToString("g"); }
        }
        public bool Read { get;set; }
        public string ReadStr
        {
            get
            {
                if (Read)
                    return "已读";
                return "未读";
            }
        }
    }
}
