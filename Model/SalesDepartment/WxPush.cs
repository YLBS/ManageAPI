using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.SalesDepartment
{
    /// <summary>
    /// 微信推送相关的
    /// </summary>
    public class WxPush
    {
        public class WxPusRecord
        {
            public int MemId { get; set; }
            public int EplId { get; set; }
            public string WxPushContext { get; set; }
            public string EplName { get; set; }
            public string Result { get; set; }
            public int SendType { get; set; }
        }

        /// <summary>
        /// 微信发送限制
        /// </summary>
        public class WxPushLimit
        {
            public int Id { get; set; }
            /// <summary>
            /// 限制行业名称
            /// </summary>
            public string CalingName { get; set; }
            /// <summary>
            /// 限制企业名称关键字
            /// </summary>
            public string MemKeyWored { get; set; }
            /// <summary>
            /// 限制企业Id
            /// </summary>
            public string MemId { get; set; }
            /// <summary>
            /// 限制职位名称
            /// </summary>
            public string PosKeyWored { get; set; }
            /// <summary>
            /// 禁止地区
            /// </summary>
            public string RegionCId { get; set; }

            /// <summary>
            /// 每日次数
            /// </summary>
            public int DayCount { get; set; }
            /// <summary>
            /// 业务每日次数
            /// </summary>
            public int EplDayCount { get; set; }
            /// <summary>
            /// 是否开启地区关联
            /// </summary>
            public bool UseKeyWordFilter { get; set; }
            public int EplDayMinute { get; set; }
        }

        public class WxPushLimitKeyWrod
        {
            public int Id { get; set; }
            public int WxPLId { get; set; }
            public string KeyWords { get; set; }
            public string KeyWord { get; set; }
        }
    }
}
