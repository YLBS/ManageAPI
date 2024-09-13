using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.SalesDepartment
{
    public class PubPerApplyPosInfo
    {
        public int MemId { get; set; }
        public string MemName { get; set; } = null!;
        public int PosId { get; set; }
        public string PosName { get; set; } = null!;
        public DateTime ReceiveDates { get; set; }
        public string ReceiveDate
        {
            get { return ReceiveDates.ToString("yyyy-M-d hh:mm"); }
        }
        public int CommendType { get; set; }
        public string CommendTypeStr
        {
            get
            {
                if (CommendType == 0)
                {
                    return "正常投递";
                }
                if (CommendType == 2)
                {
                    return "模拟应聘";
                }
                if (CommendType == 3)
                {
                    return "兼职应聘";
                }

                return "";
            }
        }

        public int MyUserId { get; set; }
        public string PerName { get; set; } = null!;

        public int MemFlag { get; set; }

        public string MemFlagStr { get { if (MemFlag == 0) return "已读"; return "未读"; } }
    }
}
