using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.SalesDepartment
{
    public class PositionListInfo
    {
        /// <summary>
        /// 职位编号
        /// </summary>
        public int posId { get; set; }             
        public int memId { get; set; }
        public string memName { get; set; } = null!;
        public string posName { get; set; } = null!;          //职位名称
    }
}
