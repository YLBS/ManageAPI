using Goodjob.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ResumeSearch
{
    /// <summary>
    /// 
    /// </summary>
    public class ResumeCheckingInfo
    {
        /// <summary>
        /// 简历Id
        /// </summary>
        public int MyUserID
        {
            get;
            set;
        }
        /// <summary>
        /// 姓名
        /// </summary>
        public string PerName
        {
            get;
            set;
        }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email
        {
            get;
            set;
        }
    }
}
