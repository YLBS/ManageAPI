using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.SalesDepartment
{
    public class WxPusRecord
    {
        public int MemId { get; set; }
        public int EplId { get; set; }
        public string WxPushContext { get; set; } = null!;
        public string EplName { get; set; } = null!;
        public string Result { get; set; } = null!;
        public int SendType { get; set; }
    }
}
