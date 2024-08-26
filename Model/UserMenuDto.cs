using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// 用戶菜單
    /// </summary>
    public class UserMenuDto
    {
        public int MenuID { get; set; }
        public int ParentID { get; set; }
        public int Depth { get; set; }
        public int Sequence { get; set; }
        public string MenuName { get; set; }
        public string LinkUrl { get; set; }
    }
}
