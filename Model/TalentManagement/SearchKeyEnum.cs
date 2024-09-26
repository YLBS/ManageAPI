using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.TalentManagement;
/// <summary>
/// ResumeSearcher.ascx，搜索关键字类型
/// </summary>
public enum ResumeKeywordTypes
{
    /// <summary>
    /// 姓名
    /// </summary>
    PerName = 1,
    /// <summary>
    /// 用戶名
    /// </summary>
    UserName = 2,
    /// <summary>
    /// 简历编号
    /// </summary>
    MyUserId = 3,
    /// <summary>
    /// 电子邮件
    /// </summary>
    Email = 4,
    /// <summary>
    /// 手机号码
    /// </summary>
    MobileNum = 5,
}


