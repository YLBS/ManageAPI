using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// 登录信息
    /// </summary>
    public class LoginInfo
    {
        /// <summary>
        /// 账号
        /// </summary>
        public string UserName { get; set; }=null!;
        /// <summary>
        /// 随机码
        /// </summary>
        public string Code { get; set; } = null!;
    }
}
