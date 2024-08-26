using System;
using System.Collections.Generic;

namespace Entity.GoodjobResum
{
    public partial class MyUser
    {
        /// <summary>
        /// 个人帐号编号
        /// </summary>
        public int MyUserId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; } = null!;
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; } = null!;
        /// <summary>
        /// 电子邮件地址
        /// </summary>
        public string Email { get; set; } = null!;
        /// <summary>
        /// 0=无英文简历，1=显示英文简历，2=隐藏英文简历
        /// </summary>
        public byte EngResumeFlag { get; set; }
        /// <summary>
        /// 0=简体内码，1=繁体内码			
        /// </summary>
        public byte ReceiveMailType { get; set; }
        /// <summary>
        /// 账号注册日期
        /// </summary>
        public DateTime RegisterDate { get; set; }
        /// <summary>
        /// 0=正常，1=隐藏简历，2=对指定的企业隐藏，3=待删除
        /// </summary>
        public byte ResumeStatus { get; set; }
        /// <summary>
        /// 0=公开，1=保密联系方式
        /// </summary>
        public byte ContactStatus { get; set; }
        /// <summary>
        /// 0=正试注册的(培训和教育及工作经历按表显示)，1=copy来的，按字段显示			
        /// </summary>
        public byte DisplayFlag { get; set; }
        /// <summary>
        /// 0=未完成注册，1=待审核，2=审核通过，3=审核不通过，4=更新后待复审
        /// </summary>
        public byte CheckFlag { get; set; }
        /// <summary>
        /// 注册来源
        /// </summary>
        public byte RegisterBy { get; set; }
        /// <summary>
        /// 登录次数
        /// </summary>
        public int LoginCount { get; set; }
        /// <summary>
        /// 最后登录日期
        /// </summary>
        public DateTime LastLoginDate { get; set; }
        public int HitCount { get; set; }
        public string PhoneNum { get; set; } = null!;
        public bool IsAuPhone { get; set; }
        public bool IsAuEmail { get; set; }
        public bool IsRefresh { get; set; }
        public int RegisterFrom { get; set; }
        public int BelongType { get; set; }
    }
}
