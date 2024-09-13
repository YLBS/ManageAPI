using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.SalesDepartment
{
    public class ExplosiveCompany
    {
        public int MemId { get; set; }
        public string MemName { get; set; }
        public int MemState { get; set; }
        public string MemStateName
        {
            get
            {
                switch (MemState)
                {
                    case -1:
                        return "爆品过期会员";
                    case 0:
                        return "普通注册会员";
                    case 2:
                        return "正式会员";
                    default:
                        return "";
                }
            }
        }
        public DateTime EndValidDate { get; set; }
        public int PosCount { get; set; }
        /// <summary>
        /// 应聘人数
        /// </summary>
        public int ApplyCount { get; set; }
        /// <summary>
        /// 模拟应聘人数
        /// </summary>
        public int SimulateCount { get; set; }
        /// <summary>
        /// 最后登陆时间
        /// </summary>
        public DateTime LastLoginDate { get; set; }
    }
}
