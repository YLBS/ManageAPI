﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.TalentManagement
{
    /// <summary>
    /// 简历部分信息
    /// </summary>
    public partial class MyUserInfo
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int MyUserId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string? UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string? Password { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string? Email { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string? Phone { get; set; }
    }
}
